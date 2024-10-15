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
    public class AppraisalRatingScaleController : TimeAideWebControllers<AppraisalRatingScale>
    {
        // GET: AppraisalRatingScale

        public override ActionResult Create()
        {
            ViewBag.IsBaseCompanyObject = true;
            ViewBag.IsAllCompanies = true;
            ViewBag.CanBeAssignedToCurrentCompany = true;
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.AppraisalRatingScale.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.CanBeAssignedToCurrentCompany = false;

            if (!model.CompanyId.HasValue)
            {
                model.IsAllCompanies = true;
            }
            var companies = model.GetRefferredCompanies();
            if (companies.Count == 0)//|| (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
            {
                ViewBag.CanBeAssignedToCurrentCompany = true;
            }
            return PartialView(model);
        }
        public JsonResult AjaxGetRatingScale(bool IsAllCompanies)
        {
            var ratingScaleList = db.GetAllByCompany<AppraisalRatingScale>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId)
                                .Where(w => IsAllCompanies ?(w.CompanyId==null):true)
                                .Select(s => new { id = s.Id, name = s.ScaleName }).ToList();

            JsonResult jsonResult = new JsonResult()
            {
                Data = ratingScaleList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
            //return Json(cityList);
        }
        [HttpPost]
        public JsonResult CreateEdit(AppraisalRatingScale model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            AppraisalRatingScale appraisalRatingScaleEntity = null;
            try
            {
                var isAlreadyExist = db.AppraisalRatingScale
                                        .Where( w => w.DataEntryStatus==1 && (w.Id!= model.Id) && (w.ScaleName.ToLower() == model.ScaleName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Scale Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        appraisalRatingScaleEntity = new AppraisalRatingScale();
                        db.AppraisalRatingScale.Add(appraisalRatingScaleEntity);
                    }
                    else
                    {
                        appraisalRatingScaleEntity = db.AppraisalRatingScale.Find(model.Id);
                        appraisalRatingScaleEntity.ModifiedBy = SessionHelper.LoginId;
                        appraisalRatingScaleEntity.ModifiedDate = DateTime.Now;
                    }
                    appraisalRatingScaleEntity.ScaleName = model.ScaleName;
                    appraisalRatingScaleEntity.ScaleDescription = model.ScaleDescription;
                    appraisalRatingScaleEntity.ScaleMaxValue = model.ScaleMaxValue;
                    appraisalRatingScaleEntity.CompanyId = model.IsAllCompanies ? null : (int?)SessionHelper.SelectedCompanyId;
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
            var appraisalRatingScaleEntity = db.AppraisalRatingScale.Find(id);
            try
            {
                appraisalRatingScaleEntity.ModifiedBy = SessionHelper.LoginId;
                appraisalRatingScaleEntity.ModifiedDate = DateTime.Now;
                appraisalRatingScaleEntity.DataEntryStatus = 0;
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