using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Reports;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ReportCriteriaTemplateController : TimeAideWebControllers<ReportCriteriaTemplate>
    {

        public JsonResult GetReportTemplateList(int? id)
        {
            IEnumerable<SelectListItem> reportCriteriaTemplateList = null;
            try
            {
                //AllowView();
               reportCriteriaTemplateList = db.GetAllByCompany<ReportCriteriaTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                                      .Where(w => w.ReportId == id && w.CreatedBy == SessionHelper.LoginId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.ReportCriteriaTemplateName });
                
            }
            catch (Exception ex)
            {
                             
                return Json(new { status = "Error", data = ex.Message });
            }
            return Json(new { status = "Success", data = reportCriteriaTemplateList },
                          JsonRequestBehavior.AllowGet);
        }
        public  JsonResult GetReportTemplateDetail(int? id)
        {
            ReportCriteriaTemplate model = null;
            try
            {
               
               // AllowView();
               model = db.Find<ReportCriteriaTemplate>(id.Value, SessionHelper.SelectedClientId);

                if (model == null)
                    model = new ReportCriteriaTemplate() { CriteriaType=0 };
                              
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", data = ex.Message }, JsonRequestBehavior.AllowGet);               
            }

            return Json(new { status = "Success", data =model }, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Create()
        {

            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.ReportCriteriaTemplate.Where(w => w.Id == id).FirstOrDefault();

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateEdit(ReportCriteriaTemplate model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            int id = 0;
            ReportCriteriaTemplate reportTemplateEntity = null;
            try
            {
                var isAlreadyExist = db.GetAllByCompany<ReportCriteriaTemplate>(SessionHelper.SelectedClientId, model.CompanyId??0)
                                        .Where(w => w.DataEntryStatus == 1 && w.ReportId== model.ReportId && (w.Id != model.Id) 
                                                && (w.ReportCriteriaTemplateName.ToLower() == model.ReportCriteriaTemplateName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Template Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        reportTemplateEntity = new ReportCriteriaTemplate();
                        db.ReportCriteriaTemplate.Add(reportTemplateEntity);
                    }
                    else
                    {
                        reportTemplateEntity = db.ReportCriteriaTemplate.Find(model.Id);
                        reportTemplateEntity.ModifiedBy = SessionHelper.LoginId;
                        reportTemplateEntity.ModifiedDate = DateTime.Now;
                    }
                    reportTemplateEntity.ReportId = model.ReportId;
                    reportTemplateEntity.ReportCriteriaTemplateName = model.ReportCriteriaTemplateName;
                    reportTemplateEntity.CompanyId = model.CompanyId;
                    reportTemplateEntity.CriteriaType = model.CriteriaType;
                    reportTemplateEntity.DepartmentSelectionIds = model.DepartmentSelectionIds;
                    reportTemplateEntity.SubDepartmentSelectionIds = model.SubDepartmentSelectionIds;
                    reportTemplateEntity.EmployeeTypeSelectionIds = model.EmployeeTypeSelectionIds;
                    reportTemplateEntity.EmploymentTypeSelectionIds = model.EmploymentTypeSelectionIds;
                    reportTemplateEntity.PositionSelectionIds = model.PositionSelectionIds;
                    reportTemplateEntity.StatusSelectionIds = model.StatusSelectionIds;
                    reportTemplateEntity.EmployeeSelectionIds = model.EmployeeSelectionIds;
                    reportTemplateEntity.FromDate = model.FromDate;
                    reportTemplateEntity.ToDate = model.ToDate;
                    reportTemplateEntity.DegreeSelectionIds = model.DegreeSelectionIds;
                    reportTemplateEntity.TrainingSelectionIds = model.TrainingSelectionIds;
                    reportTemplateEntity.CredentialSelectionIds = model.CredentialSelectionIds;
                    reportTemplateEntity.CustomFieldSelectionIds = model.CustomFieldSelectionIds;
                    reportTemplateEntity.BenefitSelectionIds = model.BenefitSelectionIds;
                    reportTemplateEntity.ActionTypeSelectionIds = model.ActionTypeSelectionIds;
                    reportTemplateEntity.SuperviorId = model.SuperviorId;
                                       
                    db.SaveChanges();
                    id = reportTemplateEntity.Id;
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status,id= id ,message = message });
        }

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var reportTemplateEntity = db.ReportCriteriaTemplate.Find(id);
            try
            {
                reportTemplateEntity.ModifiedBy = SessionHelper.LoginId;
                reportTemplateEntity.ModifiedDate = DateTime.Now;
                reportTemplateEntity.DataEntryStatus = 0;
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