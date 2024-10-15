using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Models.ViewModel;
using NLog;
using TimeAide.Data;

namespace TimeAide.CRAutoCancellation
{
   
    public class CRAutoCancellationHelper
    {
        private TimeAideContext timeAideWebDBContext = null;
        private TimeAideWindowContext timeAideWinDBContext = null;
        private Logger logger = null;
        public CRAutoCancellationHelper()
        {

            timeAideWebDBContext = new TimeAideContext();
            logger = LogManager.GetCurrentClassLogger();
        }
        public void RunCRAutoProcess()
        {
            DateTime executionDate;
           var exeDteStr= ConfigurationManager.AppSettings["executionDate"].ToString();
           var res  = DateTime.TryParse(exeDteStr,out executionDate);
            if(!res) executionDate = DateTime.Today;
            //WriteProcessLog("CR Auto Process started");
            logger.Info(string.Format("Starting Timestamp:{0} {1}", executionDate.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            logger.Info("CR Auto Process started");
            //  WriteProcessLog(string.Format("Starting Timestamp: {0}",DateTime.Now));
            //  WriteProcessLog("Logging Process detail");
            logger.Info("Logging Process detail");
            try
            {
                var clientList = timeAideWebDBContext.Client.Where(w => w.DataEntryStatus == 1).ToList();
                foreach (var client in clientList)
                {
                    try
                    {
                        timeAideWinDBContext = DataHelper.GetClientTAWinEFContext(client.Id);
                        // WriteProcessLog(string.Format("Processing Client: {0}", client.ClientName));
                        logger.Info(string.Format("Processing Client: {0}", client.ClientName));
                        var timeOffRequestList = timeAideWebDBContext.SP_TimeOffRequestAutoCancellation<EmployeeTimeOffRequestViewModel>(client.Id, executionDate).OrderBy(o => o.ProcessType);
                        foreach (var timeOffRequest in timeOffRequestList)
                        {
                            // WriteProcessLog(string.Format("Processing Time-Off Request Type:: {0}", timeOffRequest.ProcessType));
                            logger.Info(string.Format("Processing Time-Off Request Id {0} of Type: {1}", timeOffRequest.EmployeeTimeOffRequestId, timeOffRequest.ProcessType));
                            var processChangeRequest = timeAideWebDBContext.EmployeeTimeOffRequest.Find(timeOffRequest.EmployeeTimeOffRequestId);
                            WorkflowTriggerRequest workflowTriggerRequest = timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.EmployeeTimeOffRequestId == processChangeRequest.Id);
                            try
                            {
                                var IsValid= DataHelper.fnComp_CheckTimeOffValidation(timeAideWinDBContext, processChangeRequest.UserInformation.EmployeeId.Value, processChangeRequest.StartDate.Value);
                                timeOffRequest.ProcessType = IsValid == false ? "Auto-Cancel" : timeOffRequest.ProcessType;
                                var result = WorkflowService.ProcessRequestAutoCancellation(timeAideWebDBContext, workflowTriggerRequest, processChangeRequest, timeOffRequest.ProcessType);
                                logger.Info(string.Format("Processing result:{0}", result));
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.Message);
                            }
                        }
                        var changeRequestList = timeAideWebDBContext.SP_CRAutoCancellation<ChangeRequestViewModel>(client.Id, executionDate);
                        foreach (var changeRequest in changeRequestList)
                        {
                            // WriteProcessLog(string.Format("Processing Time-Off Request Type:: {0}", timeOffRequest.ProcessType));
                            logger.Info(string.Format("Processing {0} Request Id {1} of Type: {2}", changeRequest.RequestType, changeRequest.ReferenceId, changeRequest.ProcessType));
                            ChangeRequestBase processChangeRequest = null;
                            //timeAideWebDBContext.EmployeeTimeOffRequest.Find(timeOffRequest.EmployeeTimeOffRequestId);
                            WorkflowTriggerRequest workflowTriggerRequest = null;
                            //= timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.EmployeeTimeOffRequestId == processChangeRequest.Id);
                            switch (changeRequest.RequestType)
                            {
                                case "CRAddress":
                                    processChangeRequest = timeAideWebDBContext.ChangeRequestAddress.Find(changeRequest.ReferenceId);
                                    workflowTriggerRequest = timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestAddressId == processChangeRequest.Id);
                                    break;
                                case "CREmailNumbers":
                                    processChangeRequest = timeAideWebDBContext.ChangeRequestEmailNumbers.Find(changeRequest.ReferenceId);
                                    workflowTriggerRequest = timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestEmailNumbersId == processChangeRequest.Id);
                                    break;
                                case "CREmergencyContact":
                                    processChangeRequest = timeAideWebDBContext.ChangeRequestEmergencyContact.Find(changeRequest.ReferenceId);
                                    workflowTriggerRequest = timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestEmergencyContactId == processChangeRequest.Id);
                                    break;
                                case "CREmployeeDependent":
                                    processChangeRequest = timeAideWebDBContext.ChangeRequestEmployeeDependent.Find(changeRequest.ReferenceId);
                                    workflowTriggerRequest = timeAideWebDBContext.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestEmployeeDependentId == processChangeRequest.Id);
                                    break;
                            }

                            try
                            {
                                var result = WorkflowService.ProcessRequestAutoCancellation(timeAideWebDBContext, workflowTriggerRequest, processChangeRequest, changeRequest.ProcessType);
                                logger.Info(string.Format("Processing result:{0}", result));
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.Message);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                    // WriteProcessLog(string.Format("Finished Processing Client: {0}", client.ClientName));
                    logger.Info(string.Format("Finished Processing Client: {0}", client.ClientName));
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            // WriteProcessLog(string.Format("Ending Timestamp: {0}", DateTime.Now));
            logger.Info("Process Ended");
            logger.Info("-----------------------------------------------------------------------");
           // Console.WriteLine("Press any key to close...");
           // Console.Read();
        }
     
    }
}
