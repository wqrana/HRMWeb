
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
    public class AppraisalGoalController : TimeAideWebControllers<AppraisalGoal>
    {
        public override ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.IsBaseCompanyObject = true;
                ViewBag.IsAllCompanies = true;
                ViewBag.CanBeAssignedToCurrentCompany = true;
                ViewBag.Label = ViewBag.Label + " - Add";
                var ratingScaleList = db.GetAllByCompany<AppraisalRatingScale>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w=>w.CompanyId==null);
                ViewBag.AppraisalRatingScaleId = new SelectList(ratingScaleList, "Id", "ScaleName");                                          
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AppraisalGoal", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public override ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();    
                
                var model = db.Find<AppraisalGoal>(id.Value, SessionHelper.SelectedClientId);
                var ratingScaleList = db.GetAllByCompany<AppraisalRatingScale>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                      .Where(w => model.CompanyId == null?(w.CompanyId==null):true);
                ViewBag.AppraisalRatingScaleId = new SelectList(ratingScaleList, "Id", "ScaleName",model.AppraisalRatingScaleId);
                ViewBag.Label = ViewBag.Label + " - Edit";
                ViewBag.IsBaseCompanyObject = true;
                ViewBag.IsAllCompanies = false;
                ViewBag.CanBeAssignedToCurrentCompany = false;
                
                    if (!model.CompanyId.HasValue)
                    {
                        ViewBag.IsAllCompanies = true;
                    }
                    var companies = model.GetRefferredCompanies();
                    if (companies.Count == 0) //|| (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
                    {
                        ViewBag.CanBeAssignedToCurrentCompany = true;
                    }
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
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(AppraisalGoal model)
        {
            if (ModelState.IsValid)
            {
                db.AppraisalGoal.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            //return PartialView(model);
            return GetErrors();
        }
       
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(AppraisalGoal model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(model);
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.AppraisalGoal.Include(u => u.AppraisalTemplateGoal)
            //                             .Include(u => u.EmployeeAppraisalGoal)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.AppraisalTemplateGoal.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmployeeAppraisalGoal.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

