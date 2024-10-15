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
    public class EmployeeWithholdingController : TimeAideWebControllers<EmployeeWithholding>
    {
        public ActionResult ActiveEmployeeWithholding(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("Index", EmployeeWithholdingService.ActiveEmployeeWithholding(id??0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeWithholding).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult InActiveEmployeeWithholding(int? id)
        {
            try
            {
                AllowView();
                ViewBag.UserInformationId = id;
                return PartialView("InActiveEmployeeWithholding", EmployeeWithholdingService.InactiveEmployeeWithholding(id ?? 0));
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeWithholding).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override void OnCreate(EmployeeWithholding model)
        {
            if (model != null)
            {
                Employment activeEmployment = EmploymentService.GetActiveEmployment(ViewBag.UserInformationId);
                if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue)
                    model.StartDate = activeEmployment.OriginalHireDate.Value;
                ViewBag.CompanyWithholdings = db.GetAllByCompany<CompanyWithholding>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToDictionary(w => w.Id,w=> w.IsLoan);
            }
            base.OnCreate(model);
        }
        public override EmployeeWithholding OnEdit(EmployeeWithholding model)
        {
            if (model != null)
            {
                //Employment activeEmployment = EmploymentService.GetActiveEmployment(ViewBag.UserInformationId);
                //if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue)
                //    model.StartDate = activeEmployment.OriginalHireDate.Value;
                ViewBag.CompanyWithholdings = db.GetAllByCompany<CompanyWithholding>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToDictionary(w => w.Id, w => w.IsLoan);
            }
            return base.OnEdit(model);
        }
        [HttpPost]
        public ActionResult AddEmployeeWithholding(EmployeeWithholding EmployeeWithholding)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeWithholding(EmployeeWithholding);
            }
            if (ModelState.IsValid)
            {
                if (EmployeeWithholding.Id == 0)
                {
                    db.EmployeeWithholding.Add(EmployeeWithholding);
                }
                else
                {
                    EmployeeWithholding.SetUpdated<EmployeeWithholding>();
                    db.Entry(EmployeeWithholding).State = EntityState.Modified;
                }
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(EmployeeWithholding);
            }
            return GetErrors();
        }
        private void ValidateEmployeeWithholding(EmployeeWithholding employeeWithholding)
        {
            var activeWithholding = EmployeeWithholdingService.ActiveEmployeeWithholding(employeeWithholding.UserInformationId ?? 0, employeeWithholding.CompanyWithholdingId);
            if (activeWithholding != null && activeWithholding.Id!=employeeWithholding.Id && activeWithholding.StartDate.Date == employeeWithholding.StartDate.Date)
            {
                ModelState.AddModelError("CompanyWithholdingId", "Same Withholding cannot have same start date.");
            }
            Employment activeEmployment = EmploymentService.GetActiveEmployment(employeeWithholding.UserInformationId??0);
            if (activeEmployment != null && activeEmployment.OriginalHireDate.HasValue && employeeWithholding.StartDate < activeEmployment.OriginalHireDate.Value)
            {
                ModelState.AddModelError("StartDate", "Start Date cannot be prior to the employee hiring date.");
            }
        }
        [HttpPost]
        public ActionResult Edit(EmployeeWithholding employeeWithholding)
        {
            if (ModelState.IsValid)
            {
                ValidateEmployeeWithholding(employeeWithholding);
            }
            if (ModelState.IsValid)
            {
                employeeWithholding.SetUpdated<EmployeeWithholding>();
                db.Entry(employeeWithholding).State = EntityState.Modified;
                db.SaveChanges();
                return Json(employeeWithholding);
            }
            return GetErrors();
        }
        public ActionResult CreateDefaults()
        {
            try
            {
                AllowAdd();
                ViewBag.CompanyWithholdingList = db.GetAllByCompany<CompanyWithholding>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
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
        public ActionResult CreateDefaults(EmployeeWithholding employeeWithholding)
        {
            //if (ModelState.IsValid)
            //{
            //    ValidateEmployeeWithholding(employeeWithholding);
            //}
            //var list = db.WithholdingPeriodEntry.ToList();
            //if (list.Count > 0)
            //    employeeWithholding.PeriodEntryId = list.FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                db.EmployeeWithholding.Add(employeeWithholding);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(employeeWithholding);
            }
            return GetErrors();
        }
        public virtual ActionResult WithholdingTimeline(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var model = EmployeeWithholdingService.GetWithholding(3,id ?? 0);
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
