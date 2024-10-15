
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
    public class ApplicantInterviewController : BaseApplicantRoleRightsController<ApplicantInterview>
    {


        [HttpGet]
        // GET: 
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantInterview.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id).OrderBy(o => o.Id);
            return PartialView("Index", model);
        }


        public ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.ApplicantInterviewQuestionId = new SelectList(db.GetAll<ApplicantInterviewQuestion>(SessionHelper.SelectedClientId), "Id", "QuestionName");
                ViewBag.ApplicantInterviewAnswerId = new SelectList(db.GetAll<ApplicantInterviewAnswer>(SessionHelper.SelectedClientId), "Id", "AnswerName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInterview", "Create");
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
                var model = db.ApplicantInterview.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.ApplicantInterviewQuestionId = new SelectList(db.GetAll<ApplicantInterviewQuestion>(SessionHelper.SelectedClientId), "Id", "QuestionName",model.ApplicantInterviewQuestionId);
                ViewBag.ApplicantInterviewAnswerId = new SelectList(db.GetAll<ApplicantInterviewAnswer>(SessionHelper.SelectedClientId), "Id", "AnswerName",model.ApplicantInterviewAnswerId);
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

        public JsonResult AjaxGetInterviewAnswerRating(int id)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            dynamic answerRating = null;

            if (id > 0)
            {
                try
                {
                    answerRating = db.ApplicantInterviewAnswer.Where(w => w.Id == id)
                                    .Select(s => new {AnwserValue=s.AnswerValue, AnswerMaxValue=s.AnswerMaxValue })
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
            retResult = new { status = status, message = message, answerRating = answerRating };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateEdit(ApplicantInterview model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantInterview applicantInterviewEntity = null;
            try
            {
                var IsExistingQuestion = db.ApplicantInterview.Where(w => w.ApplicantInformationId == model.ApplicantInformationId
                                                   && w.ApplicantInterviewQuestionId == model.ApplicantInterviewQuestionId
                                                   && w.DataEntryStatus == 1 && w.Id!=model.Id).Count();
                if (IsExistingQuestion > 0)
                {
                    throw new Exception("Same question is already added!");
                }
                if (model.Id == 0)
                {
                 
                    applicantInterviewEntity = new ApplicantInterview();
                    applicantInterviewEntity.ApplicantInformationId = model.ApplicantInformationId;
                    db.ApplicantInterview.Add(applicantInterviewEntity);
                }
                else
                {
                    applicantInterviewEntity = db.ApplicantInterview.Find(model.Id);
                    applicantInterviewEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantInterviewEntity.ModifiedDate = DateTime.Now;
                }
                applicantInterviewEntity.ApplicantInterviewQuestionId = model.ApplicantInterviewQuestionId;
                applicantInterviewEntity.ApplicantAnswer = model.ApplicantAnswer;
                applicantInterviewEntity.ApplicantInterviewAnswerId = model.ApplicantInterviewAnswerId;
                applicantInterviewEntity.InterviewAnswerValue = model.InterviewAnswerValue;
                applicantInterviewEntity.InterviewAnswerMaxValue = model.InterviewAnswerMaxValue;
                applicantInterviewEntity.Note = model.Note;

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
                var model = db.ApplicantInterview.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInterview", "Delete");
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
            var applicantInterviewQAEntity = db.ApplicantInterview.Find(id);
            try
            {
                applicantInterviewQAEntity.ModifiedBy = SessionHelper.LoginId;
                applicantInterviewQAEntity.ModifiedDate = DateTime.Now;
                applicantInterviewQAEntity.DataEntryStatus = 0;
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