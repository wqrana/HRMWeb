
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
    public class PositionQuestionController : TimeAideWebControllers<PositionQuestion>
    {
        public ActionResult IndexByPosition(int? id)
        {
            var model = db.PositionQuestion.Where(w => w.PositionId == id);
            return PartialView("Index", model);
        }
        public ActionResult CreateEdit(int? id)
        {
            IEnumerable<SelectListItem> questionItemList = null;
            var positionQuestionList = db.PositionQuestion.Where(w => w.PositionId == id && w.DataEntryStatus == 1)
                                              .ToList();
            questionItemList = db.GetAll<ApplicantInterviewQuestion>(SessionHelper.SelectedClientId)
                                      .Where(w=>w.IsPositionSpecific==true)
                                      .Select(s => new SelectListItem
                                      {
                                          Text = s.QuestionName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.QuestionItemList = questionItemList;
            string[] TempData = positionQuestionList.Select(s => s.ApplicantInterviewQuestionId.ToString()).ToArray<string>();
            ViewBag.SelectedPositionQuestions = TempData;
            return PartialView("CreateEdit");
        }
        public ActionResult CreateAdhocQuestion()
        {
           
            return PartialView("CreateAdhocQuestion");
        }

        [HttpPost]
        public JsonResult CreateAdhocQuestion(ApplicantInterviewQuestion model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            using (var adhocQDBTrans = db.Database.BeginTransaction())
            {
                try
                {

                    db.ApplicantInterviewQuestion.Add(model);
                    db.SaveChanges();
                    if (model.Id > 0)
                    {
                        var positionQuestion = new PositionQuestion();
                        positionQuestion.PositionId = model.PositionId.Value;
                        positionQuestion.ApplicantInterviewQuestionId = model.Id;
                        db.PositionQuestion.Add(positionQuestion);
                        db.SaveChanges();
                    }
                    adhocQDBTrans.Commit();
                }
                catch (Exception ex)
                {
                    adhocQDBTrans.Rollback();
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedQuestionIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedQuestionsList = selectedQuestionIds.Split(',').ToList();
                List<PositionQuestion> questionAddList = new List<PositionQuestion>();
                List<PositionQuestion> questionRemoveList = new List<PositionQuestion>();
                var existingQuestionList = db.PositionQuestion.Where(w => w.PositionId == id).ToList();

                foreach (var questionItem in existingQuestionList)
                {
                    var RecCnt = selectedQuestionsList.Where(w => w == questionItem.ApplicantInterviewQuestionId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        questionRemoveList.Add(questionItem);
                    }

                }
                foreach (var selectedQuestionId in selectedQuestionsList)
                {
                    if (selectedQuestionId == "") continue;
                    int questionId = int.Parse(selectedQuestionId);
                    var recExists = existingQuestionList.Where(w => w.ApplicantInterviewQuestionId == questionId).Count();
                    if (recExists == 0)
                    {
                        questionAddList.Add(new PositionQuestion() { PositionId = id, ApplicantInterviewQuestionId = questionId });

                    }
                }

                db.PositionQuestion.RemoveRange(questionRemoveList);
                db.PositionQuestion.AddRange(questionAddList);

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