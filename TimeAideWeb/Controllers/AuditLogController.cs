using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class AuditLogController : TimeAideWebControllers<AuditLog>
    {
        public override ActionResult Index()
        {
            try
            {
                var actionTypeList = new List<string> { "Added", "Modified", "Deleted" }; ;
                var emptyList = new List<string>();
                var excludingRecordTypes = new List<string> { "AuditLog", "AuditLogDetail", "UserSessionLog", "UserSessionLogDetail", "NotificationServiceEvent", "DataMigrationLog", "DataMigrationLogDetail" };
                
                ViewBag.ActionType = new SelectList(actionTypeList);
                ViewBag.RecordType =new SelectList(db.GetAll<AuditLog>(SessionHelper.SelectedClientId).Where(w=> !excludingRecordTypes.Contains(w.TableName)).Select(s=>s.TableName).Distinct().OrderBy(o=>o));
                ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.CompanyId == SessionHelper.SelectedCompanyId && w.Id != SessionHelper.LoginId).Select(s => new { id = s.Id, text = s.ShortFullName }).OrderBy(o => o.text),"id","text");
                AllowView();
                
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AuditLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult IndexAuditLog()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();
                var model = db.AuditLog.Where(u=>(u.CompanyId== SessionHelper.SelectedCompanyId || u.AuditLogDetail.Any(ud=>ud.CompanyId== SessionHelper.SelectedCompanyId)) 
                                                    && (u.ClientId==SessionHelper.SelectedClientId || u.AuditLogDetail.Any(ud => ud.ClientId == SessionHelper.SelectedClientId)))
                                                    .OrderByDescending(e => e.CreatedDate).ToList();
                model = OnIndex(model);
                AuditLogViewModel viewModel = new AuditLogViewModel();
                viewModel.AuditLog = model;
                return PartialView("Index", viewModel);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AuditLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public virtual ActionResult GetFilteredAuditLogList(AuditLogFilterViewModel filterViewModel)
        {
            try
            {
               
                AllowView();
                // var model = new List<AuditLogListViewModel>();
                filterViewModel.SupervisorId = filterViewModel.SupervisorId == 0 ? SessionHelper.LoginId : filterViewModel.SupervisorId;
                var model = db.SP_AuditLogInfo<AuditLogListViewModel>(filterViewModel.FromDate, filterViewModel.ToDate, filterViewModel.EmployeeId ?? 0, 
                                                                      filterViewModel.EmployeeName, filterViewModel.ActionType, filterViewModel.RecordType, filterViewModel.SupervisorId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);
               
                return PartialView("AuditLogDetailList", model);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AuditLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public virtual ActionResult FilterAuditLog(AuditLogViewModel viewModel)
        {
            try
            {
                Expression<Func<AuditLog, bool>> predicate =u=> 1==1;
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();

                var model = db.AuditLog.Where(u => (u.CompanyId == SessionHelper.SelectedCompanyId || u.AuditLogDetail.Any(ud => ud.CompanyId == SessionHelper.SelectedCompanyId))
                                                    && (u.ClientId == SessionHelper.SelectedClientId || u.AuditLogDetail.Any(ud => ud.ClientId == SessionHelper.SelectedClientId)))
                              .Where(u=> string.IsNullOrEmpty(viewModel.FullName) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.FirstLastName) && u.UserInformation.FirstLastName.ToLower().Contains(viewModel.FullName.ToLower())) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.FirstName) && u.UserInformation.FirstName.ToLower().Contains(viewModel.FullName.ToLower())) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.SecondLastName) && u.UserInformation.SecondLastName.ToLower().Contains(viewModel.FullName.ToLower()))||
                                    (!string.IsNullOrEmpty(u.UserInformation.ShortFullName) && u.UserInformation.ShortFullName.ToLower().Contains(viewModel.FullName.ToLower())))
                              .Where(u => viewModel.EmployeeId<=0 || !u.UserInformation.EmployeeId.HasValue ||
                                    ((u.UserInformation.EmployeeId??0).ToString().Contains(viewModel.EmployeeId.ToString())))
                              .OrderByDescending(e => e.CreatedDate).ToList();

                model = OnIndex(model);
                viewModel = new AuditLogViewModel();
                viewModel.AuditLog = model;
                return PartialView("_AuditLogList", viewModel);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AuditLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult AuditLogDetail(int? id)
        {
            try
            {
                //AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = db.AuditLogDetail.Where(i=>i.AuditLogId == id.Value);
                if (model == null)
                {
                    return PartialView();
                }
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AuditLog", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Country.Include(u => u.States)
                                      .Include(u => u.MailingCountryUserContactInformation)
                                      .Include(u => u.HomeCountryUserContactInformation)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.States.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.MailingCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.HomeCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
