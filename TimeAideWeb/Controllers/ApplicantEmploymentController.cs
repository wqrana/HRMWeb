using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ApplicantEmploymentController : BaseApplicantRoleRightsController<ApplicantEmployment>
    {

        [HttpGet]
       
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantEmployment.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
        public ActionResult MostRecentRecord(int? id)
        {
            try
            {
                AllowView();
                var model = db.ApplicantEmployment.Where(w => w.ApplicantInformationId == id && w.DataEntryStatus == 1)
                                                .OrderByDescending(u => u.CreatedDate)
                                                    .FirstOrDefault();

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
               
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEmployment", "MostRecentRecord");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Details(int id)
        {
            try
            {
                AllowView();
                var model = db.ApplicantEmployment.Where(w => w.Id == id).FirstOrDefault();
              
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
               
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEmployment", "Details");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.ApplicantCompanyId = new SelectList(db.GetAll<ApplicantCompany>(SessionHelper.SelectedClientId), "Id", "CompanyName");
                ViewBag.ApplicantPositionId = new SelectList(db.GetAll<ApplicantPosition>(SessionHelper.SelectedClientId), "Id", "PositionName");
                ViewBag.ApplicantExitTypeId = new SelectList(db.GetAll<ApplicantExitType>(SessionHelper.SelectedClientId), "Id", "ExitTypeName");
                ViewBag.RateFrequencyId = new SelectList(db.GetAll<RateFrequency>(SessionHelper.SelectedClientId), "Id", "RateFrequencyName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEmployment", "Create");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantEmployment.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.ApplicantCompanyId = new SelectList(db.GetAll<ApplicantCompany>(SessionHelper.SelectedClientId), "Id", "CompanyName",model.ApplicantCompanyId);
                ViewBag.ApplicantPositionId = new SelectList(db.GetAll<ApplicantPosition>(SessionHelper.SelectedClientId), "Id", "PositionName",model.ApplicantPositionId);
                ViewBag.ApplicantExitTypeId = new SelectList(db.GetAll<ApplicantExitType>(SessionHelper.SelectedClientId), "Id", "ExitTypeName",model.ApplicantExitTypeId);
                ViewBag.RateFrequencyId = new SelectList(db.GetAll<RateFrequency>(SessionHelper.SelectedClientId), "Id", "RateFrequencyName",model.RateFrequencyId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEmployment", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult CreateEdit(ApplicantEmployment model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantEmployment applicantEmploymentEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    applicantEmploymentEntity = new ApplicantEmployment();
                    applicantEmploymentEntity.ApplicantInformationId = model.ApplicantInformationId;
                    db.ApplicantEmployment.Add(applicantEmploymentEntity);
                }
                else
                {
                    applicantEmploymentEntity = db.ApplicantEmployment.Find(model.Id);
                    applicantEmploymentEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantEmploymentEntity.ModifiedDate = DateTime.Now;
                }
                applicantEmploymentEntity.ApplicantCompanyId = model.ApplicantCompanyId;
                applicantEmploymentEntity.ApplicantPositionId = model.ApplicantPositionId;
                applicantEmploymentEntity.CompanyTelephone = model.CompanyTelephone;
                applicantEmploymentEntity.CompanyAddress = model.CompanyAddress;
                applicantEmploymentEntity.ApplicantExitTypeId = model.ApplicantExitTypeId;
                applicantEmploymentEntity.ExitReason = model.ExitReason;
                applicantEmploymentEntity.EmploymentStartDate = model.EmploymentStartDate;
                applicantEmploymentEntity.IsCurrentEmployment = model.IsCurrentEmployment;
                applicantEmploymentEntity.EmploymentEndDate = model.EmploymentEndDate;
                applicantEmploymentEntity.SuperviorName = model.SuperviorName;
                applicantEmploymentEntity.Rate = model.Rate;
                applicantEmploymentEntity.RateFrequencyId = model.RateFrequencyId;

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

        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();
                var model = db.ApplicantEmployment.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEmployment", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantEmploymentEntity = db.ApplicantEmployment.Find(id);
            try
            {
                applicantEmploymentEntity.ModifiedBy = SessionHelper.LoginId;
                applicantEmploymentEntity.ModifiedDate = DateTime.Now;
                applicantEmploymentEntity.DataEntryStatus = 0;
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