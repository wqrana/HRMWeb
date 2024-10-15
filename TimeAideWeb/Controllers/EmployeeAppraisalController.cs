
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
    public class EmployeeAppraisalController : TimeAideWebControllers<EmployeeAppraisal>
    {

        //public ActionResult IndexByUser()
        //{
        //    try
        //    {
        //        AllowView();
        //        var entitySet = db.GetAll<AppraisalTemplate>();
        //        return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
        //    }
        //    catch (AuthorizationException ex)
        //    {
        //        Exception exception = new Exception(ex.ErrorMessage);
        //        HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AppraisalTemplate).Name, "Index");
        //        return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
        //    }

        //}

        public override ActionResult MostRecentRecord(int? id)
        {
            try
            {
                EmployeeAppraisal model = null;
                AllowView();                
                 model = db.EmployeeAppraisal.Where(w => w.DataEntryStatus == 1 && w.UserInformationId==id).OrderByDescending(o => o.Id).FirstOrDefault();
                 if (model == null) model = new EmployeeAppraisal();

                 return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "UserInformation", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public  ActionResult CreateByUser(int id)
        {
            try
            {
                int? companyId=null, departmentId = null, positionId = null;
                AllowAdd();
                var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                if (employmentHistoryEntity != null)
                {
                    companyId = employmentHistoryEntity.CompanyId;
                    departmentId = employmentHistoryEntity.DepartmentId;
                    positionId = employmentHistoryEntity.PositionId;
                }
                //var payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                //if (payInfoHistoryEntity != null)
                //{
                //    positionId = payInfoHistoryEntity.PositionId;
                //}
                companyId= companyId ?? SessionHelper.SelectedCompanyId;
                ViewBag.EmployeeAppraisalCompanyID = new SelectList(db.GetAll<Company>(SessionHelper.SelectedClientId), "Id", "CompanyName", companyId);
                ViewBag.EmployeeAppraisalDepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "DepartmentName", departmentId);
                ViewBag.EmployeeAppraisalPositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedClientId, SessionHelper.SelectedClientId), "Id", "PositionName", positionId);
                ViewBag.EmployeeAppraisalTemplateId = new SelectList(db.GetAll<AppraisalTemplate>(SessionHelper.SelectedClientId), "Id", "TemplateName");
                var model = new EmployeeAppraisal();
                return PartialView("Create",model);
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
                var model = db.EmployeeAppraisal.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.EmployeeAppraisalCompanyID = new SelectList(db.GetAll<Company>(SessionHelper.SelectedClientId), "Id", "CompanyName", model.CompanyId);
                ViewBag.EmployeeAppraisalDepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "DepartmentName", model.DepartmentId);
                ViewBag.EmployeeAppraisalPositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName", model.PositionId);
                ViewBag.EmployeeAppraisalTemplateId = new SelectList(db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "TemplateName",model.AppraisalTemplateId);
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
        public JsonResult CreateEdit(EmployeeAppraisal model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAppraisal employeeAppraisalEntity = null;
            try
            {
               
                    if (model.Id == 0) { 
                    
                        employeeAppraisalEntity = new EmployeeAppraisal();
                        employeeAppraisalEntity.UserInformationId = model.UserInformationId;
                        db.EmployeeAppraisal.Add(employeeAppraisalEntity);
                    }
                    else
                    {
                        employeeAppraisalEntity = db.EmployeeAppraisal.Find(model.Id);
                        employeeAppraisalEntity.ModifiedBy = SessionHelper.LoginId;
                        employeeAppraisalEntity.ModifiedDate = DateTime.Now;
                    }
                employeeAppraisalEntity.AppraisalReviewDate = model.AppraisalReviewDate;
                employeeAppraisalEntity.AppraisalDueDate = model.AppraisalDueDate;
                employeeAppraisalEntity.NextAppraisalDueDate = model.NextAppraisalDueDate;
                employeeAppraisalEntity.AppraisalTemplateId = model.AppraisalTemplateId;
                employeeAppraisalEntity.EvaluationStartDate = model.EvaluationStartDate;
                employeeAppraisalEntity.EvaluationEndDate = model.EvaluationEndDate;
                employeeAppraisalEntity.PositionId = model.PositionId;
                employeeAppraisalEntity.CompanyId = model.CompanyId;
                employeeAppraisalEntity.DepartmentId = model.DepartmentId;

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
            var employeeAppraisalEntity = db.EmployeeAppraisal.Find(id);
            try
            {
                employeeAppraisalEntity.ModifiedBy = SessionHelper.LoginId;
                employeeAppraisalEntity.ModifiedDate = DateTime.Now;
                employeeAppraisalEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult AppraisalTemplateSelectionPopUp(int? id)
        {
            string positionName = "";
            var appraisalTemplateSelectionList = new SelectList(db.AppraisalTemplate.Where(w => w.Id == -1), "Id", "TemplateName");
                     
            if (id != null)
            {
                var positionAppraisalTemplateList = db.PositionAppraisalTemplate.Where(w => w.DataEntryStatus == 1 && w.PositionId == id).Select(s => s.AppraisalTemplate);
                appraisalTemplateSelectionList = new SelectList(positionAppraisalTemplateList.Where(w => w.DataEntryStatus == 1), "Id", "TemplateName");
                positionName = db.Position.Where(w=>w.Id==id).Select(s=>s.PositionName).FirstOrDefault();
            }
            ViewBag.PositionName = (positionName == "" || positionName == null) ? "Not Available" : positionName;
            ViewBag.AppraisalTemplateSelectionId = appraisalTemplateSelectionList;
           
            return PartialView("TemplateSelectionPopUp");
        }
        public JsonResult AjaxGetAppraisalTemplateList(int? id, bool isAllMasterData)
        {
            dynamic appraisalTemplateList = db.AppraisalTemplate.Where(w => w.Id == -1).Select(s => new { id = s.Id, text = s.TemplateName });
            if (isAllMasterData == false)
            {
                if (id != null)
                {
                    var positionAppraisalTemplateList = db.PositionAppraisalTemplate.Where(w => w.DataEntryStatus == 1 && w.PositionId == id).Select(s => s.AppraisalTemplate);
                    appraisalTemplateList = positionAppraisalTemplateList.Select(s => new { id = s.Id, text = s.TemplateName });
                }
            }
            else
            {
                appraisalTemplateList = db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).
                                            Select(s => new { id = s.Id, text = s.TemplateName });
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = appraisalTemplateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;

        }
        public ActionResult DisplayResult(int? id)
        {
            try
            {
              
                var employeeAppraisalEntity = db.EmployeeAppraisal.Where(w => w.Id == id).FirstOrDefault();
                var appraisalResult = CalculateAppraisalResult(id);
                employeeAppraisalEntity.AppraisalOverallScore = appraisalResult.score;
                employeeAppraisalEntity.AppraisalTotalMaxValue = appraisalResult.maxScore;
                employeeAppraisalEntity.AppraisalOverallPct = appraisalResult.pct;
                db.SaveChanges();
               
                return PartialView(employeeAppraisalEntity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeAppraisal", "DisplayResult");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeAppraisal", "DisplayResult");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
               
            }
        }
        public  ActionResult EditResult(int? id)
        {
            try
            {
                AllowEdit();

                var model = db.EmployeeAppraisal.Where(w => w.Id == id).FirstOrDefault();
                var appraisalResult = CalculateAppraisalResult(id);
                model.AppraisalOverallScore = appraisalResult.score;
                model.AppraisalTotalMaxValue = appraisalResult.maxScore;
                model.AppraisalOverallPct = appraisalResult.pct;

                ViewBag.AppraisalResultId = new SelectList(db.GetAllByCompany<AppraisalResult>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "ResultName", model.AppraisalResultId);
                
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        private dynamic CalculateAppraisalResult(int? employeeAppraisalId)
        {
           decimal goalScore, goalMaxScore;
           decimal skillScore, skillMaxScore;
           decimal overallPct, overallScore, overallMaxScore;
           var appraisalGoalScore=
                db.EmployeeAppraisalGoal.Where(w => w.DataEntryStatus == 1 && w.EmployeeAppraisalId == employeeAppraisalId)
                .Select(s => new { GoalScore = s.GoalRatingValue, MaxGoalScore = s.GoalScaleMaxValue }).ToList();

            var appraisalSkillScore =
                db.EmployeeAppraisalSkill.Where(w => w.DataEntryStatus == 1 && w.EmployeeAppraisalId == employeeAppraisalId)
                .Select(s => new { SkillScore = s.SkillRatingValue, MaxSkillScore = s.SkillScaleMaxValue }).ToList();

            goalScore = appraisalGoalScore.Select(s => s.GoalScore).Sum();
            goalMaxScore = appraisalGoalScore.Select(s => s.MaxGoalScore).Sum();

            skillScore = appraisalSkillScore.Select(s => s.SkillScore).Sum();
            skillMaxScore = appraisalSkillScore.Select(s => s.MaxSkillScore).Sum();

            overallScore = goalScore + skillScore;
            overallMaxScore = goalMaxScore + skillMaxScore;
            overallPct = Math.Round((overallScore / overallMaxScore)*100, 2);
            return new {score = overallScore, maxScore= overallMaxScore,pct= overallPct };

        }
        [HttpPost]
        public JsonResult EditResult(EmployeeAppraisal model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAppraisal employeeAppraisalEntity = null;
            try
            {

                if (model.Id != 0)
                {
                    employeeAppraisalEntity = db.EmployeeAppraisal.Find(model.Id);
                    employeeAppraisalEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeAppraisalEntity.ModifiedDate = DateTime.Now;
                    employeeAppraisalEntity.AppraisalResultId = model.AppraisalResultId;
                    employeeAppraisalEntity.AppraisalOverallScore = model.AppraisalOverallScore;
                    employeeAppraisalEntity.AppraisalTotalMaxValue = model.AppraisalTotalMaxValue;
                    employeeAppraisalEntity.AppraisalOverallPct = model.AppraisalOverallPct;
                    employeeAppraisalEntity.AppraisalReviewerComments = model.AppraisalReviewerComments;
                    employeeAppraisalEntity.AppraisalEmployeeComments = model.AppraisalEmployeeComments;
                                   

                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

    }
}


