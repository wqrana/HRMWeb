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
    public class PositionAppraisalTemplateController : TimeAideWebControllers<PositionAppraisalTemplate>
    {
        public ActionResult IndexByPosition(int? id)
        {
            var model = db.PositionAppraisalTemplate.Where(w => w.PositionId == id);
            return PartialView("Index", model);
        }
        [HttpGet]
        public ActionResult CreateEdit(int? id)
        {
            var position = db.Position.Find(id);
            var companyId = position.CompanyId ?? 0;
            IEnumerable<SelectListItem> appraisalTemplateItemList = null;
            var positionAppraisalTemplateList = db.PositionAppraisalTemplate.Where(w => w.PositionId == id && w.DataEntryStatus == 1)
                                              .ToList();
            appraisalTemplateItemList = db.GetAllByCompany<AppraisalTemplate>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).
                                       Where(w => companyId == 0 ? ((w.CompanyId ?? 0) == companyId) : true)
                                       .Select(s => new SelectListItem
                                      {
                                          Text = s.TemplateName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.AppraisalTemplateItemList = appraisalTemplateItemList;
            string[] TempData = positionAppraisalTemplateList.Select(s => s.AppraisalTemplateId.ToString()).ToArray<string>();
            ViewBag.SelectedPositionAppraisalTemplates = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedAppraisalTemplateIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedAppraisalTemplatesList = selectedAppraisalTemplateIds.Split(',').ToList();
                List<PositionAppraisalTemplate> appraisalTemplateAddList = new List<PositionAppraisalTemplate>();
                List<PositionAppraisalTemplate> appraisalTemplateRemoveList = new List<PositionAppraisalTemplate>();
                var existingAppraisalTemplateList = db.PositionAppraisalTemplate.Where(w => w.PositionId == id).ToList();

                foreach (var appraisalTemplateItem in existingAppraisalTemplateList)
                {
                    var RecCnt = selectedAppraisalTemplatesList.Where(w => w == appraisalTemplateItem.AppraisalTemplateId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        appraisalTemplateRemoveList.Add(appraisalTemplateItem);
                    }

                }
                foreach (var selectedAppraisalTemplateId in selectedAppraisalTemplatesList)
                {
                    if (selectedAppraisalTemplateId == "") continue;
                    int appraisalTemplateId = int.Parse(selectedAppraisalTemplateId);
                    var recExists = existingAppraisalTemplateList.Where(w => w.AppraisalTemplateId == appraisalTemplateId).Count();
                    if (recExists == 0)
                    {
                        appraisalTemplateAddList.Add(new PositionAppraisalTemplate() { PositionId = id, AppraisalTemplateId = appraisalTemplateId });

                    }
                }

                db.PositionAppraisalTemplate.RemoveRange(appraisalTemplateRemoveList);
                db.PositionAppraisalTemplate.AddRange(appraisalTemplateAddList);

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