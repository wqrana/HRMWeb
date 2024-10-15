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
    public class AppraisalRatingScaleDetailController : TimeAideWebControllers<AppraisalRatingScaleDetail>
    {
                
        // GET: AppraisalRatingScaleDetail
        public ActionResult IndexByRatingScale(int? id)
        {
            ViewBag.AppraisalRatingScaleId = id;
            var scaleRecord=  db.AppraisalRatingScale.Where(w => w.Id == id).Select(s => new { ScaleName = s.ScaleName, MaxValue=s.ScaleMaxValue }).FirstOrDefault();
            if (scaleRecord != null)
            {
                ViewBag.ScaleName = scaleRecord.ScaleName;
                ViewBag.MaxValue = scaleRecord.MaxValue;
            }
            //ViewBag.SelectedScale = new { Id = id, ScaleName = scaleRecord.ScaleName, MaxValue = scaleRecord.MaxValue };
           var model = db.AppraisalRatingScaleDetail.Where(w => w.AppraisalRatingScaleId == (id??0)).OrderBy(o => o.RatingLevelId).ToList();
            return PartialView("Index",model);
        }
        public override ActionResult Create()
        {
            ViewBag.RatingLevelId = new SelectList(db.RatingLevel, "Id", "RatingLevelName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.AppraisalRatingScaleDetail.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.RatingLevelId = new SelectList(db.RatingLevel, "Id", "RatingLevelName", model.RatingLevelId);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateEdit(AppraisalRatingScaleDetail model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            AppraisalRatingScaleDetail appraisalRatingScaleEntity = null;
            try
            {
                var isAlreadyExist = db.AppraisalRatingScaleDetail
                                        .Where(w => w.DataEntryStatus==1 && (w.AppraisalRatingScaleId== model.AppraisalRatingScaleId)
                                               && (w.Id != model.Id) && 
                                               ((w.RatingValue == model.RatingValue)||(w.RatingName.ToLower() == model.RatingName.ToLower())) )
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Scale Value/Name is already added";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        appraisalRatingScaleEntity = new AppraisalRatingScaleDetail();
                        appraisalRatingScaleEntity.AppraisalRatingScaleId = model.AppraisalRatingScaleId;
                        db.AppraisalRatingScaleDetail.Add(appraisalRatingScaleEntity);
                    }
                    else
                    {
                        appraisalRatingScaleEntity = db.AppraisalRatingScaleDetail.Find(model.Id);
                        appraisalRatingScaleEntity.ModifiedBy = SessionHelper.LoginId;
                        appraisalRatingScaleEntity.ModifiedDate = DateTime.Now;
                    }
                    appraisalRatingScaleEntity.RatingLevelId = model.RatingLevelId;
                    appraisalRatingScaleEntity.RatingValue = model.RatingValue;
                    appraisalRatingScaleEntity.RatingName = model.RatingName;
                    appraisalRatingScaleEntity.RatingDescription = model.RatingDescription;
                    appraisalRatingScaleEntity.RatingAbbreviation = model.RatingAbbreviation;
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
            var appraisalRatingScaleEntity = db.AppraisalRatingScaleDetail.Find(id);
            try
            {
                appraisalRatingScaleEntity.ModifiedBy = SessionHelper.LoginId;
                appraisalRatingScaleEntity.ModifiedDate = DateTime.Now;
                appraisalRatingScaleEntity.DataEntryStatus = 0;
                db.SaveChanges();
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