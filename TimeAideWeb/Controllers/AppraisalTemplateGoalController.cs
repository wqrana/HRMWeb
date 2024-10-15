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
    public class AppraisalTemplateGoalController : TimeAideWebControllers<AppraisalTemplateGoal>
    {
        public ActionResult IndexByTemplate(int? id)
        {
            var model = db.AppraisalTemplateGoal.Where(w => w.AppraisalTemplateId == id);
            return PartialView("Index", model);
        }
        public  ActionResult CreateEdit(int? id)
        {
            var appraisalTemplate = db.AppraisalTemplate.Find(id);
            var companyId = appraisalTemplate.CompanyId??0;
            IEnumerable<SelectListItem> appraisalGoalItemList = null;
            var templateGoalList = db.AppraisalTemplateGoal.Where(w => w.AppraisalTemplateId == id && w.DataEntryStatus == 1)
                                              .ToList();
            appraisalGoalItemList = db.GetAllByCompany<AppraisalGoal>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).
                                      Where(w=> companyId==0?((w.CompanyId??0)== companyId):true)
                                      .Select(s => new SelectListItem
                                      {
                                          Text = s.GoalName,
                                          Value = s.Id.ToString()

                                      });
           
            ViewBag.AppraisalGoalItemList = appraisalGoalItemList;
            string[] TempData = templateGoalList.Select(s => s.AppraisalGoalId.ToString()).ToArray<string>();
            ViewBag.SelectedTemplateGoals = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedGoalIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedGoalsList = selectedGoalIds.Split(',').ToList();
                List<AppraisalTemplateGoal> goalAddList = new List<AppraisalTemplateGoal>();
                List<AppraisalTemplateGoal> goalRemoveList = new List<AppraisalTemplateGoal>();
                var existingGoalList = db.AppraisalTemplateGoal.Where(w => w.AppraisalTemplateId == id).ToList();

                foreach (var goalItem in existingGoalList)
                {
                    var RecCnt = selectedGoalsList.Where(w => w == goalItem.AppraisalGoalId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        goalRemoveList.Add(goalItem);
                    }

                }
                foreach (var selectedGoalId in selectedGoalsList)
                {
                    if (selectedGoalId == "") continue;
                    int goalId = int.Parse(selectedGoalId);
                    var recExists = existingGoalList.Where(w => w.AppraisalGoalId == goalId).Count();
                    if (recExists == 0)
                    {
                        goalAddList.Add(new AppraisalTemplateGoal() { AppraisalTemplateId = id, AppraisalGoalId = goalId});

                    }
                }

                db.AppraisalTemplateGoal.RemoveRange(goalRemoveList);
                db.AppraisalTemplateGoal.AddRange(goalAddList);

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