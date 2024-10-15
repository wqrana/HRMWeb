using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class PayslipController : Controller
    {// moved to timesheet controller
        // GET: Payslip
        //public ActionResult EmployeePayslipView(int? userInformationId)
        //{
        //    TimeAideContext db = new TimeAideContext();
        //    //var u = db.UserInformation.FirstOrDefault(i => i.Id == userInformationId);
        //    var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
        //    tReportWeek model = new tReportWeek() { e_id = userInformationId };
        //    tReportWeek reportWeek = DataHelper.SP_GetReportWeek<tReportWeek>(contextWinDb, model).FirstOrDefault();
        //    if (reportWeek != null)
        //    {
        //        var parameter = new tPunchDate() { nWeekID = Convert.ToInt64(reportWeek.nWeekID) };
        //        reportWeek.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, parameter);
        //    }
        //    //reportWeek.UserInformationId = u.Id;
        //    return View(reportWeek);
        //}
        //public ActionResult ApproveEmployeePayslipView(tReportWeek model)
        //{
        //    TimeAideContext db = new TimeAideContext();
        //    //var u = db.UserInformation.FirstOrDefault(i => i.Id == model.UserInformationId);
        //    var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
        //    DataHelper.SP_SelfService_ApproveTimesheet<tReportWeek>(contextWinDb, model,false);
        //    tReportWeek reportWeek = DataHelper.SP_GetReportWeek<tReportWeek>(contextWinDb, model).FirstOrDefault();
        //    if (reportWeek != null)
        //    {
        //        var parameter = new tPunchDate() { nWeekID = Convert.ToInt64(reportWeek.nWeekID) };
        //        reportWeek.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, parameter);
        //    }
        //    //reportWeek.UserInformationId = u.Id;
        //    return Json(reportWeek);
        //}

        public ActionResult ApproveTimesheetBySupervisor(tReportWeek model)
        {
            //TimeAideContext db = new TimeAideContext();
            //var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            //var dbObject = DataHelper.GetReportWeek<tReportWeek>(contextWinDb, model);
            //DataHelper.SP_SelfService_ApproveTimesheet<tReportWeek>(contextWinDb, model, true);
            //tReportWeek reportWeek = DataHelper.SP_GetReportWeek<tReportWeek>(contextWinDb, model).FirstOrDefault();
            //if (reportWeek != null)
            //{
            //    var parameter = new tPunchDate() { nWeekID = Convert.ToInt64(reportWeek.nWeekID) };
            //    reportWeek.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, parameter);
            //}
            TimeAideContext db = new TimeAideContext();
            var usrInfo = db.UserInformation.FirstOrDefault(i => i.Id == SessionHelper.LoginId);
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            var dbObject = DataHelper.GetReportWeek<tReportWeek>(contextWinDb, model);
            var reportWeekEntity = contextWinDb.tReportWeeks.Where(w => w.nWeekID == model.nWeekID && w.e_id == model.e_id).FirstOrDefault();
                if (reportWeekEntity != null)
                {
                    reportWeekEntity.nReviewStatus = model.nReviewStatus;
                    reportWeekEntity.nReviewSupervisorID = usrInfo.EmployeeId;
                    reportWeekEntity.sSupervisorName = usrInfo.ShortFullName;                    
                    contextWinDb.SaveChanges();
                    if (reportWeekEntity != null)
                    {
                        var parameter = new tPunchDate() { nWeekID = Convert.ToInt64(reportWeekEntity.nWeekID) };
                        reportWeekEntity.tPunchDate = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, parameter);
                    }

                }
                AddAuditLog(model, dbObject);

            return Json(reportWeekEntity);
        }

        private void AddAuditLog(tReportWeek model,tReportWeek tReportWeek)
        {
            var db = new TimeAideContext();
            var userInfo = db.UserInformation.FirstOrDefault(u => u.EmployeeId == model.e_id && u.ClientId==SessionHelper.SelectedClientId);
            var log = new AuditLog()
            {
                ActionType = "Modified",
                TableName = "TimesheetApproval",
                ReferenceId1 = (model.nWeekID ?? 0),
                UserInformationId = userInfo.Id,
            };
            if (SessionHelper.UserSessionLogDetailId != 0)
                log.UserSessionLogDetailId = SessionHelper.UserSessionLogDetailId;
            if (model.nReviewStatus != tReportWeek.nReviewStatus)
            {
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "nReviewStatus",
                    OldValue = (tReportWeek.nReviewStatus ?? 0).ToString(),
                    NewValue = (model.nReviewStatus ?? 0).ToString(),
                    CompanyId = SessionHelper.SelectedCompanyId,
                    ClientId = SessionHelper.SelectedClientId
                };
                log.AuditLogDetail.Add(logDetail);
            }

            if (model.Notes != tReportWeek.Notes)
            {
                var logDetail = new AuditLogDetail()
                {
                    AuditLog = log,
                    ColumnName = "Notes",
                    OldValue = "",
                    NewValue = model.Notes,
                    CompanyId = SessionHelper.SelectedCompanyId,
                    ClientId = SessionHelper.SelectedClientId
                };
                log.AuditLogDetail.Add(logDetail);
            }

            db.AuditLog.Add(log);
            db.SaveChanges();
        }
    }
}