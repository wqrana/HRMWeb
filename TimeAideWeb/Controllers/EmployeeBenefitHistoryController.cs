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
    public class EmployeeBenefitHistoryController : TimeAideWebControllers<EmployeeBenefitHistory>
    {
        // GET: EmployeeDentalInsurance

        public override ActionResult Create()
        {
            ViewBag.BenefitPayFrequencyId = new SelectList(db.GetAll<PayFrequency>(SessionHelper.SelectedClientId), "Id", "PayFrequencyName");
            ViewBag.BenefitId = new SelectList(db.GetAll<Benefit>(SessionHelper.SelectedClientId), "Id", "BenefitName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeBenefitHistory.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.BenefitPayFrequencyId = new SelectList(db.GetAll<PayFrequency>(SessionHelper.SelectedClientId), "Id", "PayFrequencyName",model.PayFrequencyId);
            ViewBag.BenefitId = new SelectList(db.GetAll<Benefit>(SessionHelper.SelectedClientId), "Id", "BenefitName", model.BenefitId);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateEdit(EmployeeBenefitHistory model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeBenefitHistory employeeBenefitHistoryEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeBenefitHistoryEntity = new EmployeeBenefitHistory();
                    employeeBenefitHistoryEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeBenefitHistory.Add(employeeBenefitHistoryEntity);
                }
                else
                {
                    employeeBenefitHistoryEntity = db.EmployeeBenefitHistory.Find(model.Id);
                    employeeBenefitHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeBenefitHistoryEntity.ModifiedDate = DateTime.Now;
                }
                //employeeDentalInsuranceEntity.InsuranceStatusId = model.InsuranceStatusId;
                employeeBenefitHistoryEntity.StartDate = model.StartDate;
                employeeBenefitHistoryEntity.ExpiryDate = model.ExpiryDate;
                employeeBenefitHistoryEntity.Amount = model.Amount;
                employeeBenefitHistoryEntity.PayFrequencyId = model.PayFrequencyId;
                employeeBenefitHistoryEntity.BenefitId = model.BenefitId;
                employeeBenefitHistoryEntity.Notes = model.Notes;
                employeeBenefitHistoryEntity.EmployeeContribution = model.EmployeeContribution;
                employeeBenefitHistoryEntity.CompanyContribution = model.CompanyContribution;
                employeeBenefitHistoryEntity.OtherContribution = model.OtherContribution;
                employeeBenefitHistoryEntity.TotalContribution = model.TotalContribution;

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

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var employeeBenefitHistoryEntity = db.EmployeeBenefitHistory.Find(id);
            try
            {
                employeeBenefitHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                employeeBenefitHistoryEntity.ModifiedDate = DateTime.Now;
                employeeBenefitHistoryEntity.DataEntryStatus = 0;
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