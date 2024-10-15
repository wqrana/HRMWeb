using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmailTemplateController : TimeAideWebControllers<EmailTemplate>
    {

        // POST: Credential/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EmailTemplate emailTemplate)
        {
            if (ModelState.IsValid)
            {
                AddValidation(emailTemplate);
            }

            if (ModelState.IsValid)
            {
                db.EmailTemplate.Add(emailTemplate);
                db.SaveChanges();
                return Json(emailTemplate);
            }

            return GetErrors();
        }

        private void AddValidation(EmailTemplate emailTemplate)
        {
            var model = db.GetAllByCompany<EmailTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(x => x.EmailTypeId == emailTemplate.EmailTypeId).ToList();
            var canCreateAllCompany = model.Any(t => !t.CompanyId.HasValue);
            var canCreateForCompany = model.Any(t => t.CompanyId.HasValue);
            if (emailTemplate.IsAllCompanies)
            {
                if (canCreateAllCompany)
                {
                    ModelState.AddModelError("OneRecordPerCompanyValidation", "Cannot add all companies email template. Selected client already has a template defined for all compnies.");
                }
            }
            else
            {
                if (canCreateForCompany)
                {
                    ModelState.AddModelError("OneRecordPerCompanyValidation", "Cannot add email template. Selected company already has a template.");
                }
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var model = db.GetAllByCompany<EmailTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList();
            ViewBag.CanCreateAllCompany = model.Count(t => !t.CompanyId.HasValue);
            ViewBag.CanCreateForCompany = model.Count(t => t.CompanyId.HasValue);
        }
        // POST: Credential/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmailTemplate emailTemplate)
        {
            TimeAideContext db2 = new TimeAideContext();
            var dbRecord = db2.Find<EmailTemplate>(emailTemplate.Id, SessionHelper.SelectedClientId);
            if (ModelState.IsValid && dbRecord.IsAllCompanies != emailTemplate.IsAllCompanies)
            {
                AddValidation(emailTemplate);
            }

            if (ModelState.IsValid)
            {
                db.Entry(emailTemplate).State = EntityState.Modified;
                emailTemplate.SetUpdated<EmailTemplate>();
                db.SaveChanges();
                return Json(emailTemplate);
            }
            return GetErrors();
        }
        [HttpGet]
        [Route("EmailBlastTemplte/{id}/{emailBody}/{emailSubject}")]
        public virtual ActionResult EmailBlastTemplte(int? id,string emailBody, string emailSubject)
        {
            //int id, string EmailSubject, string EmailBody
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "Create");
                AllowAdd();
                ViewBag.Label = ViewBag.Label + " - Add";
                ViewBag.IsBaseCompanyObject = false;
                ViewBag.IsAllCompanies = false;
                ViewBag.CanBeAssignedToCurrentCompany = true;
                OnCreate();
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public virtual ActionResult EmailBlastTemplte(EmailTemplate emailTemplate)
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "Create");
                AllowAdd();
                if (ModelState.IsValid)
                {
                    //db.EmailTemplate.Add(emailTemplate);
                    //db.SaveChanges();
                    return Json(emailTemplate);
                }

                return GetErrors();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmailTemplate", "Index");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
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
