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
    public class EmployeeDentalInsuranceController : TimeAideWebControllers<EmployeeDentalInsurance>
    {
        // GET: EmployeeDentalInsurance
     
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
            var model = db.EmployeeDentalInsurance.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.InsuranceStatusId = new SelectList(db.GetAll<InsuranceStatus>(SessionHelper.SelectedClientId), "Id", "InsuranceStatusName", model.InsuranceStatusId);
            ViewBag.InsuranceTypeId = new SelectList(db.GetAll<InsuranceType>(SessionHelper.SelectedClientId), "Id", "InsuranceTypeName", model.InsuranceTypeId);
            ViewBag.InsuranceCoverageId = new SelectList(db.GetAll<InsuranceCoverage>(SessionHelper.SelectedClientId), "Id", "InsuranceCoverageName", model.InsuranceCoverageId);
            ViewBag.CobraStatusId = new SelectList(db.GetAll<CobraStatus>(SessionHelper.SelectedClientId), "Id", "CobraStatusName", model.CobraStatusId);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateEdit(EmployeeDentalInsurance model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeDentalInsurance employeeDentalInsuranceEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeDentalInsuranceEntity = new EmployeeDentalInsurance();
                    employeeDentalInsuranceEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeDentalInsurance.Add(employeeDentalInsuranceEntity);
                }
                else
                {
                    employeeDentalInsuranceEntity = db.EmployeeDentalInsurance.Find(model.Id);
                    employeeDentalInsuranceEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeDentalInsuranceEntity.ModifiedDate = DateTime.Now;
                }
                employeeDentalInsuranceEntity.InsuranceStatusId = model.InsuranceStatusId;
                employeeDentalInsuranceEntity.InsuranceTypeId = model.InsuranceTypeId;
                employeeDentalInsuranceEntity.InsuranceStartDate = model.InsuranceStartDate;
                employeeDentalInsuranceEntity.InsuranceExpiryDate = model.InsuranceExpiryDate;
                employeeDentalInsuranceEntity.InsuranceCoverageId = model.InsuranceCoverageId;
                employeeDentalInsuranceEntity.GroupId = model.GroupId;

                employeeDentalInsuranceEntity.EmployeeContribution = model.EmployeeContribution;
                employeeDentalInsuranceEntity.CompanyContribution = model.CompanyContribution;
                employeeDentalInsuranceEntity.OtherContribution = model.OtherContribution;
                employeeDentalInsuranceEntity.TotalContribution = model.TotalContribution;
                employeeDentalInsuranceEntity.PCORIFee = model.PCORIFee;

                employeeDentalInsuranceEntity.CobraStatusId = model.CobraStatusId;
                employeeDentalInsuranceEntity.LeyCobraStartDate = model.LeyCobraStartDate;
                employeeDentalInsuranceEntity.LeyCobraExpiryDate = model.LeyCobraExpiryDate;
                employeeDentalInsuranceEntity.InsurancePremium = model.InsurancePremium;


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
            var entity = db.EmployeeDentalInsurance.Include(u => u.DentalInsuranceCobraHistory)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.DentalInsuranceCobraHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }
    }
}