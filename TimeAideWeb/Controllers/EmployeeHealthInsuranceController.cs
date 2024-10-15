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
    public class EmployeeHealthInsuranceController : TimeAideWebControllers<EmployeeHealthInsurance>
    {
        // GET: EmployeeHealthInsurance
       
        public override ActionResult Create()
        {           
            ViewBag.InsuranceStatusId = new SelectList(db.GetAll<InsuranceStatus>(SessionHelper.SelectedClientId), "Id", "InsuranceStatusName");
            ViewBag.InsuranceTypeId = new SelectList(db.GetAll<InsuranceType>(SessionHelper.SelectedClientId), "Id", "InsuranceTypeName");
            ViewBag.InsuranceCoverageId = new SelectList(db.GetAll<InsuranceCoverage>(SessionHelper.SelectedClientId), "Id", "InsuranceCoverageName");
            ViewBag.CobraStatusId = new SelectList(db.GetAll<CobraStatus>(SessionHelper.SelectedClientId), "Id", "CobraStatusName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeHealthInsurance.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.InsuranceStatusId = new SelectList(db.GetAll<InsuranceStatus>(SessionHelper.SelectedClientId), "Id", "InsuranceStatusName",model.InsuranceStatusId);
            ViewBag.InsuranceTypeId = new SelectList(db.GetAll<InsuranceType>(SessionHelper.SelectedClientId), "Id", "InsuranceTypeName",model.InsuranceTypeId);
            ViewBag.InsuranceCoverageId = new SelectList(db.GetAll<InsuranceCoverage>(SessionHelper.SelectedClientId), "Id", "InsuranceCoverageName",model.InsuranceCoverageId);
            ViewBag.CobraStatusId = new SelectList(db.GetAll<CobraStatus>(SessionHelper.SelectedClientId), "Id", "CobraStatusName", model.CobraStatusId);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateEdit(EmployeeHealthInsurance model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeHealthInsurance employeeEmployeeHealthInsuranceEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeEmployeeHealthInsuranceEntity = new EmployeeHealthInsurance();
                    employeeEmployeeHealthInsuranceEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeHealthInsurance.Add(employeeEmployeeHealthInsuranceEntity);
                }
                else
                {
                    employeeEmployeeHealthInsuranceEntity = db.EmployeeHealthInsurance.Find(model.Id);
                    employeeEmployeeHealthInsuranceEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeEmployeeHealthInsuranceEntity.ModifiedDate = DateTime.Now;
                }
                employeeEmployeeHealthInsuranceEntity.InsuranceStatusId = model.InsuranceStatusId;
                employeeEmployeeHealthInsuranceEntity.InsuranceTypeId = model.InsuranceTypeId;
                employeeEmployeeHealthInsuranceEntity.InsuranceStartDate = model.InsuranceStartDate;
                employeeEmployeeHealthInsuranceEntity.InsuranceExpiryDate = model.InsuranceExpiryDate;
                employeeEmployeeHealthInsuranceEntity.InsuranceCoverageId = model.InsuranceCoverageId;
                employeeEmployeeHealthInsuranceEntity.GroupId = model.GroupId;

                employeeEmployeeHealthInsuranceEntity.EmployeeContribution = model.EmployeeContribution;
                employeeEmployeeHealthInsuranceEntity.CompanyContribution = model.CompanyContribution;
                employeeEmployeeHealthInsuranceEntity.OtherContribution = model.OtherContribution;
                employeeEmployeeHealthInsuranceEntity.TotalContribution = model.TotalContribution;
                employeeEmployeeHealthInsuranceEntity.PCORIFee = model.PCORIFee;

                employeeEmployeeHealthInsuranceEntity.CobraStatusId = model.CobraStatusId;
                employeeEmployeeHealthInsuranceEntity.LeyCobraStartDate = model.LeyCobraStartDate;
                employeeEmployeeHealthInsuranceEntity.LeyCobraExpiryDate = model.LeyCobraExpiryDate;
                employeeEmployeeHealthInsuranceEntity.InsurancePremium = model.InsurancePremium;


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
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeHealthInsurance.Include(u => u.HealthInsuranceCobraHistory)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.HealthInsuranceCobraHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }
    }
}