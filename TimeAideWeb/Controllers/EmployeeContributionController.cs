using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;

namespace WebApplication4.Controllers
{
    public class EmployeeContributionController : TimeAideWebControllers<EmployeeContribution>
    {
        public ActionResult ActiveContribution(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("Index", EmployeeContributionService.ActiveContribution(id??0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeContribution).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult InActiveContribution(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("InActiveContribution", EmployeeContributionService.InactiveContribution(id ?? 0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeContribution).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override void OnCreate(EmployeeContribution model)
        {
            if (model != null)
            {
                Employment activeEmployment = EmploymentService.GetActiveEmployment(ViewBag.UserInformationId);
                if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue)
                    (model as EmployeeContribution).StartDate = activeEmployment.OriginalHireDate.Value;
                model.Enabled = true;
            }
            base.OnCreate(model);
        }
        [HttpPost]
        public ActionResult AddEmployeeContribution(EmployeeContribution EmployeeContribution)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeContribution(EmployeeContribution);
            }
            if (ModelState.IsValid)
            {
                db.EmployeeContribution.Add(EmployeeContribution);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(EmployeeContribution);
            }
            return GetErrors();
        }
        private void ValidateEmployeeContribution(EmployeeContribution EmployeeContribution)
        {
            var activeContribution = EmployeeContributionService.ActiveContribution(EmployeeContribution.UserInformationId ?? 0, EmployeeContribution.CompanyContributionId);
            if (activeContribution != null && activeContribution.Id!=EmployeeContribution.Id && activeContribution.StartDate.Date == EmployeeContribution.StartDate.Date)
            {
                ModelState.AddModelError("CompanyContributionId", "Same compensation cannot have same start date.");
            }
            Employment activeEmployment = EmploymentService.GetActiveEmployment(EmployeeContribution.UserInformationId??0);
            if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue && EmployeeContribution.StartDate < activeEmployment.OriginalHireDate.Value)
            {
                ModelState.AddModelError("StartDate", "Start Date cannot be prior to the employee hiring date.");
            }
        }
        [HttpPost]
        public ActionResult Edit(EmployeeContribution EmployeeContribution)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeContribution(EmployeeContribution);
            }
            if (ModelState.IsValid)
            {
                EmployeeContribution.SetUpdated<EmployeeContribution>();
                db.Entry(EmployeeContribution).State = EntityState.Modified;
                db.SaveChanges();
                return Json(EmployeeContribution);
            }
            return GetErrors();
        }
        public ActionResult CreateDefaults()
        {
            try
            {
                AllowAdd();
                ViewBag.CompanyContributionList = db.GetAllByCompany<CompanyContribution>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "CreateAccrualRule");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public ActionResult CreateDefaults(EmployeeContribution EmployeeContribution)
        {
            //if (ModelState.IsValid)
            //{
            //    ValidateEmployeeContribution(EmployeeContribution);
            //}
            //var list = db.ContributionPeriodEntry.ToList();
            //if (list.Count > 0)
            //    EmployeeContribution.PeriodEntryId = list.FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.EmployeeContribution.Add(EmployeeContribution);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(EmployeeContribution);
            }
            return GetErrors();
        }
        public virtual ActionResult ContributionTimeline(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = EmployeeContributionService.GetContribution(3,id ?? 0);
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
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
