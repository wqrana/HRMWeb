using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Models.ViewModel;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using static System.Collections.Specialized.BitVector32;

namespace TimeAide.Services
{
    public class WorkflowService
    {
        public static TimeAideWindowContext TimeAideWindowContext { get; set; }
        public enum ChangeRequestTriggerType
        {
            CHGREQ_PERSONAL_INFO = 1,
            CHGREQ_EMPLOYMENT_INFO,
            CHGREQ_CONTACT_NUMBERS,
            CHGREQ_EMERGNY_CONTACTS,
            CHGREQ_EMP_DEPENDENTS,
            CHGREQ_EMP_ADDRESS,
            EMP_TIMEOFF_REQ
        }


        public static List<BellIconNotificationViewModel> GetNewWorkFlowNotifications()
        {
            using (TimeAideContext db = new TimeAideContext())
            {

                var PendingNotificationList = db.SP_GetPendingNotifications<BellIconNotificationViewModel>(SessionHelper.LoginId, SessionHelper.SelectedClientId).Result;

                return PendingNotificationList.OrderByDescending(n => n.CreatedDate).ToList();
            }
        }

        public static List<BellIconNotificationViewModel> GetNewWorkFlowClosingNotifications(Boolean unread, NotificationFilterViewModel notificationFilterViewModel)
        {
            using (TimeAideContext db = new TimeAideContext())
            {
                // var ClosedNotificationList = db.SP_GetClosedNotifications<BellIconNotificationViewModel>(SessionHelper.LoginEmail, SessionHelper.SelectedClientId, unread, SessionHelper.LoginId);
                List<BellIconNotificationViewModel> ClosedNotificationList = null;
                ClosedNotificationList = db.SP_GetClosedNotifications<BellIconNotificationViewModel>(SessionHelper.LoginEmail, SessionHelper.SelectedClientId, unread, SessionHelper.LoginId, notificationFilterViewModel).Result;
                return ClosedNotificationList.OrderByDescending(n => n.CreatedDate).ToList();
            }
        }


        public static List<BellIconNotificationViewModel> GetNewWorkFlowNotificationHistory(NotificationFilterViewModel filterModel)
        {
            using (TimeAideContext db = new TimeAideContext())
            {

                var PendingNotificationList = db.SP_GetNotificationsHistory<BellIconNotificationViewModel>(SessionHelper.LoginEmail, SessionHelper.SelectedClientId,filterModel).Result;

                return PendingNotificationList.OrderByDescending(n => n.CreatedDate).ToList();
            }
        }


        public static List<WorkflowTriggerRequestDetail> GetWorkFlowNotifications()
        {
            TimeAideContext db = new TimeAideContext();





            var res = from element in db.WorkflowTriggerRequestDetail
                      where element.ClientId == SessionHelper.SelectedClientId
                      group element by element.WorkflowTriggerRequestId
                  into groups
                      select groups.OrderByDescending(p => p.Id).FirstOrDefault();

            List<WorkflowTriggerRequestDetail> list = new List<WorkflowTriggerRequestDetail>();
            foreach (var each in res.ToList().Where(element => element.WorkflowTriggerRequest.ChangeRequest.ChangeRequestStatusId < 2))
            {

                if (each.WorkflowLevel.WorkflowLevelTypeId == 1)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(each.WorkflowTriggerRequest.ChangeRequest.UserInformationId, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                }

                if (each.WorkflowLevel.WorkflowLevelGroup.Count > 0)
                {
                    var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                }
            }

            return list.OrderByDescending(n => n.WorkflowTriggerRequest.CreatedDate).ToList();
        }
        public static List<WorkflowTriggerRequestDetail> GetWorkFlowNotificationHistory()
        {
            TimeAideContext db = new TimeAideContext();
            tReportWeek model = new tReportWeek();

            //string employeeList = db.sp_GetSupervisedEmployeeIds(SessionHelper.SelectedClientId, SessionHelper.LoginId, model, "sp_GetSupervisedEmployees");
            //List<int> supervisedEmployees = new List<int>();
            //if (string.IsNullOrEmpty(employeeList))
            //    supervisedEmployees = employeeList.Split(',').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();

            List<int> employeeGroupId = db.UserEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.EmployeeGroup.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == SessionHelper.LoginId)
                                          .Select(c => c.EmployeeGroupId).ToList();

            var res = from element in db.WorkflowTriggerRequestDetail
                      where element.ClientId == SessionHelper.SelectedClientId
                      select element;

            List<WorkflowTriggerRequestDetail> list = new List<WorkflowTriggerRequestDetail>();
            foreach (var each in res.ToList().Where(element => element.WorkflowActionTypeId != 6))
            {
                if (each.WorkflowLevel == null)
                    continue;
                if (each.WorkflowLevel.WorkflowLevelTypeId == 1)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(each.WorkflowTriggerRequest.ChangeRequest.UserInformationId, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                }
                if (each.WorkflowLevel.WorkflowLevelGroup.Count > 0)
                {
                    var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                }
            }

            return list;
        }
        public static List<WorkflowTriggerRequestDetail> GetWorkFlowClosingNotifications(Boolean unread)
        {
            TimeAideContext db = new TimeAideContext();

            var res = from element in db.WorkflowTriggerRequestDetail
                      where element.ClientId == SessionHelper.SelectedClientId
                      //where element.ClientId == SessionHelper.SelectedClientId
                      group element by element.WorkflowTriggerRequestId
                  into groups
                      select groups.OrderByDescending(p => p.Id).FirstOrDefault();

            List<WorkflowTriggerRequestDetail> list = new List<WorkflowTriggerRequestDetail>();

            foreach (var each in res.ToList().Where(element => element.WorkflowTriggerRequest.ChangeRequest.ChangeRequestStatusId >= 2))
            {
                if (each.WorkflowActionTypeId != 6 || each.WorkflowLevel == null)
                    continue;
                if (each.WorkflowLevel.Workflow.IsZeroLevel)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(each.WorkflowTriggerRequest.ChangeRequest.UserInformationId, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                }
                else if (each.WorkflowLevel.Workflow.ClosingNotificationId == (int)ClosingNotificationTypes.Employees)
                {
                    if (each.WorkflowTriggerRequest.ChangeRequest.UserInformationId == SessionHelper.LoginId)
                        list.Add(each);
                }
                else if (each.WorkflowLevel.Workflow.ClosingNotificationId == (int)ClosingNotificationTypes.EmployeeAndApprovers)
                {
                    var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                    if (each.WorkflowTriggerRequest.ChangeRequest.UserInformationId == SessionHelper.LoginId)
                        list.Add(each);
                }
                else if (each.WorkflowLevel.Workflow.ClosingNotificationId == (int)ClosingNotificationTypes.All)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(each.WorkflowTriggerRequest.ChangeRequest.UserInformationId, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                    userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                    if (userInformationList.Any(u => u.Id == SessionHelper.LoginId && u.Id != each.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        if (!list.Contains(each))
                            list.Add(each);
                    }
                    if (each.WorkflowTriggerRequest.ChangeRequest.UserInformationId == SessionHelper.LoginId)
                        list.Add(each);
                }

            }

            if (unread)
            {
                list = (from item in list
                        where !item.WorkflowTriggerRequest.NotificationLogMessageReadBy.Any(i => i.ReadById == SessionHelper.LoginId)
                        select item).ToList();
            }

            return list.OrderByDescending(n => n.WorkflowTriggerRequest.CreatedDate).ToList();
        }


        public static bool CanApproveChangeRequest(WorkflowTriggerRequestDetail requestDetail)
        {
            var canApproveChangeRequest = false;
            TimeAideContext db = new TimeAideContext();
            if (requestDetail.WorkflowActionTypeId == 1)
            {
                if (requestDetail.WorkflowLevel.WorkflowLevelTypeId == 1)
                {
                    var SupervisorInfoList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(requestDetail.WorkflowTriggerRequest.ChangeRequest.UserInformationId, SessionHelper.SelectedClientId);

                    if (SupervisorInfoList.Any(u => u.LoginEmail == SessionHelper.LoginEmail && u.Id != requestDetail.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        canApproveChangeRequest = true;
                    }
                }

                if (requestDetail.WorkflowLevel.WorkflowLevelGroup.Count > 0)
                {
                    var GruserInfoList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(requestDetail.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                    if (GruserInfoList.Any(u => u.LoginEmail == SessionHelper.LoginEmail && u.Id != requestDetail.WorkflowTriggerRequest.ChangeRequest.UserInformationId))
                    {
                        canApproveChangeRequest = true;
                    }
                }
            }
            return canApproveChangeRequest;

        }



        public static List<ChangeRequestEmployeeDependent> GetChangeRequestEmployeeDependent(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.ChangeRequestEmployeeDependent.Where(element => element.UserInformationId == userInformationId).ToList();
        }
        public static List<ChangeRequestAddress> GetChangeRequestAddress(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.ChangeRequestAddress.Where(element => element.UserInformationId == userInformationId).ToList();
        }
        public static List<ChangeRequestEmailNumbers> GetChangeRequestEmailNumbers(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.ChangeRequestEmailNumbers.Where(element => element.UserInformationId == userInformationId).ToList();
        }
        public static List<ChangeRequestEmergencyContact> GetChangeRequestEmergencyContact(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.ChangeRequestEmergencyContact.Where(element => element.UserInformationId == userInformationId).ToList();
        }

        public static List<SelfServiceEmployeeDocument> GetSelfServiceEmployeeDocument(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.SelfServiceEmployeeDocument.Where(element => element.UserInformationId == userInformationId).ToList();
        }

        public static List<SelfServiceEmployeeCredential> GetSelfServiceEmployeeCredential(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.SelfServiceEmployeeCredential.Where(element => element.UserInformationId == userInformationId).ToList();
        }
        public static List<EmployeeTimeOffRequest> GetEmployeeTimeOffRequest(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.EmployeeTimeOffRequest.Where(element => element.UserInformationId == userInformationId).ToList();
        }
        public static UserInformation GetUserInformation(WorkflowTriggerRequestDetail workflowTriggerRequestDetail)
        {
            if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestAddress != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestAddress.UserInformation;
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmailNumbers != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmailNumbers.UserInformation;
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmergencyContact != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmergencyContact.UserInformation;
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeDocument != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeDocument.UserInformation;
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeCredential != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeCredential.UserInformation;
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.EmployeeTimeOffRequest != null)
                return workflowTriggerRequestDetail.WorkflowTriggerRequest.EmployeeTimeOffRequest.UserInformation;
            return new UserInformation();
        }

        public static int GetRefrenceId(WorkflowTriggerRequestDetail workflowTriggerRequestDetail)
        {
            return GetRefrenceId(workflowTriggerRequestDetail.WorkflowTriggerRequest);
        }
        public static int GetRefrenceId(WorkflowTriggerRequest workflowTriggerRequest)
        {
            if (workflowTriggerRequest.ChangeRequestAddressId.HasValue)
                return workflowTriggerRequest.ChangeRequestAddressId.Value;
            else if (workflowTriggerRequest.ChangeRequestEmailNumbersId.HasValue)
                return workflowTriggerRequest.ChangeRequestEmailNumbersId.Value;
            else if (workflowTriggerRequest.ChangeRequestEmergencyContactId.HasValue)
                return workflowTriggerRequest.ChangeRequestEmergencyContactId.Value;
            else if (workflowTriggerRequest.SelfServiceEmployeeDocumentId.HasValue)
                return workflowTriggerRequest.SelfServiceEmployeeDocumentId.Value;
            else if (workflowTriggerRequest.ChangeRequestEmployeeDependentId.HasValue)
                return workflowTriggerRequest.ChangeRequestEmployeeDependentId.Value;

            else if (workflowTriggerRequest.SelfServiceEmployeeCredentialId.HasValue)
                return workflowTriggerRequest.SelfServiceEmployeeCredentialId.Value;
            else if (workflowTriggerRequest.EmployeeTimeOffRequestId.HasValue)
                return workflowTriggerRequest.EmployeeTimeOffRequestId.Value;
            return 0;
        }
        public static string GetChangeRequestType(WorkflowTriggerRequestDetail workflowTriggerRequestDetail)
        {
            if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestAddress != null)
                return "Address Change Request";
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmailNumbers != null)
                return "Emails and Numbers";
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmergencyContact != null)
                return "Emergency Contact";
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeDocument != null)
                return "Employee Document";
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeCredential != null)
                return "Employee Credential";
            if (workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmployeeDependent != null)
                return "Employee Dependent";
            //return "Emergency Contact Change Request";
            else if (workflowTriggerRequestDetail.WorkflowTriggerRequest.EmployeeTimeOffRequest != null)
                return "Time-Off Request";
            return "";
        }
        public static WorkflowTriggerRequestDetail StratWorkflow(TimeAideContext db, int workflowTriggerTypeId)
        {
            var workflowTRList = db.GetAllByCompany<WorkflowTrigger>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
            WorkflowTrigger workflowTrigger = workflowTRList.FirstOrDefault(w => w.WorkflowTriggerTypeId == workflowTriggerTypeId);
            WorkflowTriggerRequest workflowTriggerRequest = new WorkflowTriggerRequest();
            workflowTriggerRequest.WorkflowTrigger = workflowTrigger;

            WorkflowTriggerRequestDetail workflowTriggerRequestDetail = new WorkflowTriggerRequestDetail();
            workflowTriggerRequestDetail.WorkflowActionTypeId = 1;
            workflowTriggerRequestDetail.WorkflowLevel = workflowTrigger.Workflow.WorkflowLevel.OrderBy(i => i.Id).FirstOrDefault();
            workflowTriggerRequest.WorkflowTriggerRequestDetail.Add(workflowTriggerRequestDetail);
            db.WorkflowTriggerRequest.Add(workflowTriggerRequest);

            return workflowTriggerRequestDetail;
        }
        public static WorkflowTriggerRequestDetail GetNextWorkflowLevel<T>(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest) where T : ChangeRequestBase
        {
            // var abc = db.GetAll<WorkflowTrigger>(SessionHelper.SelectedClientId).ToList();

            WorkflowTrigger workflowTrigger = workflowTriggerRequest.WorkflowTrigger;
            WorkflowTriggerRequestDetail previousStep = workflowTriggerRequest.WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
            previousStep.ActionRemarks = changeRequest.ChangeRequestRemarks;

            previousStep.WorkflowActionTypeId = changeRequest.ChangeRequestStatusId;
            previousStep.ActionById = SessionHelper.LoginId;
            previousStep.ActionDate = DateTime.Now;
            WorkflowLevel previousStepLevel = previousStep.WorkflowLevel;
            Workflow workflow = db.Workflow.FirstOrDefault(w => w.Id == previousStepLevel.WorkflowId);
            WorkflowLevel nextStepLevel = workflow.WorkflowLevel.Where(w => w.Id > previousStepLevel.Id && w.DataEntryStatus == 1).OrderBy(w => w.Id).FirstOrDefault();

            //Initiate next level
            if (nextStepLevel != null && changeRequest.ChangeRequestStatusId == 2)
            {
                WorkflowTriggerRequestDetail workflowTriggerRequestDetail = new WorkflowTriggerRequestDetail();
                workflowTriggerRequestDetail.WorkflowActionTypeId = 1;
                changeRequest.ChangeRequestStatusId = 1;
                workflowTriggerRequestDetail.WorkflowLevel = nextStepLevel;
                workflowTriggerRequestDetail.WorkflowTriggerRequestId = workflowTriggerRequest.Id;
                workflowTriggerRequest.WorkflowTriggerRequestDetail.Add(workflowTriggerRequestDetail);
                db.WorkflowTriggerRequestDetail.Add(workflowTriggerRequestDetail);

                db.SaveChanges();
                //Send next level notification  
                UtilityHelper.SendEmailByWorkflow(changeRequest.UserInformationId, workflowTriggerRequestDetail, changeRequest.Id, db);
                return workflowTriggerRequestDetail;
            }
            else //workflow closing level processing
            {
                if (previousStep.WorkflowActionTypeId == 2)
                {
                    if (typeof(ChangeRequestAddress) == typeof(T))
                        ApplyChanges(db, changeRequest as ChangeRequestAddress);
                    else if (typeof(ChangeRequestEmailNumbers) == typeof(T))
                        ApplyChanges(db, changeRequest as ChangeRequestEmailNumbers);
                    else if (typeof(ChangeRequestEmergencyContact) == typeof(T))
                        ApplyChanges(db, changeRequest as ChangeRequestEmergencyContact);
                    else if (typeof(ChangeRequestEmployeeDependent) == typeof(T))
                        ApplyChanges(db, changeRequest as ChangeRequestEmployeeDependent);
                    else if (typeof(SelfServiceEmployeeDocument) == typeof(T))
                        ApplyChanges(db, changeRequest as SelfServiceEmployeeDocument);
                    else if (typeof(SelfServiceEmployeeCredential) == typeof(T))
                        ApplyChanges(db, changeRequest as SelfServiceEmployeeCredential);
                    else if (typeof(EmployeeTimeOffRequest) == typeof(T))
                        ApplyChanges(db, changeRequest as EmployeeTimeOffRequest);
                }
                db.SaveChanges();
                //send closing notification
                return ProcesstWorkflowClosingNotification(db, workflowTriggerRequest, changeRequest);
            }
        }
        public static string ProcessRequestCancellation<T>(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest) where T : ChangeRequestBase
        {
            try
            {
                WorkflowTrigger workflowTrigger = workflowTriggerRequest.WorkflowTrigger;
                WorkflowTriggerRequestDetail currentStep = workflowTriggerRequest.WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
                WorkflowLevel currentStepLevel = currentStep.WorkflowLevel;
                Workflow workflow = db.Workflow.FirstOrDefault(w => w.Id == currentStepLevel.WorkflowId);
                // WorkflowLevel nextStepLevel = workflow.WorkflowLevel.Where(w => w.Id > currentStepLevel.Id).OrderBy(w => w.Id).FirstOrDefault();

                changeRequest.ChangeRequestRemarks = changeRequest.ChangeRequestRemarks;
                changeRequest.ChangeRequestStatusId = changeRequest.ChangeRequestStatusId;
                currentStep.ActionById = changeRequest.UserInformationId;
                currentStep.ActionRemarks = changeRequest.ChangeRequestRemarks;
                currentStep.WorkflowActionTypeId = changeRequest.ChangeRequestStatusId;
                currentStep.ActionDate = DateTime.Now;
                db.SaveChanges();
                //process and send cancellation notification
                ProcesstWorkflowAutoCancelNotification(db, workflow, workflowTriggerRequest, changeRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Success";
        }

        public static string ProcessRequestAutoCancellation<T>(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest, string processType) where T : ChangeRequestBase
        {
            try
            {

                SessionHelper.IsExternalSession = true;
                SessionHelper.ExternalSessionClientId = changeRequest.UserInformation.ClientId ?? 0;
                SessionHelper.ExternalSessionCompanyId = changeRequest.UserInformation.CompanyId??0;
                SessionHelper.ExternalSessionLoginId = 1;

                WorkflowTrigger workflowTrigger = workflowTriggerRequest.WorkflowTrigger;
                WorkflowTriggerRequestDetail currentStep = workflowTriggerRequest.WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
                WorkflowLevel currentStepLevel = currentStep.WorkflowLevel;
                Workflow workflow = db.Workflow.FirstOrDefault(w => w.Id == currentStepLevel.WorkflowId);
                // WorkflowLevel nextStepLevel = workflow.WorkflowLevel.Where(w => w.Id > currentStepLevel.Id).OrderBy(w => w.Id).FirstOrDefault();
                if (processType == "Auto-Cancel")
                {
                    changeRequest.ChangeRequestRemarks = "Auto Cancel";
                    changeRequest.ChangeRequestStatusId = 5;
                    currentStep.ActionRemarks = changeRequest.ChangeRequestRemarks;
                    currentStep.WorkflowActionTypeId = changeRequest.ChangeRequestStatusId;
                    currentStep.ActionDate = DateTime.Now;
                    db.SaveChanges();
                    //process and send cancellation notification
                    ProcesstWorkflowAutoCancelNotification(db, workflow, workflowTriggerRequest, changeRequest);
                }
                else if (processType == "Reminder")
                {
                    UtilityHelper.SendEmailByWorkflowReminderNotification(changeRequest.UserInformationId, workflow, currentStep, changeRequest.Id, db);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Success";
        }

        public static SelfServiceEmployeeDocument ApplyChanges(TimeAideContext db, SelfServiceEmployeeDocument changeRequest)
        {
            var employeeDocument = db.Find<EmployeeDocument>(changeRequest.EmployeeDocumentId ?? 0, SessionHelper.SelectedClientId);
            if (employeeDocument == null)
            {
                employeeDocument = db.GetAll<EmployeeDocument>().Where(w => w.DocumentId == changeRequest.DocumentId && 
                                                                    w.DataEntryStatus == 1 && w.UserInformationId == changeRequest.UserInformationId)
                                                                    .FirstOrDefault(); ;
                if (employeeDocument == null)
                {
                    employeeDocument = new EmployeeDocument();
                    employeeDocument.DocumentId = changeRequest.DocumentId;
                    employeeDocument.Document = db.Document.Find(changeRequest.DocumentId);
                    employeeDocument.UserInformationId = changeRequest.UserInformationId;
                    db.EmployeeDocument.Add(employeeDocument);
                }
            }
            employeeDocument.ExpirationDate = changeRequest.ExpirationDate;
            employeeDocument.DocumentNote = changeRequest.DocumentNote;
            employeeDocument.SubmissionDate = DateTime.Now;

            FilePathHelper filePathHelper = new FilePathHelper();
            string docName = changeRequest.OriginalDocumentName;
            string serverFilePathTarget = filePathHelper.GetPath("EmployeeDocuments", ref docName, employeeDocument.UserInformationId ?? 0, employeeDocument.Id);

            if (!string.IsNullOrEmpty(changeRequest.DocumentPath))
                System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + changeRequest.DocumentPath, serverFilePathTarget);
            employeeDocument.DocumentPath = serverFilePathTarget.Replace(AppDomain.CurrentDomain.BaseDirectory, "");
            employeeDocument.DocumentName = docName;

            string approvers = "";
            bool isZeroLevel = false;
            foreach (var each in changeRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail)
            {
                if (each.WorkflowLevel.Workflow.IsZeroLevel)
                {
                    isZeroLevel = true;
                    break;
                }
                var actionBy = db.UserInformation.Find(each.ActionById);
                approvers += actionBy.FirstLastName + ",";
            }
            if (isZeroLevel)
                employeeDocument.SefServiceRemarks = "Modified by self-service user through zero level workflow " + approvers;
            else
                employeeDocument.SefServiceRemarks = "Requested by user and approve by: " + approvers;

            db.SaveChanges();
            return changeRequest;
        }
        public static SelfServiceEmployeeCredential ApplyChanges(TimeAideContext db, SelfServiceEmployeeCredential changeRequest)
        {
            var employeeCredential = db.Find<EmployeeCredential>(changeRequest.EmployeeCredentialId ?? 0, SessionHelper.SelectedClientId);
            if (employeeCredential == null)
            {
                employeeCredential = db.GetAll<EmployeeCredential>().FirstOrDefault(d => d.UserInformationId== changeRequest.UserInformationId && d.CredentialId == changeRequest.CredentialId && d.DataEntryStatus == 1);
                if (employeeCredential == null)
                {
                    employeeCredential = new EmployeeCredential();
                    employeeCredential.CredentialId = changeRequest.CredentialId;
                    employeeCredential.Credential = db.Credential.Find(changeRequest.CredentialId);
                    employeeCredential.UserInformationId = changeRequest.UserInformationId;
                    db.EmployeeCredential.Add(employeeCredential);
                }
            }
            employeeCredential.ExpirationDate = changeRequest.ExpirationDate;
            employeeCredential.EmployeeCredentialName = changeRequest.EmployeeCredentialName;
            employeeCredential.EmployeeCredentialDescription = changeRequest.EmployeeCredentialDescription;
            employeeCredential.IssueDate = changeRequest.IssueDate;
            employeeCredential.Note = changeRequest.Note;

            FilePathHelper filePathHelper = new FilePathHelper();
            string docName = changeRequest.OriginalDocumentName;
            string serverFilePathTarget = filePathHelper.GetPath("EmployeeCredentials", ref docName, employeeCredential.UserInformationId ?? 0, employeeCredential.Id);

            if (!string.IsNullOrEmpty(changeRequest.DocumentPath))
                System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + changeRequest.DocumentPath, serverFilePathTarget);

            employeeCredential.DocumentPath = filePathHelper.RelativePath;
            employeeCredential.DocumentName = docName;


            string approvers = "";
            foreach (var each in changeRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail)
            {
                if (each.ActionBy == null)
                {
                    var emp = db.UserInformation.FirstOrDefault(u => u.Id == each.ActionById);
                    approvers += emp.FullName + ",";
                }
                else
                {
                    approvers += each.ActionBy.FullName + ",";
                }
            }
            approvers = approvers.Trim(',');
            employeeCredential.SefServiceRemarks = "Requested by user and approve by: " + approvers;
            db.SaveChanges();

            return changeRequest;
        }
        public static ChangeRequestAddress ApplyChanges(TimeAideContext db, ChangeRequestAddress changeRequest)
        {
            var userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.Id == changeRequest.UserContactInformationId);
            userContactInformation.AddressType = changeRequest.AddressType;
            if (changeRequest.NewAddress1 != changeRequest.Address1)
                userContactInformation.Address1 = changeRequest.NewAddress1;
            if (changeRequest.NewAddress2 != changeRequest.Address2)
                userContactInformation.Address2 = changeRequest.NewAddress2;
            if (changeRequest.NewCityId != changeRequest.CityId)
                userContactInformation.CityId = changeRequest.NewCityId;
            if (changeRequest.NewCountryId != changeRequest.CountryId)
                userContactInformation.CountryId = changeRequest.NewCountryId;
            if (changeRequest.NewStateId != changeRequest.StateId)
                userContactInformation.StateId = changeRequest.NewStateId;
            if (changeRequest.NewZipCode != changeRequest.ZipCode)
                userContactInformation.ZipCode = changeRequest.NewZipCode;

            userContactInformation.SetUpdated<UserContactInformation>();
            return changeRequest;
        }
        public static ChangeRequestEmailNumbers ApplyChanges(TimeAideContext db, ChangeRequestEmailNumbers changeRequest)
        {
            var userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.Id == changeRequest.UserContactInformationId);
            if (changeRequest.NewHomeNumber != changeRequest.HomeNumber)
                userContactInformation.HomeNumber = changeRequest.NewHomeNumber;
            if (changeRequest.NewCelNumber != changeRequest.CelNumber)
                userContactInformation.CelNumber = changeRequest.NewCelNumber;
            if (changeRequest.NewWorkNumber != changeRequest.WorkNumber)
                userContactInformation.WorkNumber = changeRequest.NewWorkNumber;
            if (changeRequest.NewWorkExtension != changeRequest.WorkExtension)
                userContactInformation.WorkExtension = changeRequest.NewWorkExtension;
            if (changeRequest.NewFaxNumber != changeRequest.FaxNumber)
                userContactInformation.FaxNumber = changeRequest.NewFaxNumber;
            if (changeRequest.NewOtherNumber != changeRequest.OtherNumber)
                userContactInformation.OtherNumber = changeRequest.NewOtherNumber;
            if (changeRequest.NewWorkEmail != changeRequest.WorkEmail)
                userContactInformation.WorkEmail = changeRequest.NewWorkEmail;
            if (changeRequest.NewPersonalEmail != changeRequest.PersonalEmail)
                userContactInformation.PersonalEmail = changeRequest.NewPersonalEmail;
            if (changeRequest.NewOtherEmail != changeRequest.OtherEmail)
                userContactInformation.OtherEmail = changeRequest.NewOtherEmail;
            userContactInformation.SetUpdated<UserContactInformation>();
            return changeRequest;
        }
        public static ChangeRequestEmergencyContact ApplyChanges(TimeAideContext db, ChangeRequestEmergencyContact changeRequest)
        {
            EmergencyContact emergencyContact = db.EmergencyContact.FirstOrDefault(u => u.Id == changeRequest.EmergencyContactId);
            if (string.IsNullOrEmpty(changeRequest.ReasonForDelete))
            {
                if (changeRequest.EmergencyContactId == null || changeRequest.EmergencyContactId < 1)
                {
                    emergencyContact = new EmergencyContact();
                    emergencyContact.UserInformationId = changeRequest.UserInformationId;
                }
                if (changeRequest.NewAlternateNumber != changeRequest.AlternateNumber)
                    emergencyContact.AlternateNumber = changeRequest.NewAlternateNumber;
                if (changeRequest.NewContactPersonName != changeRequest.ContactPersonName)
                    emergencyContact.ContactPersonName = changeRequest.NewContactPersonName;
                if (changeRequest.NewIsDefault != changeRequest.IsDefault)
                    emergencyContact.IsDefault = changeRequest.NewIsDefault;
                if (changeRequest.NewMainNumber != changeRequest.MainNumber)
                    emergencyContact.MainNumber = changeRequest.NewMainNumber;
                if (changeRequest.NewRelationshipId != changeRequest.RelationshipId)
                    emergencyContact.RelationshipId = changeRequest.NewRelationshipId;
            }
            else
            {
                emergencyContact.DataEntryStatus = 2;
            }

            if (changeRequest.EmergencyContactId == null || changeRequest.EmergencyContactId < 1)
            {
                db.EmergencyContact.Add(emergencyContact);
            }
            else
            {
                emergencyContact.SetUpdated<EmergencyContact>();
                if (!string.IsNullOrEmpty(changeRequest.ReasonForDelete))
                {
                    emergencyContact.DataEntryStatus = 2;
                }
                db.Entry(emergencyContact).State = EntityState.Modified;
            }




            if (changeRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTrigger.Workflow.IsZeroLevel)
            {
                emergencyContact.SefServiceRemarks = "Modified by self-service user through zero level workflow.";
            }
            else
            {
                string approvers = "";
                foreach (var each in changeRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail)
                {
                    approvers += each.ActionBy.FirstLastName + ",";
                }
                emergencyContact.SefServiceRemarks = "Requested by user and approve by: " + approvers;
            }
            return changeRequest;
        }
        public static ChangeRequestEmployeeDependent ApplyChanges(TimeAideContext db, ChangeRequestEmployeeDependent changeRequest)
        {
            var employeeDependent = db.EmployeeDependent.FirstOrDefault(u => u.Id == changeRequest.EmployeeDependentId);
            if (string.IsNullOrEmpty(changeRequest.ReasonForDelete))
            {
                if (changeRequest.EmployeeDependentId == null || changeRequest.EmployeeDependentId < 1)
                {
                    employeeDependent = new EmployeeDependent();
                    employeeDependent.UserInformationId = changeRequest.UserInformationId;
                }
                if (changeRequest.NewFirstName != changeRequest.FirstName)
                {
                    employeeDependent.FirstName = changeRequest.NewFirstName;
                }
                if (changeRequest.NewBirthDate != changeRequest.BirthDate)
                {
                    employeeDependent.BirthDate = changeRequest.NewBirthDate;
                }
                if (changeRequest.NewDependentStatusId != changeRequest.DependentStatusId)
                {
                    employeeDependent.DependentStatusId = changeRequest.NewDependentStatusId;
                }
                if (changeRequest.NewExpiryDate != changeRequest.ExpiryDate)
                {
                    employeeDependent.ExpiryDate = changeRequest.NewExpiryDate;
                }
                if (changeRequest.NewGenderId != changeRequest.GenderId)
                {
                    employeeDependent.GenderId = changeRequest.NewGenderId;
                }
                if (changeRequest.NewIsDentalInsurance != changeRequest.IsDentalInsurance)
                {
                    employeeDependent.IsDentalInsurance = changeRequest.NewIsDentalInsurance ?? false;
                }
                if (changeRequest.NewIsFullTimeStudent != changeRequest.IsFullTimeStudent)
                {
                    employeeDependent.IsFullTimeStudent = changeRequest.NewIsFullTimeStudent ?? false;
                }
                if (changeRequest.NewIsHealthInsurance.HasValue != changeRequest.IsHealthInsurance)
                {
                    employeeDependent.IsHealthInsurance = changeRequest.NewIsHealthInsurance ?? false;
                }
                if (changeRequest.NewIsTaxPurposes != changeRequest.IsTaxPurposes)
                {
                    employeeDependent.IsTaxPurposes = changeRequest.NewIsTaxPurposes ?? false;
                }
                if (changeRequest.NewLastName != changeRequest.LastName)
                {
                    employeeDependent.LastName = changeRequest.NewLastName;
                }
                if (changeRequest.NewRelationshipId != changeRequest.RelationshipId)
                {
                    employeeDependent.RelationshipId = changeRequest.NewRelationshipId;
                }
                if (changeRequest.NewSchoolAttending != changeRequest.SchoolAttending)
                {
                    employeeDependent.SchoolAttending = changeRequest.NewSchoolAttending;
                }
                if (changeRequest.NewSSN != changeRequest.SSN)
                {
                    employeeDependent.SSN = changeRequest.NewSSN;
                }
            }
            else
            {
                employeeDependent.DataEntryStatus = 2;
            }
            if (changeRequest.EmployeeDependentId == null || changeRequest.EmployeeDependentId < 1)
            {
                db.EmployeeDependent.Add(employeeDependent);
            }
            else
            {
                employeeDependent.SetUpdated<EmployeeDependent>();
                if (!string.IsNullOrEmpty(changeRequest.ReasonForDelete))
                {
                    employeeDependent.DataEntryStatus = 2;
                }
                db.Entry(employeeDependent).State = EntityState.Modified;
            }
            return changeRequest;
        }



        public static string GetNewChangeRequestApprovalUrl(BellIconNotificationViewModel bellIconNotification, bool isForApproval, bool isFromExternalLink)
        {


            if (isForApproval)
            {
                bellIconNotification.ActionName = "Approve" + bellIconNotification.ActionName;
                if(bellIconNotification.ControllerName!= "/EmployeeTimeOffRequest")
                {
                    bellIconNotification.ControllerName = "Approve" + bellIconNotification.ControllerName;
                }
                
            }
            if (isForApproval!=true)
            {
                bellIconNotification.ActionName = "View" + bellIconNotification.ActionName;
            }
           
            if (isFromExternalLink)
            {
                string url = "Home/RequestApproval?param1=" + bellIconNotification.ControllerName + "&param2=" + bellIconNotification.ActionName + "&Id=" + bellIconNotification.ReferenceId;
                return url;
            }
            else
            {
                string url = bellIconNotification.ControllerName + "/" + bellIconNotification.ActionName + "?Id=" + bellIconNotification.ReferenceId;
                return url;
            }
        }



      
        public static string GetChangeRequestApprovalUrl(WorkflowTriggerRequestDetail workflowTriggerRequestDetail, bool isForApproval, bool isFromExternalLink)
        {
            string controllerName = GetChangeRequestApprovalActionControllerName(workflowTriggerRequestDetail, isForApproval); //= "ChangeRequestAddress";
            if (workflowTriggerRequestDetail.WorkflowTriggerRequest.EmployeeTimeOffRequest != null)
            {
                controllerName = "/EmployeeTimeOffRequest";
            }
            string actionMethod = GetChangeRequestApprovalActionMethod(workflowTriggerRequestDetail, isForApproval);
            if (isFromExternalLink)
            {
                string url = "Home/RequestApproval?param1=" + controllerName + "&param2=" + actionMethod + "&Id=" + WorkflowService.GetRefrenceId(workflowTriggerRequestDetail);
                return url;
            }
            else
            {
                string url = controllerName + "/" + actionMethod + "?Id=" + WorkflowService.GetRefrenceId(workflowTriggerRequestDetail);
                return url;
            }
        }
        public static string GetChangeRequestApprovalActionControllerName(WorkflowTriggerRequestDetail workflowTriggerRequestDetail, bool isForApproval)
        {
            return GetChangeRequestApprovalActionControllerName(workflowTriggerRequestDetail.WorkflowTriggerRequest, isForApproval);
        }
        public static string GetChangeRequestApprovalActionControllerName(WorkflowTriggerRequest workflowTriggerRequest, bool isForApproval)
        {
            string controllerType, controllerName = "";
            if (isForApproval)
                controllerType = "Approve";
            else
                controllerType = "";

            if (workflowTriggerRequest.ChangeRequestAddress != null)
                controllerName = "ChangeRequestAddress";
            else if (workflowTriggerRequest.ChangeRequestEmailNumbers != null)
                controllerName = "ChangeRequestEmailsAndNumber";
            else if (workflowTriggerRequest.ChangeRequestEmergencyContact != null)
                controllerName = "ChangeRequestEmergencyContact";
            else if (workflowTriggerRequest.SelfServiceEmployeeDocument != null)
                controllerName = "ChangeRequestEmployeeDocument";
            else if (workflowTriggerRequest.SelfServiceEmployeeCredential != null)
                controllerName = "ChangeRequestEmployeeCredential";
            else if (workflowTriggerRequest.ChangeRequestEmployeeDependent != null)
                controllerName = "ChangeRequestEmployeeDependent";
            else if (workflowTriggerRequest.EmployeeTimeOffRequest != null)
                return "ApproveTimeOffRequest";
            return controllerType + controllerName;
        }
        public static string GetChangeRequestApprovalActionMethod(WorkflowTriggerRequestDetail workflowTriggerRequestDetail, bool isForApproval)
        {
            return GetChangeRequestApprovalActionMethod(workflowTriggerRequestDetail.WorkflowTriggerRequest, isForApproval);
        }
        public static string GetChangeRequestApprovalActionMethod(WorkflowTriggerRequest workflowTriggerRequest, bool isForApproval)
        {
            string actionType, actionMethodName = "";
            if (isForApproval)
                actionType = "Approve";
            else
                actionType = "View";

            if (workflowTriggerRequest.ChangeRequestAddress != null)
                actionMethodName = "MailingChangeRequestAddress";
            else if (workflowTriggerRequest.ChangeRequestEmailNumbers != null)
                actionMethodName = "ChangeRequestEmailsAndNumbers";
            else if (workflowTriggerRequest.ChangeRequestEmergencyContact != null)
                actionMethodName = "ChangeRequestEmergencyContact";
            else if (workflowTriggerRequest.ChangeRequestEmployeeDependent != null)
                actionMethodName = "ChangeRequestEmployeeDependent";
            else if (workflowTriggerRequest.SelfServiceEmployeeDocument != null)
                actionMethodName = "SelfServiceEmployeeDocument";
            else if (workflowTriggerRequest.SelfServiceEmployeeCredential != null)
                actionMethodName = "SelfServiceEmployeeCredential";
            else if (workflowTriggerRequest.EmployeeTimeOffRequest != null)
                actionMethodName = "TimeOffRequest";
            return actionType + actionMethodName;
        }
        public static string BuildChangeRequestViewFunction(WorkflowTriggerRequest workflowTriggerRequest)
        {
            if (workflowTriggerRequest == null)
                return "";
            //ShowRequestDetail(@item.WorkflowTriggerRequest.FirstOrDefault().ChangeRequestAddressId, 'ApproveMailingChangeRequestAddress', 'ApproveChangeRequestAddress');
            return "ShowRequestDetail(" + GetRefrenceId(workflowTriggerRequest) + ",'" + GetChangeRequestApprovalActionMethod(workflowTriggerRequest, false) + "','" + GetChangeRequestApprovalActionControllerName(workflowTriggerRequest, false) + "');";
        }
        public static List<string> CanWorkflowIntiated(int userInformationId, int workflowTriggerTypeId)
        {
            List<string> validationMessages = new List<string>();
            TimeAideContext db = new TimeAideContext();
            var abc = db.GetAllByCompany<WorkflowTrigger>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
            WorkflowTrigger workflowTrigger = abc.FirstOrDefault(w => w.WorkflowTriggerTypeId == workflowTriggerTypeId);
            if (abc.Count == 0 || workflowTrigger == null || (!workflowTrigger.Workflow.IsZeroLevel && workflowTrigger.Workflow.WorkflowLevel.Count == 0))
            {
                validationMessages.Add("Workflows are not properly setup, please contact admin");
                return validationMessages.ToList();
            }
            foreach (var each in workflowTrigger.Workflow.WorkflowLevel)
            {
                if (each.WorkflowLevelTypeId == 1)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(userInformationId, SessionHelper.SelectedClientId)
                                                                        .Where(w=>w.EmployeeStatusId==1).ToList<UserInformationViewModel>();// check active employee
                    
                    if (userInformationList.Count == 0)
                    {
                        validationMessages.Add("Employee is not associated with either any supervisor(s) or there is not active supervisor(s).");
                    }
                }
                if (each.WorkflowLevelGroup.Count > 0)
                {
                    var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.Id, SessionHelper.SelectedClientId)
                                                    .Where(w => w.EmployeeStatusId == 1).ToList<UserInformationViewModel>(); // check active employee
                    if (userInformationList.Count == 0)
                    {
                        validationMessages.Add("Any workflow Level group, should have at least one active user: " + each.SelectedWorkflowLevelGroupNames);
                    }
                }
            }

            return validationMessages.ToList();
        }
        public static WorkflowTriggerRequestDetail ProcesstWorkflowAutoCancelNotification<T>(TimeAideContext db, Workflow workflow, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest) where T : ChangeRequestBase
        {
            Dictionary<string, string> toEmails = new Dictionary<string, string>();
            GetClosingNotificationEmails(db, workflowTriggerRequest, changeRequest, toEmails);
            var workflowTriggerRequestDetail = ClosingNotificationLevel(db, workflowTriggerRequest, toEmails);
            UtilityHelper.SendEmailByWorkflowAutoCancelNotification(changeRequest.UserInformationId, changeRequest.Id, workflow, workflowTriggerRequest.WorkflowTriggerRequestDetail.FirstOrDefault(), toEmails, db);
            return workflowTriggerRequestDetail;
        }

        public static WorkflowTriggerRequestDetail ProcesstWorkflowClosingNotification<T>(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest) where T : ChangeRequestBase

        {
            Dictionary<string, string> toEmails = new Dictionary<string, string>();
            GetClosingNotificationEmails(db, workflowTriggerRequest, changeRequest, toEmails);
            var workflowTriggerRequestDetail = ClosingNotificationLevel(db, workflowTriggerRequest, toEmails);
            UtilityHelper.SendEmailByWorkflowClosingNotification(changeRequest.UserInformationId, changeRequest.Id, workflowTriggerRequest.WorkflowTrigger.Workflow, workflowTriggerRequest.WorkflowTriggerRequestDetail.FirstOrDefault(), toEmails, db);
            return workflowTriggerRequestDetail;
        }
        private static WorkflowTriggerRequestDetail ClosingNotificationLevel(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, Dictionary<string, string> toEmails)
        {
            WorkflowTriggerRequestDetail workflowTriggerRequestDetail = new WorkflowTriggerRequestDetail();
            workflowTriggerRequestDetail.WorkflowActionTypeId = 6;
            workflowTriggerRequestDetail.ActionRemarks = "Closing of request.";
            workflowTriggerRequestDetail.WorkflowLevel = workflowTriggerRequest.WorkflowTrigger.Workflow.WorkflowLevel.FirstOrDefault();
            workflowTriggerRequestDetail.WorkflowTriggerRequestId = workflowTriggerRequest.Id;
            workflowTriggerRequest.WorkflowTriggerRequestDetail.Add(workflowTriggerRequestDetail);
            db.WorkflowTriggerRequestDetail.Add(workflowTriggerRequestDetail);
            WorkflowTriggerRequestDetailEmail logEmail = new WorkflowTriggerRequestDetailEmail();
            logEmail.SenderAddress = ConfigurationManager.AppSettings["FromMail"].ToString();
            logEmail.ToAddress = string.Format("({0})", string.Join(",", toEmails.Keys)); ;
            logEmail.CcAddress = "";
            logEmail.BccAddress = "";
            logEmail.WorkflowTriggerRequestDetail = workflowTriggerRequestDetail;
            db.WorkflowTriggerRequestDetailEmail.Add(logEmail);
            db.SaveChanges();
            return workflowTriggerRequestDetail;
        }

        private static void GetClosingNotificationEmails<T>(TimeAideContext db, WorkflowTriggerRequest workflowTriggerRequest, T changeRequest, Dictionary<string, string> toEmails) where T : ChangeRequestBase
        {
            var workflow = workflowTriggerRequest.WorkflowTrigger.Workflow;
            var userInformation = db.UserContactInformation
                                    .Where(w=>w.UserInformation.EmployeeStatusId==1)//Active employee
                                    .FirstOrDefault(u => u.UserInformationId == changeRequest.UserInformationId);
            if (!String.IsNullOrEmpty(userInformation.NotificationEmail))
                toEmails.Add(userInformation.NotificationEmail, userInformation.NotificationEmail);
            if (workflow.IsZeroLevel)
            {
                var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(changeRequest.UserInformationId, SessionHelper.SelectedClientId)
                                                            .Where(w => w.EmployeeStatusId == 1); //active Supervisors
                foreach (var each in userInformationList)
                {
                    var notifyEmail = each.NotificationEmail;
                    if (!String.IsNullOrEmpty(notifyEmail) && !toEmails.Keys.Contains(notifyEmail))
                    {
                        toEmails.Add(notifyEmail, notifyEmail);
                    }
                }
            }
            else
            {
                if (workflow.ClosingNotificationId == (int)ClosingNotificationTypes.EmployeeAndApprovers)
                {
                    foreach (var each in workflowTriggerRequest.WorkflowTriggerRequestDetail)
                    {
                        if (each.ActionBy.EmployeeStatusId == 1) //Check action taken by should be active at that time
                        {
                            var notifyEmail = each.ActionBy.ActiveUserContactInformation.NotificationEmail;
                            if (!String.IsNullOrEmpty(notifyEmail) && !toEmails.Keys.Contains(notifyEmail))
                            {
                                toEmails.Add(notifyEmail, notifyEmail);
                            }
                        }
                    }
                }
                else if (workflow.ClosingNotificationId == (int)ClosingNotificationTypes.All)
                {
                    foreach (var each in workflowTriggerRequest.WorkflowTriggerRequestDetail)
                    {
                        if (each.WorkflowLevel.WorkflowLevelTypeId == 1)
                        {
                            var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(changeRequest.UserInformationId, SessionHelper.SelectedClientId)
                                                                            .Where(w => w.EmployeeStatusId == 1); //active supervisor(s)
                            foreach (var eachDF in userInformationList)
                            {
                                if (!String.IsNullOrEmpty(eachDF.NotificationEmail) && !toEmails.Keys.Contains(eachDF.NotificationEmail))
                                {
                                    toEmails.Add(eachDF.NotificationEmail, eachDF.NotificationEmail);
                                }
                            }
                        }
                        if (each.WorkflowLevel.WorkflowLevelGroup.Count > 0)
                        {
                            var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(each.WorkflowLevel.Id, SessionHelper.SelectedClientId)
                                                                        .Where(w => w.EmployeeStatusId == 1); //active employee group user
                            foreach (var eachDF in userInformationList)
                            {
                                if (!String.IsNullOrEmpty(eachDF.NotificationEmail) && !toEmails.Keys.Contains(eachDF.NotificationEmail))
                                {
                                    toEmails.Add(eachDF.NotificationEmail, eachDF.NotificationEmail);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ApplyChanges(TimeAideContext db, EmployeeTimeOffRequest timeOffRequestWeb)
        {
            //Apply Time-Off Request detail
            if (TimeAideWindowContext != null)
            {
                try
                {
                    tblSS_TimeOffRequest timeAideWinTOREntity = new tblSS_TimeOffRequest();
                    timeAideWinTOREntity.intUserID = timeOffRequestWeb.UserInformation.EmployeeId;
                    timeAideWinTOREntity.strTransType = timeOffRequestWeb.TransType;
                    timeAideWinTOREntity.dtStartDate = timeOffRequestWeb.StartDate;
                    timeAideWinTOREntity.dtEndDate = timeOffRequestWeb.EndDate;
                    timeAideWinTOREntity.dtDayStart = timeOffRequestWeb.StartDate;
                    timeAideWinTOREntity.bitSingleDay = (timeOffRequestWeb.StartDate == timeOffRequestWeb.EndDate);
                    timeAideWinTOREntity.decDayHours = timeOffRequestWeb.DayHours;
                    timeAideWinTOREntity.intTORStatusID = 0;
                    //timeAideWinTOREntity.strRequestNote = timeOffRequestWeb.RequestNote.Substring(0,500);
                    var mxNoteLen = timeOffRequestWeb.RequestNote.Length > 500 ? 500 : timeOffRequestWeb.RequestNote.Length;
                    timeAideWinTOREntity.strRequestNote = timeOffRequestWeb.RequestNote.Substring(0, mxNoteLen);

                    timeAideWinTOREntity.dtTimestamp = DateTime.Now;
                    timeAideWinTOREntity.strCompanyName = "";
                    TimeAideWindowContext.tblSS_TimeOffRequest.Add(timeAideWinTOREntity);

                    TimeAideWindowContext.SaveChanges();
                    if (timeAideWinTOREntity.intTORUniqueID > 0)
                    {
                        //To update the transaction table in timeaide window
                        TimeAideWindowContext.spSS_UpdateTimeOffRequestFromWeb(timeAideWinTOREntity.intTORUniqueID);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }


        }
        public static bool CanTakeAction(ChangeRequestBase changeRequest)
        {
            WorkflowTriggerRequestDetail previousStep = changeRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
            List<UserInformationViewModel> userInformationList = new List<UserInformationViewModel>();
            TimeAideContext db = new TimeAideContext();
            if (previousStep.WorkflowActionTypeId == 6)
            {
                return false;
            }
            if (previousStep.WorkflowLevel.WorkflowLevelTypeId == 1)
            {
                userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(changeRequest.UserInformationId, SessionHelper.SelectedClientId);
            }
            if (previousStep.WorkflowLevel.WorkflowLevelGroup.Count > 0)
            {
                userInformationList = userInformationList.Union(db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(previousStep.WorkflowLevel.Id, SessionHelper.SelectedClientId).ToList()).ToList();
            }
            return userInformationList.Any(u => !string.IsNullOrEmpty(u.NotificationEmail) && u.NotificationEmail.ToLower() == SessionHelper.NotificationEmail.ToLower());
        }
        public static bool IsClosingNotification(ChangeRequestBase item)
        {
            WorkflowTriggerRequestDetail previousStep = item.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
            if (previousStep != null && previousStep.WorkflowActionTypeId == 6)
            {
                TimeAideContext db = new TimeAideContext();
                int workflowTriggerRequestId = previousStep.WorkflowTriggerRequest.WorkflowTriggerRequestDetail.FirstOrDefault().WorkflowTriggerRequestId;
                if (!db.NotificationLogMessageReadBy.Any(n => n.WorkflowTriggerRequestId == workflowTriggerRequestId && n.ReadById == SessionHelper.LoginId))
                {
                    NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                    notificationLogMessageReadBy.WorkflowTriggerRequestId = workflowTriggerRequestId;
                    notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                    db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public static void GetNewListItem(WorkflowTriggerRequestDetail detail, StringBuilder li)
        {
            li.Append("<li class=\"notification-message\" id=\"li_");
            li.Append(detail.Id);
            li.Append("\">");
            if (detail.ActionById == 6)
                li.Append("<a href=\"javascript:ChangeRequestView(\"li_");
            else
                li.Append("<a href=\"javascript:ChangeRequestApproval(\"li_");
            li.Append(detail.Id);
            li.Append("\",\"");
            if (detail.WorkflowActionTypeId == 6)
                li.Append(TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(detail, false, false).Replace('/', '\\'));
            else
                li.Append(TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(detail, true, false).Replace('/', '\\'));
            li.Append("\")>");
            li.Append("<div class=\"media\">");
            if (detail.WorkflowActionTypeId == 6)
            {
                if (detail.PreviousWorkflowTriggerRequest.WorkflowActionTypeId == 2)
                    li.Append("<span class=\"avatar\" style=\"background: #428bca !important \">A</span>");
                else if (detail.PreviousWorkflowTriggerRequest.WorkflowActionTypeId == 3)
                    li.Append("<span class=\"avatar\" style=\"background: #428bca !important \">D</span>");
                else if (detail.PreviousWorkflowTriggerRequest.WorkflowActionTypeId == 4)
                    li.Append("<span class=\"avatar\" style=\"background: #428bca !important \">C</span>");
            }
            else
                li.Append("<span class=\"avatar\" style=\"background: #428bca !important \">P</span>");
            li.Append("<div class=\"media-body\">");
            li.Append("<p class=\"noti-details\"><span class=\"noti-title\">");
            li.Append(@TimeAide.Services.WorkflowService.GetUserInformation(detail).ShortFullName);
            li.Append("</span> Change Request for <span class=\"noti-title\">");
            li.Append(@TimeAide.Services.WorkflowService.GetChangeRequestType(detail));
            li.Append("</span></p>");
            li.Append("<p class=\"noti-time\"><span class=\"notification-time\">");
            li.Append(detail.WorkflowTriggerRequest.CreatedSince);
            li.Append("</span></p></div></div></a></li>");
        }
    }
}
