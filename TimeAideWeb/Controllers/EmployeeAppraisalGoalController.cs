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
    public class EmployeeAppraisalGoalController : TimeAideWebControllers<EmployeeAppraisalGoal>
    {

        public ActionResult IndexByAppraisal(int? id)
        {
            try
            {
                AllowView();
                var model = db.EmployeeAppraisalGoal.Where(w => w.DataEntryStatus == 1 && w.EmployeeAppraisalId == id);
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
               
                ViewBag.AppraisalGoalId = new SelectList(db.GetAllByCompany<AppraisalGoal>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "GoalName");
                ViewBag.AppraisalRatingScaleDetailId = new SelectList(db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId==0), "Id", "RatingName");
                
                var model = new EmployeeAppraisalGoal();
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
                var model = db.EmployeeAppraisalGoal.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.AppraisalGoalId = new SelectList(db.GetAllByCompany<AppraisalGoal>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "GoalName",model.AppraisalGoalId);
                ViewBag.AppraisalRatingScaleDetailId = new SelectList(db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId == model.AppraisalGoal.AppraisalRatingScaleId), "Id", "RatingName",model.AppraisalRatingScaleDetailId);
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
        public JsonResult CreateEdit(EmployeeAppraisalGoal model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAppraisalGoal employeeAppraisalGoalEntity = null;
            try
            {

                if (model.Id == 0)
                {

                    employeeAppraisalGoalEntity = new EmployeeAppraisalGoal();
                    employeeAppraisalGoalEntity.EmployeeAppraisalId = model.EmployeeAppraisalId;
                    db.EmployeeAppraisalGoal.Add(employeeAppraisalGoalEntity);
                }
                else
                {
                    employeeAppraisalGoalEntity = db.EmployeeAppraisalGoal.Find(model.Id);
                    employeeAppraisalGoalEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeAppraisalGoalEntity.ModifiedDate = DateTime.Now;
                }
                employeeAppraisalGoalEntity.AppraisalGoalId = model.AppraisalGoalId;
                employeeAppraisalGoalEntity.AppraisalRatingScaleDetailId = model.AppraisalRatingScaleDetailId;
                employeeAppraisalGoalEntity.GoalRatingName = model.GoalRatingName;
                employeeAppraisalGoalEntity.GoalRatingValue = model.GoalRatingValue;
                employeeAppraisalGoalEntity.GoalScaleMaxValue = model.GoalScaleMaxValue;
                employeeAppraisalGoalEntity.ReviewerComments = model.ReviewerComments;
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
            var employeeAppraisalGoalEntity = db.EmployeeAppraisalGoal.Find(id);
            try
            {
                employeeAppraisalGoalEntity.ModifiedBy = SessionHelper.LoginId;
                employeeAppraisalGoalEntity.ModifiedDate = DateTime.Now;
                employeeAppraisalGoalEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult AppraisalGoalSelectionPopUp(int? id)
        {
            string TemplateName = "";
            var appraisalGoalSelectionList = new SelectList(db.AppraisalGoal.Where(w => w.Id == -1), "Id", "GoalName");

            if (id != null)
            {
                var appraisalTemplateGoalList = db.AppraisalTemplateGoal.Where(w => w.DataEntryStatus == 1 && w.AppraisalTemplateId == id).Select(s => s.AppraisalGoal);
                appraisalGoalSelectionList = new SelectList(appraisalTemplateGoalList.Where(w => w.DataEntryStatus == 1), "Id", "GoalName");
                TemplateName = db.AppraisalTemplate.Where(w => w.Id == id).Select(s => s.TemplateName).FirstOrDefault();
            }
            ViewBag.TemplateName = (TemplateName == "" || TemplateName == null) ? "Not Available" : TemplateName;
            ViewBag.AppraisalGoalSelectionId = appraisalGoalSelectionList;

            return PartialView("GoalSelectionPopUp");
        }
        public JsonResult AjaxGetAppraisalGoalList(int? id, bool isAllMasterData)
        {
            dynamic appraisalGoalSelectionList = db.AppraisalGoal.Where(w => w.Id == -1).Select(s => new { id = s.Id, text = s.GoalName });
            if (isAllMasterData == false)
            {
                if (id != null)
                {
                    var appraisalTemplateGoalList = db.AppraisalTemplateGoal.Where(w => w.DataEntryStatus == 1 && w.AppraisalTemplateId == id).Select(s => s.AppraisalGoal);
                    appraisalGoalSelectionList = appraisalTemplateGoalList.Select(s => new { id = s.Id, text = s.GoalName });
                }
            }
            else
            {
                appraisalGoalSelectionList = db.GetAllByCompany<AppraisalGoal>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.GoalName });
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalGoalSelectionList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }

        public JsonResult AjaxGetAppraisalGoalRatingList(int? id)
        {
            int appraisalGoalRatingId = db.AppraisalGoal.Where(w => w.Id == id).Select(s => s.AppraisalRatingScaleId).FirstOrDefault();
           
           dynamic appraisalGoalRatingList = db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.AppraisalRatingScaleId== appraisalGoalRatingId).Select(s => new { id = s.Id, text = s.RatingName});
            

            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalGoalRatingList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }

        public JsonResult AjaxGetAppraisalGoalRatingValue(int? id)
        {
            
            dynamic appraisalGoalRatingValue = db.AppraisalRatingScaleDetail.Where(w => w.DataEntryStatus == 1 && w.Id == id)
                                                .Select(s => new { id = s.Id, ratingValue = s.RatingValue, maxRatingValue=s.AppraisalRatingScale.ScaleMaxValue }).FirstOrDefault();


            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalGoalRatingValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }
    }
}


