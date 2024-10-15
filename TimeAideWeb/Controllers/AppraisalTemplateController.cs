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
    public class AppraisalTemplateController : TimeAideWebControllers<AppraisalTemplate>
    {

        public ActionResult IndexByTemplate()
        {
            try
            {
                AllowView();
                var entitySet = db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                var abc = entitySet.OrderByDescending(e => e.CreatedDate).ToList();
                return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AppraisalTemplate).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
                      
        }
        public override ActionResult Details(int? id)
        {
            try
            {
                AppraisalTemplate model = null;
                AllowView();
                if (id == 0)
                 model = db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderByDescending(o => o.Id).FirstOrDefault();
                else 
                    model = db.Find<AppraisalTemplate>(id.Value, SessionHelper.SelectedClientId);

                if (model == null)
                 model = new AppraisalTemplate();
               
                //return View(city);
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AppraisalTemplate", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public override ActionResult Create()
        {

            return PartialView();
        }
        public override ActionResult Edit(int? id)
        { 
            var model = db.AppraisalTemplate.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.CanBeAssignedToCurrentCompany = false;

            if (!model.CompanyId.HasValue)
            {
                model.IsAllCompanies = true;
            }
            var companies = model.GetRefferredCompanies();
            if (companies.Count == 0 )//|| (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
            {
                ViewBag.CanBeAssignedToCurrentCompany = true;
            }
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateEdit(AppraisalTemplate model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            AppraisalTemplate appraisalTemplateEntity = null;
            try
            {
                var isAlreadyExist = db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                        .Where(w => w.DataEntryStatus == 1 && (w.Id != model.Id) && (w.TemplateName.ToLower() == model.TemplateName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Template Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        appraisalTemplateEntity = new AppraisalTemplate();
                        db.AppraisalTemplate.Add(appraisalTemplateEntity);
                    }
                    else
                    {
                        appraisalTemplateEntity = db.AppraisalTemplate.Find(model.Id);
                        appraisalTemplateEntity.ModifiedBy = SessionHelper.LoginId;
                        appraisalTemplateEntity.ModifiedDate = DateTime.Now;
                    }
                    appraisalTemplateEntity.TemplateName = model.TemplateName;
                    appraisalTemplateEntity.CompanyId = model.IsAllCompanies ? null : (int?)SessionHelper.SelectedCompanyId;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var appraisalTemplateEntity = db.AppraisalTemplate.Find(id);
            try
            {
                appraisalTemplateEntity.ModifiedBy = SessionHelper.LoginId;
                appraisalTemplateEntity.ModifiedDate = DateTime.Now;
                appraisalTemplateEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }

    }
}