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
    public class EmployeeAppraisalSkillController : TimeAideWebControllers<EmployeeAppraisalSkill>
    {

        public ActionResult IndexByAppraisal(int? id)
        {
            try
            {
                AllowView();
                var model = db.EmployeeAppraisalSkill.Where(w => w.DataEntryStatus == 1 && w.EmployeeAppraisalId == id);
                return PartialView("Index", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AppraisalTemplate).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }


        public ActionResult CreateByAppraisal(int id)
        {
            try
            {
                AllowAdd();

                ViewBag.AppraisalSkillId = new SelectList(db.GetAllByCompany<AppraisalSkill>(SessionHelper.CompanyId,SessionHelper.SelectedClientId), "Id", "SkillName");
                ViewBag.AppraisalRatingScaleDetailId = new SelectList(db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId == 0), "Id", "RatingName");

                var model = new EmployeeAppraisalSkill();
                return PartialView("Create", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.EmployeeAppraisalSkill.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.AppraisalSkillId = new SelectList(db.GetAllByCompany<AppraisalSkill>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "SkillName", model.AppraisalSkillId);
                ViewBag.AppraisalRatingScaleDetailId = new SelectList(db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId == model.AppraisalSkill.AppraisalRatingScaleId), "Id", "RatingName", model.AppraisalRatingScaleDetailId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult CreateEdit(EmployeeAppraisalSkill model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAppraisalSkill employeeAppraisalSkillEntity = null;
            try
            {

                if (model.Id == 0)
                {

                    employeeAppraisalSkillEntity = new EmployeeAppraisalSkill();
                    employeeAppraisalSkillEntity.EmployeeAppraisalId = model.EmployeeAppraisalId;
                    db.EmployeeAppraisalSkill.Add(employeeAppraisalSkillEntity);
                }
                else
                {
                    employeeAppraisalSkillEntity = db.EmployeeAppraisalSkill.Find(model.Id);
                    employeeAppraisalSkillEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeAppraisalSkillEntity.ModifiedDate = DateTime.Now;
                }
                employeeAppraisalSkillEntity.AppraisalSkillId = model.AppraisalSkillId;
                employeeAppraisalSkillEntity.AppraisalRatingScaleDetailId = model.AppraisalRatingScaleDetailId;
                employeeAppraisalSkillEntity.SkillRatingName = model.SkillRatingName;
                employeeAppraisalSkillEntity.SkillRatingValue = model.SkillRatingValue;
                employeeAppraisalSkillEntity.SkillScaleMaxValue = model.SkillScaleMaxValue;
                employeeAppraisalSkillEntity.ReviewerComments = model.ReviewerComments;
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

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var employeeAppraisalSkillEntity = db.EmployeeAppraisalSkill.Find(id);
            try
            {
                employeeAppraisalSkillEntity.ModifiedBy = SessionHelper.LoginId;
                employeeAppraisalSkillEntity.ModifiedDate = DateTime.Now;
                employeeAppraisalSkillEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult AppraisalSkillSelectionPopUp(int? id)
        {
            string TemplateName = "";
            var appraisalSkillSelectionList = new SelectList(db.AppraisalSkill.Where(w => w.Id == -1), "Id", "SkillName");

            if (id != null)
            {
                var appraisalTemplateSkillList = db.AppraisalTemplateSkill.Where(w => w.DataEntryStatus == 1 && w.AppraisalTemplateId == id).Select(s => s.AppraisalSkill);
                appraisalSkillSelectionList = new SelectList(appraisalTemplateSkillList.Where(w => w.DataEntryStatus == 1), "Id", "SkillName");
                TemplateName = db.AppraisalTemplate.Where(w => w.Id == id).Select(s => s.TemplateName).FirstOrDefault();
            }
            ViewBag.TemplateName = (TemplateName == "" || TemplateName == null) ? "Not Available" : TemplateName;
            ViewBag.AppraisalSkillSelectionId = appraisalSkillSelectionList;

            return PartialView("SkillSelectionPopUp");
        }
        public JsonResult AjaxGetAppraisalSkillList(int? id, bool isAllMasterData)
        {
            dynamic appraisalSkillSelectionList = db.AppraisalSkill.Where(w => w.Id == -1).Select(s => new { id = s.Id, text = s.SkillName });
            if (isAllMasterData == false)
            {
                if (id != null)
                {
                    var appraisalTemplateSkillList = db.AppraisalTemplateSkill.Where(w => w.DataEntryStatus == 1 && w.AppraisalTemplateId == id).Select(s => s.AppraisalSkill);
                    appraisalSkillSelectionList = appraisalTemplateSkillList.Select(s => new { id = s.Id, text = s.SkillName });
                }
            }
            else
            {
                appraisalSkillSelectionList = db.GetAllByCompany<AppraisalSkill>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.SkillName });
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalSkillSelectionList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }

        public JsonResult AjaxGetAppraisalSkillRatingList(int? id)
        {
            int appraisalSkillRatingId = db.AppraisalSkill.Where(w => w.Id == id).Select(s => s.AppraisalRatingScaleId).FirstOrDefault();

            dynamic appraisalSkillRatingList = db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId == appraisalSkillRatingId).Select(s => new { id = s.Id, text = s.RatingName });


            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalSkillRatingList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }

        public JsonResult AjaxGetAppraisalSkillRatingValue(int? id)
        {

            dynamic appraisalSkillRatingValue = db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.Id == id)
                                                .Select(s => new { id = s.Id, ratingValue = s.RatingValue, maxRatingValue = s.AppraisalRatingScale.ScaleMaxValue }).FirstOrDefault();


            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalSkillRatingValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }
    }
}


