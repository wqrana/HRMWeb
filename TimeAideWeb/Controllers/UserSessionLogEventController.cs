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
    public class UserSessionLogEventController : TimeAideWebControllers<UserSessionLogEvent>
    {
        public virtual ActionResult IndexUserSessionLogEvent()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();
                var model = db.UserSessionLogEvent
                    //.Where(u => u.DataEntryStatus==1 && ((!u.CompanyId.HasValue || u.CompanyId == SessionHelper.SelectedCompanyId) && (!u.ClientId.HasValue || u.ClientId == SessionHelper.SelectedClientId)))
                    .Where(u => u.DataEntryStatus == 1)
                    .OrderByDescending(e => e.CreatedDate).ToList();
                UserSessionLogViewModel viewModel = new UserSessionLogViewModel();
                //viewModel.UserSessionLog = model;
                return PartialView("Index", model);
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
        //[ValidateAntiForgeryToken]
        public ActionResult Create(UserSessionLogEvent model)
        {
            if (ModelState.IsValid)
            {
                db.UserSessionLogEvent.Add(model);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                }
                return Json(model);
            }
            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(UserSessionLogEvent model)
        {
            if (ModelState.IsValid)
            {
                model.SetUpdated<UserSessionLogEvent>();
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(model);
            }
            return GetErrors();
        }

        public JsonResult AjaxGetClientCompany(int? clientId)
        {
            var companyName = db.Company
                                .Where(w => w.ClientId == clientId && w.DataEntryStatus == 1).OrderBy(o => o.CompanyName)
                                .Select(s => new { id = s.Id, name = s.CompanyName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = companyName,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
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
