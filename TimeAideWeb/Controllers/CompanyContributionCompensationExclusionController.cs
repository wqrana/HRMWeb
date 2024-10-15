﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Controllers
{
    public class CompanyContributionCompensationExclusionController : TimeAideWebControllers<CompanyContributionCompensationExclusion>
    {
        public ActionResult CreateCompensationExclusion()
        {
            ViewBag.CompanyCompensation = db.GetAll<CompanyCompensation>(SessionHelper.SelectedClientId);
            return PartialView();
        }
        public ActionResult CreateEdit(int? companyContributionId)
        {
            ViewBag.CompanyContributionId = companyContributionId;
            ViewBag.SelectedCompensationTransactions = db.GetAll<CompanyContributionCompensationExclusion>(SessionHelper.SelectedClientId).Where(e => e.CompanyContributionId == companyContributionId);
            ViewBag.CompensationTransactions = db.GetAll<CompanyCompensation>(SessionHelper.SelectedClientId).Where(c => !c.CompanyContributionCompensationExclusion.Any(w => w.CompanyContributionId == companyContributionId));
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }
        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                 CompanyContributionCompensationExclusionService.UpdateSelectedList(id, selectedIds);

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public virtual ActionResult CreateIndex(string id)
        {
            try
            {
                List<int> selectedCompensationList = new List<int>();
                if (!String.IsNullOrEmpty(id))
                {
                    foreach (string each in id.Split(','))
                    {
                        int compensationId;
                        if (int.TryParse(each, out compensationId))
                            selectedCompensationList.Add(compensationId);
                    }

                }
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.CompanyCompensation.Where(u => selectedCompensationList.Contains(u.Id) && u.DataEntryStatus == 1).ToList();
                return PartialView(supervisors);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult IndexCompensationExclusion(int? id)
        {
            try
            {
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.CompanyContributionCompensationExclusion.Where(u => u.CompanyContributionId == id && u.DataEntryStatus == 1).ToList();
                return PartialView(supervisors);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
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
