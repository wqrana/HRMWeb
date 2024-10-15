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
    public class AppraisalTemplateSkillController : TimeAideWebControllers<AppraisalTemplateSkill>
    {
        public ActionResult IndexByTemplate(int? id)
        {
            var model = db.AppraisalTemplateSkill.Where(w => w.AppraisalTemplateId == id);
            return PartialView("Index", model);
        }
        public ActionResult CreateEdit(int? id)
        {
            var appraisalTemplate = db.AppraisalTemplate.Find(id);
            var companyId = appraisalTemplate.CompanyId ?? 0;
            IEnumerable<SelectListItem> appraisalSkillItemList = null;
            var templateSkillList = db.AppraisalTemplateSkill.Where(w => w.AppraisalTemplateId == id && w.DataEntryStatus == 1)
                                              .ToList();
            appraisalSkillItemList = db.GetAllByCompany<AppraisalSkill>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).
                                       Where(w => companyId == 0 ? ((w.CompanyId ?? 0) == companyId) : true)
                                       .Select(s => new SelectListItem
                                      {
                                          Text = s.SkillName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.AppraisalSkillItemList = appraisalSkillItemList;
            string[] TempData = templateSkillList.Select(s => s.AppraisalSkillId.ToString()).ToArray<string>();
            ViewBag.SelectedTemplateSkills = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedSkillIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedSkillsList = selectedSkillIds.Split(',').ToList();
                List<AppraisalTemplateSkill> skillAddList = new List<AppraisalTemplateSkill>();
                List<AppraisalTemplateSkill> skillRemoveList = new List<AppraisalTemplateSkill>();
                var existingSkillList = db.AppraisalTemplateSkill.Where(w => w.AppraisalTemplateId == id).ToList();

                foreach (var skillItem in existingSkillList)
                {
                    var RecCnt = selectedSkillsList.Where(w => w == skillItem.AppraisalSkillId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        skillRemoveList.Add(skillItem);
                    }

                }
                foreach (var selectedSkillId in selectedSkillsList)
                {
                    if (selectedSkillId == "") continue;
                    int skillId = int.Parse(selectedSkillId);
                    var recExists = existingSkillList.Where(w => w.AppraisalSkillId == skillId).Count();
                    if (recExists == 0)
                    {
                        skillAddList.Add(new AppraisalTemplateSkill() { AppraisalTemplateId = id, AppraisalSkillId = skillId });

                    }
                }

                db.AppraisalTemplateSkill.RemoveRange(skillRemoveList);
                db.AppraisalTemplateSkill.AddRange(skillAddList);

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