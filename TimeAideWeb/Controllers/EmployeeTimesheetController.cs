
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
using TimeAide.Services;
using TimeAide.Data;

namespace TimeAide.Web.Controllers
{
    public class EmployeeTimesheetController : BaseTAWindowRoleRightsController<EmployeeAttendenceSchedule>
    {


        public ActionResult Index()
        {
            try
            {
                var loginUser = timeAideWebContext.UserInformation.Include("SupervisorSubDepartment.SubDepartment")
                                                                  .Include("SupervisorDepartment.Department")
                                                                  .Include("SupervisorEmployeeType.EmployeeType").FirstOrDefault(u => u.Id == SessionHelper.LoginId);
                var contextWin = DataHelper.GetSelectedClientTAWinEFContext();
                if (loginUser.SupervisorCompany.Count == 0 || loginUser.SupervisorCompany.Where(c => c.CompanyId == SessionHelper.SelectedCompanyId).Count() == 0)
                {
                    ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName").OrderBy(o => o.Text);
                    ViewBag.SubDepartmentId = new SelectList(new List<SubDepartment>(), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    ViewBag.EmployeeTypeId = new SelectList(new List<EmployeeType>(), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                    ViewBag.HasCompanyId = false;
                }
                else
                {
                    ViewBag.HasCompanyId = true;
                    if (loginUser.SupervisorDepartment.Count == 0)
                        ViewBag.DepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text); 
                    else
                        ViewBag.DepartmentId = new SelectList(loginUser.SupervisorDepartment.Select(d => d.Department), "Id", "DepartmentName").OrderBy(o => o.Text); 
                    if (loginUser.SupervisorSubDepartment.Count == 0)
                        ViewBag.SubDepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    else
                        ViewBag.SubDepartmentId = new SelectList(loginUser.SupervisorSubDepartment.Select(s => s.SubDepartment), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    if (loginUser.SupervisorEmployeeType.Count == 0)
                        ViewBag.EmployeeTypeId = new SelectList(timeAideWebContext.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName").OrderBy(o => o.Text); 
                    else
                        ViewBag.EmployeeTypeId = new SelectList(loginUser.SupervisorEmployeeType.Select(d => d.EmployeeType), "Id", "EmployeeTypeName").OrderBy(o => o.Text); 
                }
                List<SelectListItem> ReportTypeList = new List<SelectListItem>();
                ReportTypeList.Add(new SelectListItem { Text = "Timesheet", Value = "1" });
                ReportTypeList.Add(new SelectListItem { Text = "Punch Date", Value = "2" });
                ViewBag.ReportTypeId = new SelectList(ReportTypeList, "Value", "Text");

                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }

        public ActionResult ReportWeekList(tReportWeek model)
        {
            TimeAideContext db = new TimeAideContext();
            //model.e_id = model.UserInformationId;
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            model.SelectedEmployeeIds = SupervisorHelper.GetSupervisedEmployeeIds(model);
            var reportWeek = DataHelper.SP_GetTimesheetReportWeek<tReportWeek>(contextWinDb, model);
            ViewBag.SelectedReportWeekIds = string.Join(",", reportWeek.Select(s => s.tpwID.ToString()));

            return PartialView(reportWeek);
        }
        public ActionResult TimesheetDetail(tReportWeek model)
        {
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            var punchDate = new tPunchDate() { nWeekID = model.nWeekID };
            var punchDateList = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, punchDate);
            return PartialView(punchDateList);
        }

        public ActionResult DailyCompensation(tPunchDate model)
        {
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            var punchDate = DataHelper.SP_GetUserCompensationDaily<viewUserCompensationDaily>(contextWinDb, model.e_id, model.DTPunchDate);
            return PartialView(punchDate);
        }

        public ActionResult Today(int? employeeId)
        {
            //TimeAideContext db = new TimeAideContext();
            //var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            //tReportWeek model = new tReportWeek() { e_id = employeeId, DTStartDate=DateTime.Now.Date.AddYears(-3),DTEndDate=DateTime.Now.Date.AddDays(1).AddTicks(-1) };
            //tReportWeek reportWeek = DataHelper.SP_GetReportWeek<tReportWeek>(contextWinDb, model).FirstOrDefault();
            //if (reportWeek != null)
            //{
            //    var parameter = new tPunchDate() { nWeekID = Convert.ToInt64( reportWeek.nWeekID)};
            //    reportWeek.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, parameter).Take(1).ToList();
            //}
            tReportWeek model = null;
            var todayDte = DateTime.Today;
            try
            {

                model = timeAideWindowContext.tReportWeeks.Where(w => w.e_id == employeeId &&
                                                            todayDte >= w.DTStartDate && todayDte <= w.DTEndDate).FirstOrDefault();

                if (model != null)
                {
                    model.tPunchDate = timeAideWindowContext.tPunchDates.Where(w => w.e_id == model.e_id && w.nWeekID == model.nWeekID && w.DTPunchDate == todayDte).Take(1).ToList<tPunchDate>();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeTimesheet", "Today");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView(model);
        }
        public ActionResult SelfServiceTimesheet(string id)
        {
            //int? id = SessionHelper.LoginId;
            ViewBag.selfServiceTSBeforeDD = "-15d";
            ViewBag.selfServiceTSAfterDD = "+15d";
            var userId = id == null ? SessionHelper.LoginId : int.Parse(Encryption.DecryptURLParm(id));
            ViewBag.userId = userId;
            var userInfo = UserInformationService.GetUserInformation(userId);
            ViewBag.employeeId = userInfo.EmployeeId;
            var appConfigTSBeforeDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceTSBeforeDays")
                                                                                   .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            var appConfigTSAfterDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceTSAfterDays")
                                                                                   .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            int intTSBeforeDD;
            int intTSAfterDD;

            if (int.TryParse(appConfigTSBeforeDD, out intTSBeforeDD))
            {
                ViewBag.selfServiceTSBeforeDD = string.Format("-{0}d", appConfigTSBeforeDD);
            }
            if (int.TryParse(appConfigTSAfterDD, out intTSAfterDD))
            {
                ViewBag.selfServiceTSAfterDD = string.Format("+{0}d", appConfigTSAfterDD);
            }

            return PartialView("SelfServiceTimesheet");
        }
        public ActionResult GetSelfServiceTimesheetDetail(UserFilterViewModel timesheetFilter)
        {
            tReportWeek model = null;
            //  var userId = 248;//SessionHelper.LoginId;
            var empId = timesheetFilter.EmployeeId == 0 ? SessionHelper.LoginId : timesheetFilter.EmployeeId.Value;
            try
            {
                model = timeAideWindowContext.tReportWeeks.Where(w => w.e_id == empId && timesheetFilter.ScheduleDate >= w.DTStartDate && timesheetFilter.ScheduleDate <= w.DTEndDate).FirstOrDefault();

                if (model != null)
                {
                    model.tPunchDate = timeAideWindowContext.tPunchDates.Where(w => w.e_id == model.e_id && w.nWeekID == model.nWeekID).OrderBy(o => o.DTPunchDate).ToList<tPunchDate>();
                }
                else
                {
                    // model = new tReportWeek() { e_id = timesheetFilter.EmployeeId };
                    return Content("No Timesheet Available");
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeTimesheet", "GetSelfServiceTimesheetDetail");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView("SelfServiceTimesheetDetail", model);
        }
        public ActionResult ApproveEmployeeTimesheetView(tReportWeek model)
        {
            TimeAideContext db = new TimeAideContext();
            var usrInfo = db.UserInformation.FirstOrDefault(i => i.Id == SessionHelper.LoginId);
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            tReportWeek reportWeekEntity = null;
            if (model.intUserReviewed==1)//Reviewed
            {
             reportWeekEntity = contextWinDb.tReportWeeks.Where(w=>w.nWeekID==model.nWeekID && w.e_id==model.e_id).FirstOrDefault();
                if (reportWeekEntity != null)
                {
                    reportWeekEntity.intUserReviewed = model.intUserReviewed;
                    reportWeekEntity.intUserReviewedID= usrInfo.EmployeeId;
                    reportWeekEntity.strUserReviewedName=usrInfo.ShortFullName;
                    reportWeekEntity.dtUserReviewDate=DateTime.Now;
                    contextWinDb.SaveChanges();
                    if (reportWeekEntity != null)
                    {
                        var parameter = new tPunchDate() { nWeekID = Convert.ToInt64(reportWeekEntity.nWeekID) };
                        reportWeekEntity.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(timeAideWindowContext, parameter);
                    }
                    
                }           
            
            }
            //   DataHelper.SP_SelfService_ApproveTimesheet<tReportWeek>(timeAideWindowContext, model, false);
            //    tReportWeek reportWeek = DataHelper.SP_GetReportWeek<tReportWeek>(timeAideWindowContext, model).FirstOrDefault();

            ////reportWeek.UserInformationId = u.Id;
            //return Json(reportWeek);
            return Json(reportWeekEntity);
        }
        public JsonResult SupervisorNotes(long? refrenceId, int? userId)
        {
            var db = new TimeAideContext();
            var userInfo = db.UserInformation.FirstOrDefault(u => u.EmployeeId == userId && u.ClientId==SessionHelper.SelectedClientId);
            var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId1 == refrenceId && d.AuditLog.UserInformationId == userInfo.Id && d.AuditLog.TableName == "TimesheetApproval" && d.ColumnName== "Notes" && d.ClientId == SessionHelper.SelectedClientId).ToList();
            string notes = "";
            foreach (var each in entitySet)
            {
                var createdByUser = db.UserInformation.FirstOrDefault(u => u.Id == each.AuditLog.CreatedBy);
                notes += createdByUser.FullName + " added notes on " + each.CreatedDate.ToString() +": "+ each.NewValue + "<br>";
            }
            //return Json(new { status = "Success", notes = notes });
            return Json(notes, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult ChangeHistory(long refrenceId,int userId, string tableName)
        {
            try
            {
                var db = new TimeAideContext();
                //AllowChangeHistory();
                var userInfo = db.UserInformation.FirstOrDefault(u => u.EmployeeId == userId && u.ClientId == SessionHelper.SelectedClientId);
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId1 == refrenceId && d.AuditLog.UserInformationId== userInfo.Id &&  d.AuditLog.TableName == "TimesheetApproval" && d.ClientId == SessionHelper.SelectedClientId).ToList();
                if (entitySet.Count > 0)
                {
                    foreach (var each in entitySet.Select(d=>d.AuditLog))
                    {
                        each.CreatedByUser = db.UserInformation.FirstOrDefault(u => u.Id == each.CreatedBy);
                    }
                    //string tablename = entitySet.FirstOrDefault().AuditLog.TableName;
                    //int referenceId = entitySet.FirstOrDefault().AuditLog.ReferenceId;
                    //var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + tablename + " where " + tablename + "Id = " + referenceId.ToString() + "").FirstOrDefault();
                    //if (refrenceObject != null)
                    //{
                    //    ViewBag.ReferenceObject = refrenceObject;
                    //    ViewBag.TableName = tablename;
                    //    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                    //}
                }
                //var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                //if (refrenceObject != null)
                {
                    //ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = "";
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = "";
                }
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
    }
}
