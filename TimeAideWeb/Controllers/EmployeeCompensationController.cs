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
    public class EmployeeCompensationController : TimeAideWebControllers<EmployeeCompensation>
    {
        public ActionResult ActiveCompensation(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("Index", EmployeeCompensationService.ActiveCompensation(id??0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCompensation).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult InActiveCompensation(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("InActiveCompensation", EmployeeCompensationService.InactiveCompensation(id ?? 0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeCompensation).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override void OnCreate(EmployeeCompensation model)
        {
            if (model != null)
            {
                Employment activeEmployment = EmploymentService.GetActiveEmployment(ViewBag.UserInformationId);
                if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue)
                    (model as EmployeeCompensation).StartDate = activeEmployment.OriginalHireDate.Value;
                model.Enabled = true;
            }
            base.OnCreate(model);
        }
        [HttpPost]
        public ActionResult AddEmployeeCompensation(EmployeeCompensation employeeCompensation)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeCompensation(employeeCompensation);
            }
            if (ModelState.IsValid)
            {
                db.EmployeeCompensation.Add(employeeCompensation);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(employeeCompensation);
            }
            return GetErrors();
        }
        private void ValidateEmployeeCompensation(EmployeeCompensation employeeCompensation)
        {
            var activeCompensation = EmployeeCompensationService.ActiveCompensation(employeeCompensation.UserInformationId ?? 0, employeeCompensation.CompanyCompensationId);
            if (activeCompensation != null && activeCompensation.Id!=employeeCompensation.Id && activeCompensation.StartDate.Date == employeeCompensation.StartDate.Date)
            {
                ModelState.AddModelError("CompanyCompensationId", "Same compensation cannot have same start date.");
            }
            Employment activeEmployment = EmploymentService.GetActiveEmployment(employeeCompensation.UserInformationId??0);
            if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue && employeeCompensation.StartDate < activeEmployment.OriginalHireDate.Value)
            {
                ModelState.AddModelError("StartDate", "Start Date cannot be prior to the employee hiring date.");
            }
        }
        [HttpPost]
        public ActionResult Edit(EmployeeCompensation employeeCompensation)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeCompensation(employeeCompensation);
            }
            if (ModelState.IsValid)
            {
                employeeCompensation.SetUpdated<EmployeeCompensation>();
                db.Entry(employeeCompensation).State = EntityState.Modified;
                db.SaveChanges();
                return Json(employeeCompensation);
            }
            return GetErrors();
        }
        public ActionResult CreateDefaults()
        {
            try
            {
                AllowAdd();
                ViewBag.CompanyCompensationList = db.GetAllByCompany<CompanyCompensation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
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
        public ActionResult CreateDefaults(EmployeeCompensation employeeCompensation)
        {
            //if (ModelState.IsValid)
            //{
            //    ValidateEmployeeCompensation(employeeCompensation);
            //}
            //var list = db.CompensationPeriodEntry.ToList();
            //if (list.Count > 0)
            //    employeeCompensation.PeriodEntryId = list.FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.EmployeeCompensation.Add(employeeCompensation);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(employeeCompensation);
            }
            return GetErrors();
        }
        public virtual ActionResult CompensationTimeline(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = EmployeeCompensationService.GetCompensation(3,id ?? 0);
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
