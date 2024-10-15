using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmailBlastController : TimeAideWebControllers<EmailBlast>
    {
        public override List<EmailBlast> OnIndex(List<EmailBlast> model)
        {
            return model.OrderByDescending(m => m.IsSavedAsDraft ? 0 : 1).ThenByDescending(m => m.Id).ToList();
        }
        [HttpPost]
        public ActionResult Create(EmailBlast model)
        {
            dynamic retResult = null;
          
            if (ModelState.IsValid)
            {

                List<string> selectedEmployees = (model.UserInformationIds ?? "").Split(',').ToList();
                SetEmailTemplate(model);
                AddEmailBlastDetail(model, selectedEmployees);
                db.EmailBlast.Add(model);
                db.SaveChanges();
                if (!model.IsSavedAsDraft)
                {
                    SendEmail(model);
                }
                return Json("success");
            }
            else
            {
                if (string.IsNullOrEmpty(model.EmailTemplate.EmailSubject.Trim()))
                {
                    retResult = new { id = 0, status = "Error", message = $"Please select Email Template" };
                    return Json(retResult);
                }

            }
            return GetErrors();
        }

        private void AddEmailBlastDetail(EmailBlast model, List<string> selectedEmployees)
        {
            foreach (string eachEmployee in selectedEmployees)
            {
                var blastEmail = db.GetAll<UserContactInformation>(SessionHelper.SelectedClientId).Where(u => u.UserInformationId.ToString() == eachEmployee).FirstOrDefault();
                if (blastEmail != null && !string.IsNullOrEmpty(blastEmail.NotificationEmail))
                {
                    EmailBlastDetail emailBlastDetail = new EmailBlastDetail();
                    emailBlastDetail.UserInformation = blastEmail.UserInformation;
                    emailBlastDetail.UserInformationId = blastEmail.UserInformation.Id;

                    model.EmailBlastDetail.Add(emailBlastDetail);
                }
            }
        }
        private void SetEmailTemplate(EmailBlast model)
        {
            EmailTemplate temp = model.EmailTemplate;
            if (model.Id>0 && model.EmailTemplateId > 0 && model.IsSavedAsDraft)
            {
                var tempTemplate = model.EmailTemplate;
                model.EmailTemplate = db.EmailTemplate.FirstOrDefault(c => c.Id == model.EmailTemplateId);
                model.EmailTemplate.EmailBody = tempTemplate.EmailBody;
                model.EmailTemplate.EmailSubject = tempTemplate.EmailSubject;
            }
            else //if (model.EmailTemplateId < 0 && !model.IsSavedAsDraft)
            {
                model.EmailTemplate = new EmailTemplate { EmailSubject = temp.EmailSubject, EmailBody = temp.EmailBody, EmailTypeId = 5 };
            }
        }
        private NotificationLog AddNotificationLog(EmailBlast model)
        {
            NotificationLog log = new NotificationLog() { DeliveryStatusId = (int)DeliveryStatus.PendingSend, NotificationTypeId = (int)NotificationTypes.EmailBlast };
            log.EmailBlast = model;
            model.NotificationLog.Add(log);
            List<string> toEmail = new List<string>();
            foreach (var each in model.EmailBlastDetail)
            {
                toEmail.Add(each.UserInformation.ActiveUserContactInformation.NotificationEmail);
            }
            NotificationLogEmail logEmail = new NotificationLogEmail();
            logEmail.ClientId = log.ClientId;
            logEmail.CompanyId = log.CompanyId;
            logEmail.CreatedBy = log.CreatedBy;
            logEmail.SenderAddress = ConfigurationManager.AppSettings["FromMail"].ToString();
            logEmail.ToAddress = string.Join(",", toEmail);
            logEmail.CcAddress = "";
            logEmail.BccAddress = "";
            logEmail.NotificationLog = log;
            log.NotificationLogEmail.Add(logEmail);
            db.SaveChanges();
            return log;
        }
        private void SendEmail(EmailBlast model)
        {
            NotificationLog log = AddNotificationLog(model);
            log.DeliveryStatusId = (int)DeliveryStatus.Sent;

            string messageTemplate = model.EmailTemplate.EmailBody;
            var AppUrl = ConfigurationManager.AppSettings["AppUrl"];
            string url = "<a href='" + AppUrl + "NotificationLog/ReadNotificationEmail/" + log.Id + "'>Please click to review the notification.</a>";

            var mailSetting = UtilityHelper.GetSenderEmailConfiguration(log.CompanyId.Value, log.ClientId.Value, db);
            foreach (var eachEmployee in model.EmailBlastDetail)
            {
                eachEmployee.SentDate = DateTime.Now;
                //var loginEmail = db.GetAll<UserContactInformation>(SessionHelper.SelectedClientId).Where(u => u.UserInformationId.ToString() == eachEmployee).FirstOrDefault();
                Dictionary<string, string> toEmails = new Dictionary<string, string>();
                //if (loginEmail != null && !string.IsNullOrEmpty(loginEmail.LoginEmail))
                {
                    toEmails.Add(eachEmployee.UserInformation.ActiveUserContactInformation.NotificationEmail, eachEmployee.UserInformation.ShortFullName);
                    
                    string message = UtilityHelper.ReplaceMessagePlaceholders(eachEmployee.UserInformation, log, messageTemplate, url, "");
                    UtilityHelper.SendEmailWithSenderSetting(mailSetting, toEmails, "", "", null, message, model.EmailTemplate.EmailSubject);
                }
            }

        }
        [HttpPost]
        public ActionResult SendEmailBlast(int emailBlastId)
        {
            var emailBlast = db.EmailBlast.Include("EmailBlastDetail")
                .Include("EmailBlastDetail.UserInformation").FirstOrDefault(b => b.Id == emailBlastId);
            SendEmail(emailBlast);
            emailBlast.IsSavedAsDraft = false;
            db.SaveChanges();
            return Json("success");
        }
        public virtual ActionResult EditEmailBlast(int? id, string editType)
        {
            try
            {
                AllowEdit();
                EmailBlast entity = db.Find<EmailBlast>(id.Value, SessionHelper.SelectedClientId);
                ViewBag.Label = ViewBag.Label + " - Edit";
                ViewBag.EditType = editType;
                return PartialView("Edit", entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public ActionResult Edit(EmailBlast model)
        {
            if (ModelState.IsValid)
            {
                model.SetUpdated<Role>();
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult IndexEmailBlastDetail(int emailBlastId)
        {
            List<EmailBlastDetail> entitySet = db.GetAll<EmailBlastDetail>(SessionHelper.SelectedClientId);
            return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).Where(b => b.EmailBlastId == emailBlastId).ToList());
        }
        public virtual ActionResult SendEmailBlastDetails(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = db.EmailBlast.Include("EmailBlastDetail")
                .Include("EmailBlastDetail.UserInformation").FirstOrDefault(b => b.Id == id);
                if (model == null)
                {
                    return PartialView();
                }

                OnDetails(model);

                ViewBag.Label = ViewBag.Label + " - Send";
                ViewBag.IsSend = true;
                return PartialView("Details", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult EmailBlastChangeHistory(int refrenceId)
        {
            try
            {

                AllowChangeHistory();
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
                var refrenceObject = db.EmailBlast.Where(e => e.Id == refrenceId).FirstOrDefault();
                if (refrenceObject.EmailBlastDetail.Count > 0)
                {
                    var detailId = refrenceObject.EmailBlastDetail.FirstOrDefault().Id;

                    var entitySet1 = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == detailId && d.AuditLog.TableName.Contains("EmailBlastDetail") && d.ClientId == SessionHelper.SelectedClientId).ToList();
                    entitySet = entitySet1.Union(entitySet).ToList();
                }
                var auditLog = db.AuditLog.FirstOrDefault(d => d.ReferenceId == refrenceId && d.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId);
                if (auditLog != null)
                {
                    ViewBag.AuditLog = auditLog;
                    ViewBag.Remarks = auditLog.Remarks;
                }

                
                //if (refrenceObject!=null)
                //{
                    
                //    var notificationLogs = db.NotificationLog.Where(e => e.EmailBlastId == refrenceId).FirstOrDefault();
                //    if (notificationLogs != null)
                //    {
                //        var entitySet1 = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == notificationLogs.Id && d.AuditLog.TableName == "NotificationLog" && d.ClientId == SessionHelper.SelectedClientId).ToList();
                //        entitySet = entitySet1.Union(entitySet).ToList();
                //    }
                //}

                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = FormName;
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView(entitySet.OrderBy(u => u.CreatedDate));
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override bool CheckBeforeDelete(int id)
        {
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
