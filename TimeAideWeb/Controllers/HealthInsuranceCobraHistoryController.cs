using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class HealthInsuranceCobraHistoryController : TimeAideWebControllers<HealthInsuranceCobraHistory>
    {
        // GET: HealthInsuranceCobraHistory
        
        public ActionResult IndexByUserInsurance(int id)
        {
            var model = db.HealthInsuranceCobraHistory.Where(w => w.DataEntryStatus == 1
                                                        && w.EmployeeHealthInsurance.UserInformationId == id
                                                        ).OrderByDescending(o => o.Id);
            return PartialView("IndexByUserInsurance", model);
        }
        public override ActionResult Create()
        {

            ViewBag.CobraPaymentStatusId = new SelectList(db.GetAll<CobraPaymentStatus>(SessionHelper.SelectedClientId), "Id", "CobraPaymentStatusName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.HealthInsuranceCobraHistory.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.CobraPaymentStatusId = new SelectList(db.GetAll<CobraPaymentStatus>(SessionHelper.SelectedClientId), "Id", "CobraPaymentStatusName", model.CobraPaymentStatusId);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateEdit(HealthInsuranceCobraHistory model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            HealthInsuranceCobraHistory employeeInsuranceCobraHistoryEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeInsuranceCobraHistoryEntity = new HealthInsuranceCobraHistory();
                    employeeInsuranceCobraHistoryEntity.EmployeeHealthInsuranceId = model.EmployeeHealthInsuranceId;
                    db.HealthInsuranceCobraHistory.Add(employeeInsuranceCobraHistoryEntity);
                }
                else
                {
                    employeeInsuranceCobraHistoryEntity = db.HealthInsuranceCobraHistory.Find(model.Id);
                    employeeInsuranceCobraHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeInsuranceCobraHistoryEntity.ModifiedDate = DateTime.Now;
                }
                employeeInsuranceCobraHistoryEntity.DueDate = model.DueDate;
                employeeInsuranceCobraHistoryEntity.PaymentDate = model.PaymentDate;
                employeeInsuranceCobraHistoryEntity.CobraPaymentStatusId = model.CobraPaymentStatusId;
                employeeInsuranceCobraHistoryEntity.PaymentAmount = model.PaymentAmount;
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
            var employeeInsuranceCobraHistorEntity = db.HealthInsuranceCobraHistory.Find(id);
            try
            {
                employeeInsuranceCobraHistorEntity.ModifiedBy = SessionHelper.LoginId;
                employeeInsuranceCobraHistorEntity.ModifiedDate = DateTime.Now;
                employeeInsuranceCobraHistorEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
    }
}