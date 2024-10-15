
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ApplicantCustomFieldController : BaseApplicantRoleRightsController<ApplicantCustomField>
    {
      
        [HttpGet]
        // GET: 
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantCustomField.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
       
     
        public ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.CustomFieldId = new SelectList(db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "CustomFieldName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantCustomField", "Create");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantCustomField.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.CustomFieldId = new SelectList(db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "CustomFieldName", model.CustomFieldId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantCustomField", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      public JsonResult AjaxCheckExpirableCustomField(int id)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            bool isExpirable = false;
           
            if (id > 0)
            {
                try
                {
                    isExpirable = db.CustomField.Where(w => w.Id == id)
                                    .Select(s => s.IsExpirable)
                                    .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {
                status = "Error";
                message = "Invalid record data!";
            }
            retResult = new { status = status, message = message, isExpirable = isExpirable };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult CreateEdit(ApplicantCustomField model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantCustomField applicantustomFieldEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    applicantustomFieldEntity = new ApplicantCustomField();
                    applicantustomFieldEntity.ApplicantInformationId = model.ApplicantInformationId;
                    db.ApplicantCustomField.Add(applicantustomFieldEntity);
                }
                else
                {
                    applicantustomFieldEntity = db.ApplicantCustomField.Find(model.Id);
                    applicantustomFieldEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantustomFieldEntity.ModifiedDate = DateTime.Now;
                }
                applicantustomFieldEntity.CustomFieldId = model.CustomFieldId;
                applicantustomFieldEntity.CustomFieldValue = model.CustomFieldValue;
                applicantustomFieldEntity.CustomFieldNote = model.CustomFieldNote;
                applicantustomFieldEntity.ExpirationDate = model.ExpirationDate;

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

        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();
                var model = db.ApplicantCustomField.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantCustomField", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantCustomFieldEntity = db.ApplicantCustomField.Find(id);
            try
            {
                applicantCustomFieldEntity.ModifiedBy = SessionHelper.LoginId;
                applicantCustomFieldEntity.ModifiedDate = DateTime.Now;
                applicantCustomFieldEntity.DataEntryStatus = 0;
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