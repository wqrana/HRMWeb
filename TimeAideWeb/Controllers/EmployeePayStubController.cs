using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAide.Web.Extensions;
using TimeAide.Reports;

namespace TimeAide.Web.Controllers
{   
    public class EmployeePayStubController : BaseTAWindowRoleRightsController<EmployeeAttendenceSchedule>
    {
       
        public ActionResult Index(string batchId, int?id)
        {
            var userId = id ?? SessionHelper.LoginId;
            try
            {
                var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                                                 .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id, CompanyLogo = s.Company.CompanyLogo })
                                                 .FirstOrDefault();
                var payStubList = GetPayStubBatchList(userId);
                ViewBag.PayStubBatchId = new SelectList(payStubList, "BatchId", "BatchDescription", batchId);
                ViewBag.UserId = userId;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "PayStub", "Index");
                return PartialView("Error", handleErrorInfo);
            }
            return PartialView();
        }
        public ActionResult PayStubDetail(string batchId, int id)
        {
            var userId = id ;
            try
            {
                ReportDataHelper rptDataHelper = new ReportDataHelper();
                var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                                                 .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id, CompanyLogo = s.Company.CompanyLogo })
                                                 .FirstOrDefault();
                var payStubRecordData = rptDataHelper.getEmployeePayStubRptData(batchId, userInfo.EmployeeId ?? 0);
                if (payStubRecordData.EmployeePayStubCompany != null)
                {
                    payStubRecordData.EmployeePayStubCompany.CompanyLogo = userInfo.CompanyLogo;
                }

                ViewBag.PayStubCompanyInfo = payStubRecordData.EmployeePayStubCompany;
                ViewBag.PayStubCompensations = payStubRecordData.EmployeePayStubCompensations;
                ViewBag.payStubWithholdings = payStubRecordData.EmployeePayStubWithholdings;
                return PartialView(payStubRecordData.EmployeePayStubBatch);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "PayStub", "PayStubDetail");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public JsonResult AjaxPayStubBatchList(int? id)
        {
            string status = "Success";
            string message = "Successfully fetch!";
            List<dynamic> payStubWidgetData = null;  
            var userId = id ?? SessionHelper.LoginId;
          //DateTime fromDate;
          //DateTime toDate;
          //var appConfigPayStubBeforeDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "PayStubBeforeDays")
          //                                                                         .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

         
          //  int intPayStubBeforeDD;
          //  int.TryParse(appConfigPayStubBeforeDD, out intPayStubBeforeDD);
           
          //      intPayStubBeforeDD = intPayStubBeforeDD == 0 ? 90 : intPayStubBeforeDD;
          //      intPayStubBeforeDD *= -1;
          //      toDate = DateTime.Today;
          //      fromDate = toDate.AddDays(intPayStubBeforeDD);
            try
            {
                //var UserInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                //                                .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id })
                //                                .FirstOrDefault();

                //var payStubList = timeAideWindowContext.viewPay_UserBatchStatus.
                //                                Where(w => w.intUserID == UserInfo.EmployeeId && w.intCompanyID == UserInfo.OldCompanyId
                //                                      && w.dtPayDate >= fromDate && w.dtPayDate <= toDate).OrderByDescending(o => o.dtPayDate)
                //                                      .Select(s => new { BatchId = s.strBatchID.ToString(), BatchDescription = s.strBatchDescription })
                //                                      .ToList<dynamic>();
                // payStubWidgetData = payStubList;
                payStubWidgetData = GetPayStubBatchList(userId);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;

            }
                
            return Json(new { status = status, message = message, widgetData = payStubWidgetData });
        }
        public List<dynamic> GetPayStubBatchList(int id)
        {
            List<dynamic> payStubBatchList = null;
            var userId = id;
            DateTime fromDate;
            DateTime toDate;
            var appConfigPayStubBeforeDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "PayStubBeforeDays")
                                                                                     .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();


            int intPayStubBeforeDD;
            int.TryParse(appConfigPayStubBeforeDD, out intPayStubBeforeDD);

            intPayStubBeforeDD = intPayStubBeforeDD == 0 ? 90 : intPayStubBeforeDD;
            intPayStubBeforeDD *= -1;
            toDate = DateTime.Today;
            fromDate = toDate.AddDays(intPayStubBeforeDD);
            try
            {
                var UserInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                                                .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id })
                                                .FirstOrDefault();

                var payStubList = timeAideWindowContext.viewPay_UserBatchStatus.
                                                Where(w => w.intUserID == UserInfo.EmployeeId && w.intCompanyID == UserInfo.OldCompanyId
                                                      && w.dtPayDate >= fromDate && w.dtPayDate <= toDate && w.intBatchStatus==-1).OrderByDescending(o => o.dtPayDate)
                                                      .Select(s => new { BatchId = s.strBatchID.ToString(), BatchDescription = s.strBatchDescription })
                                                      .ToList<dynamic>();
                payStubBatchList = payStubList;
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return payStubBatchList;
        }
        public JsonResult AjaxDashboardPayStubWidgetData(string batchId,int? id)
        {
            string status = "Success";
            string message = "Successfully fetch!";
            dynamic payStubWidgetData = null;
            var userId = id ?? SessionHelper.LoginId;
            try
            {
                var UserInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                                               .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id })
                                               .FirstOrDefault();
                var payStubRecord = timeAideWindowContext.viewPay_UserBatchStatus.
                                                Where(w => w.strBatchID.ToString() == batchId && w.intUserID== UserInfo.EmployeeId)
                                                .ToList()
                                                      .Select(s => new { BatchId = s.strBatchID.ToString()
                                                                
                                                                        ,BatchDescription = s.strBatchDescription
                                                                        ,PayDate=s.dtPayDate.ToString("MM/dd/yyyy")
                                                                        ,PeriodStart=s.dtStartDatePeriod.ToString("MM/dd/yyyy")
                                                                        ,PeriodEnd = s.dtEndDatePeriod.ToString("MM/dd/yyyy")
                                                                        ,Earnings = s.decBatchUserCompensations
                                                                        ,Deducations = s.decBatchUserWithholdings
                                                                        ,NetPayAmount = s.decBatchNetPay
                                                      })
                                                      .FirstOrDefault<dynamic>();
                payStubWidgetData = payStubRecord;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;

            }

            return Json(new { status = status, message = message, widgetData = payStubWidgetData });
        }
        public ActionResult GetSelfServiceScheduleDetail(UserFilterViewModel schFilter)
        {
            IList<EmployeeAttendenceSchDetail> userAttendanceSchDetail = null;
            //  var userId = 248;//SessionHelper.LoginId;
            var userId = schFilter.EmployeeId == 0 ? SessionHelper.LoginId : schFilter.EmployeeId.Value;
            try
            {
              
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetSelfServiceScheduleDetail");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView("SelfServiceScheduleDetail", userAttendanceSchDetail);
        }
      

    
        private List<EmployeeAttendenceSchedule> getEmployeePayStubBatchList(int userId)
        {
            List<EmployeeAttendenceSchedule> retAttendenceScheduleList = null;

            try
            {
                //var userList = getFilterEmployeeList(schFilter);

                //var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                //    .Where(w => schFilter.ScheduleDate >= w.dStartDate && schFilter.ScheduleDate <= w.dEndDate)
                //    .Join(timeAideWindowContext.tusers,
                //     usch => usch.nUserID,
                //      ur => ur.id,
                //      (usch, ur) => new EmployeeAttendenceSchedule
                //      {
                //          Id = usch.ID,
                //          nUserID = usch.nUserID,
                //          CompanyId = ur.nCompanyID,
                //          sUserName = usch.sUserName,
                //          sWeekID = usch.sWeekID,
                //          sNote = usch.sNote,
                //          dStartDate = usch.dStartDate,
                //          dEndDate = usch.dEndDate,
                //          dblPeriodHours = usch.dblPeriodHours,
                //          nPayPeriodType = usch.nPayPeriodType,
                //          nPayWeekNum = usch.nPayWeekNum,
                //          nScheduleId = ur.nScheduleID
                //      }).ToList();



                //var usrSchFinal = userSchedule                                       //Outer Table
                //.Join(userList,                                      //Inner Table t join
                //     usch => new { userId = usch.nUserID, cmpId = usch.CompanyId },     //Condition from outer table
                //     urs => new { userId = urs.EmployeeId, cmpId = urs.OldCompanyId },          //Condition from inner table
                //     (usch, urs) => new EmployeeAttendenceSchedule
                //     {                                 //Result
                //         Id = usch.Id,
                //         nUserID = usch.nUserID,
                //         OldCompanyId = usch.CompanyId,
                //         EmployeeId = urs.EmployeeId,
                //         UserInformationId = urs.Id,
                //         CompanyId = urs.CompanyId,
                //         sUserName = usch.sUserName,
                //         sWeekID = usch.sWeekID,
                //         sNote = usch.sNote,
                //         dStartDate = usch.dStartDate,
                //         dEndDate = usch.dEndDate,
                //         dblPeriodHours = usch.dblPeriodHours,
                //         nPayPeriodType = usch.nPayPeriodType,
                //         nPayWeekNum = usch.nPayWeekNum
                //     }
                //     ).ToList();
                //retAttendenceScheduleList = usrSchFinal;

            }
            catch (Exception ex)
            {
                throw;
            }

            return retAttendenceScheduleList;

        }
      
        private IEnumerable<UserInformationViewModel> getFilterEmployeeList(UserFilterViewModel schFilter)
        {
            IEnumerable<UserInformationViewModel> retFilterEmployeeList;

            try
            {
                var employeeId = 0;
                var employeeName = "";
                var positionId = 0;
                var departmentId = 0;
                if (schFilter != null)
                {
                    employeeId = schFilter.EmployeeId ?? 0;
                    employeeName = schFilter.EmployeeName == null ? employeeName : schFilter.EmployeeName;
                    positionId = schFilter.PositionId ?? 0;
                    departmentId = schFilter.DepartmentId ?? 0;

                }
                //Calling Db Procedure
                var userInformationList = timeAideWebContext.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                         0, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).
                                         Where(w => w.DepartmentId == (departmentId == 0 ? w.DepartmentId : departmentId));



                retFilterEmployeeList = userInformationList;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retFilterEmployeeList;

        }
    
       

        public JsonResult AjaxGetPunchScheduleDates(EmployeeAttendenceSchDetail model)
        {
            string status = "Success";
            string message = "Successfully fetch!";
            EmployeeAttendenceSchDetail punchData = null;
           
            try
            {
               
                
                //punchData = model;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, punchData = model });
        }

        public JsonResult GetDashboardMyCurrentScheduleWidget(int? id)
        {
            // var userId = 248;//SessionHelper.LoginId;
            var userId = id ?? SessionHelper.LoginId;
            var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();
            string status = "Success";
            string message = "Successfully fetch!";
            EmployeeAttendenceSchedule widgetData = new EmployeeAttendenceSchedule();
            UserFilterViewModel schFilter = null;
            try
            {
              
              
            }

            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, widgetData = widgetData });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timeAideWebContext.Dispose();
                if (timeAideWindowContext != null)
                    timeAideWindowContext.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: UserContactInformation/Edit/5



    }
}
