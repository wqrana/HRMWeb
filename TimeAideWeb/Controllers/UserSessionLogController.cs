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
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class UserSessionLogController : TimeAideWebControllers<UserSessionLog>
    {
        public virtual ActionResult IndexUserSessionLog()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();
                var model = db.UserSessionLog.Where(u=>(u.CompanyId== SessionHelper.SelectedCompanyId || u.UserSessionLogDetail.Any(ud=>ud.CompanyId== SessionHelper.SelectedCompanyId)) 
                                                    && (u.ClientId==SessionHelper.SelectedClientId || u.UserSessionLogDetail.Any(ud => ud.ClientId == SessionHelper.SelectedClientId)))
                                                    .OrderByDescending(e => e.CreatedDate).ToList();
                model = OnIndex(model);
                UserSessionLogViewModel viewModel = new UserSessionLogViewModel();
                viewModel.UserSessionLog = model;
                return PartialView("Index", viewModel);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(UserSessionLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult IndexAllUsers()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();
                var model = db.UserSessionLog.Where(u => (u.CompanyId == SessionHelper.SelectedCompanyId || u.UserSessionLogDetail.Any(ud => ud.CompanyId == SessionHelper.SelectedCompanyId))
                                                    && (u.ClientId == SessionHelper.SelectedClientId || u.UserSessionLogDetail.Any(ud => ud.ClientId == SessionHelper.SelectedClientId)))
                                                    .OrderByDescending(e => e.CreatedDate).ToList();
                model = OnIndex(model);
                UserSessionLogViewModel viewModel = new UserSessionLogViewModel();
                viewModel.UserSessionLog = model;
                return PartialView(viewModel);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(UserSessionLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        
        [HttpPost]
        public virtual ActionResult FilterUserSessionLog(UserSessionLogViewModel viewModel)
        {
            try
            {
                Expression<Func<UserSessionLog, bool>> predicate =u=> 1==1;
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();

                var model = db.UserSessionLog.Where(u => (u.CompanyId == SessionHelper.SelectedCompanyId || u.UserSessionLogDetail.Any(ud => ud.CompanyId == SessionHelper.SelectedCompanyId))
                                                    && (u.ClientId == SessionHelper.SelectedClientId || u.UserSessionLogDetail.Any(ud => ud.ClientId == SessionHelper.SelectedClientId)))
                              .Where(u=> string.IsNullOrEmpty(viewModel.FullName) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.FirstLastName) && u.UserInformation.FirstLastName.ToLower().Contains(viewModel.FullName.ToLower())) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.FirstName) && u.UserInformation.FirstName.ToLower().Contains(viewModel.FullName.ToLower())) || 
                                    (!string.IsNullOrEmpty(u.UserInformation.SecondLastName) && u.UserInformation.SecondLastName.ToLower().Contains(viewModel.FullName.ToLower()))||
                                    (!string.IsNullOrEmpty(u.UserInformation.ShortFullName) && u.UserInformation.ShortFullName.ToLower().Contains(viewModel.FullName.ToLower())))
                              .Where(u => viewModel.EmployeeId<=0 || !u.UserInformation.EmployeeId.HasValue ||
                                    ((u.UserInformation.EmployeeId??0).ToString().Contains(viewModel.EmployeeId.ToString())))
                              .OrderByDescending(e => e.CreatedDate).ToList();

                model = OnIndex(model);
                viewModel = new UserSessionLogViewModel();
                viewModel.UserSessionLog = model;
                return PartialView("_UserSessionList", viewModel);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(UserSessionLog).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult SessionDetail(int? id)
        {
            try
            {
                //AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = db.UserSessionLogDetail.Where(i=>i.UserSessionLogId == id.Value);
                if (model == null)
                {
                    return PartialView();
                }
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
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
