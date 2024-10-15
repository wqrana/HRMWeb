using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class PayInformationHistoryController : TimeAideWebControllers<PayInformationHistory>
    {
        public override List<PayInformationHistory> OnIndexByUser(List<PayInformationHistory> model, int userId)
        {
            return model = model.OrderByDescending(e => e.Employment.OriginalHireDate).OrderByDescending(e=>e.StartDate).ToList();
        }

        public override PayInformationHistory OnMostRecentRecord(List<PayInformationHistory> model, int userId)
        {
            return model.OrderByDescending(e => e.StartDate).FirstOrDefault();
        }

        public override void OnClose(PayInformationHistory entity)
        {
            int userInformationId = entity.UserInformationId ?? 0;
            var list = db.EmployeeSupervisor.Where(e => e.ClientId == SessionHelper.SelectedClientId && e.EmployeeUserId == userInformationId).Select(e => e.SupervisorUser);
            ViewBag.SelectedAuthorizById = new SelectList(list, "Id", "FullName");
        }
        [HttpPost]
        public ActionResult AddByUser(PayInformationHistory payInformationHistory)
        {
            var currentRecord = EmploymentService.GetActiveEmployment(payInformationHistory.UserInformationId ?? 0);
            if (ModelState.IsValid)
            {
                if (currentRecord == null)
                {
                    ModelState.AddModelError("StartDate", "Missing active hiring information for selected employee. Please add hiring information.");
                }
            }

            if (ModelState.IsValid)
            {
                payInformationHistory.EmploymentId = currentRecord.Id;
                db.PayInformationHistory.Add(payInformationHistory);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
                return RedirectToAction("IndexByUser", new { id = payInformationHistory.UserInformationId });
            }

            return GetErrors();
        }

        public override PayInformationHistory OnEdit(PayInformationHistory entity)
        {
            ViewBag.HourlyMultiplierId = new SelectList(db.GetAll<PayFrequency>(SessionHelper.SelectedClientId), "Id", "HourlyMultiplier", entity.PayFrequencyId);
            ViewBag.RateFrequencyMultiplierId = new SelectList(db.GetAll<RateFrequency>(SessionHelper.SelectedClientId), "Id", "HourlyMultiplier", entity.RateFrequencyId);
            return entity;
        }

        public override void OnCreate()
        {
            ViewBag.HourlyMultiplierId = new SelectList(db.GetAll<PayFrequency>(SessionHelper.SelectedClientId), "Id", "HourlyMultiplier");
            ViewBag.RateFrequencyMultiplierId = new SelectList(db.GetAll<RateFrequency>(SessionHelper.SelectedClientId), "Id", "HourlyMultiplier");
        }
        // POST: PayInformationHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(PayInformationHistory payInformationHistory)
        {
            if (ModelState.IsValid)
            {

                var model = db.PayInformationHistory.FirstOrDefault(e => e.Id == payInformationHistory.Id);
                model.StartDate = payInformationHistory.StartDate;
                model.RateAmount = payInformationHistory.RateAmount;
                model.RateFrequencyId = payInformationHistory.RateFrequencyId;
                model.CommRateAmount = payInformationHistory.CommRateAmount;
                model.CommRateFrequencyId = payInformationHistory.CommRateFrequencyId;
                model.PayTypeId = payInformationHistory.PayTypeId;
                model.PayFrequencyId = payInformationHistory.PayFrequencyId;
                model.PeriodHours = payInformationHistory.PeriodHours;
                model.PeriodGrossPay = payInformationHistory.PeriodGrossPay;
                model.YearlyGrossPay = payInformationHistory.YearlyGrossPay;
                model.YearlyCommBasePay = payInformationHistory.YearlyCommBasePay;
                model.YearlyBaseNCommPay = payInformationHistory.YearlyBaseNCommPay;
                //model.PositionId = payInformationHistory.PositionId;
                model.EEOCategoryId = payInformationHistory.EEOCategoryId;
                model.WCClassCodeId = payInformationHistory.WCClassCodeId;
                model.PayScaleId = payInformationHistory.PayScaleId;
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("IndexByUser", new { id = payInformationHistory.UserInformationId });
            }
            return GetErrors();
        }

        public JsonResult GetHourlyBaseRate(int rateFrequencyId, decimal rateAmount)
        {
            decimal retHourlyRate =0;

            if(rateFrequencyId>0 && rateAmount > 0)
            {
                var rateFrequency = db.RateFrequency.Find(rateFrequencyId);

                retHourlyRate = Math.Round(rateAmount / (rateFrequency.HourlyMultiplier ?? 1),5);
            }


            return new JsonResult()
            {

                Data = retHourlyRate,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
        }
        public JsonResult GetPayPeriodHours(int payFrequencyId)
        {
            decimal retPeriodHours = 0;

            if (payFrequencyId > 0)
            {
                var payFrequency = db.PayFrequency.Find(payFrequencyId);

                retPeriodHours = payFrequency.HourlyMultiplier??0;
            }


            return new JsonResult()
            {

                Data = retPeriodHours,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
        }
        [HttpPost]
        public JsonResult AjaxPayCalculation(PayInformationHistory model)
        {
            string status = "Success";
            string message = "Successfully caculated!";
            model.PayFrequency = db.PayFrequency.Find(model.PayFrequencyId);
            model.RateFrequency = db.RateFrequency.Find(model.RateFrequencyId);
            model.CommRateFrequency = db.RateFrequency.Find(model.CommRateFrequencyId ?? 0);
            var yearlyPayFrequency = db.GetAll<PayFrequency>().Where(w => w.PayFrequencyName.ToLower().Contains("yearly")).FirstOrDefault();
            decimal yearlyPayMultiplyFactor = yearlyPayFrequency==null?2080: yearlyPayFrequency.HourlyMultiplier??2080;
            decimal periodPayMultiplyFactor = model.PayFrequency == null ? 0 : model.PayFrequency.HourlyMultiplier ?? 0;
            decimal periodtoYearlyPayFactor = 1;
            try
            {
               if(yearlyPayMultiplyFactor>0 && periodPayMultiplyFactor > 0)
                {
                    periodtoYearlyPayFactor = yearlyPayMultiplyFactor / periodPayMultiplyFactor;
                }
               if(model.HourlyRate>0 && model.PeriodHours > 0)
                {
                    model.PeriodGrossPay = Math.Round(model.HourlyRate * model.PeriodHours.Value,5);
                }
                if (model.PeriodGrossPay > 0)
                {
                    model.YearlyGrossPay = Math.Round(model.PeriodGrossPay * periodtoYearlyPayFactor,5);
                }
                if (model.HourlyCommRate > 0)
                {
                    model.YearlyCommBasePay = Math.Round(model.HourlyCommRate * yearlyPayMultiplyFactor,5);
                }
                model.YearlyBaseNCommPay = Math.Round((model.YearlyGrossPay) + (model.YearlyCommBasePay ?? 0),5);
                
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            var retPayDetail = new { PeriodGrossPay = model.PeriodGrossPay, YearlyGrossPay = model.YearlyGrossPay, YearlyCommBasePay = model.YearlyCommBasePay ?? 0, YearlyBaseNCommPay = model.YearlyBaseNCommPay ?? 0 };
            return Json(new {payInfo= retPayDetail, status = status, message = message });
        }
        [HttpPost]
        public ActionResult Close(PayInformationHistory payInformationHistory)
        {
            ModelState.Remove("StartDate");
            //if (String.IsNullOrEmpty(payInformationHistory.SelectedAuthorizById))
            //    ModelState.AddModelError("SelectedAuthorizById", "The Authorize by field is required.");
            if (!payInformationHistory.EndDate.HasValue)
                ModelState.AddModelError("EndDate", "The end date field is required.");
            if (!payInformationHistory.ApprovedDate.HasValue)
                    ModelState.AddModelError("ApprovedDate", "The approved date field is required.");
            var modelFromDb = PayInformationHistoryService.GetPayInformationHistory(payInformationHistory.Id, db);
            if (ModelState.IsValid)
            {

                if (modelFromDb.StartDate > payInformationHistory.EndDate.Value)
                    ModelState.AddModelError("EndDate", "The end date can't be prior to start Date.");

                //var currentRecord = EmploymentService.GetActiveEmployment(payInformationHistory.UserInformationId ?? 0);
                var payInfoLinkedEmploymentRecord = EmploymentService.GetEmployment(modelFromDb.EmploymentId);
                if (payInfoLinkedEmploymentRecord != null)
                {
                    if (payInfoLinkedEmploymentRecord.OriginalHireDate > payInformationHistory.EndDate.Value)
                        ModelState.AddModelError("EndDate", "The pay Information End Date can't be prior to Current Period Hiring Date.");
                    if (payInfoLinkedEmploymentRecord.EffectiveHireDate > payInformationHistory.EndDate.Value)
                        ModelState.AddModelError("EndDate", "The pay Information End Date can't be prior to Current Period Re-Hiring Date.");
                }
                if (modelFromDb.StartDate > payInformationHistory.ApprovedDate.Value)
                    ModelState.AddModelError("ApprovedDate", "The pay Information Approved Date can't be prior to pay Information start Date.");

                if (payInformationHistory.EndDate.Value > payInformationHistory.ApprovedDate.Value)
                    ModelState.AddModelError("ApprovedDate", "The pay Information Approved Date can't be prior to pay Information end Date.");

            }
            if (ModelState.IsValid)
            {
                var activeEmploymentRecord = EmploymentService.GetActiveEmployment(payInformationHistory.UserInformationId ?? 0);
                if (activeEmploymentRecord != null)
                {
                    if (payInformationHistory.EndDate>=activeEmploymentRecord.OriginalHireDate.Value  && payInformationHistory.EmploymentId!= activeEmploymentRecord.Id)
                    {
                        ModelState.AddModelError("EndDate", "The pay Information end date should be prior to active period hiring date.");
                    }
                }
                var closedEmployments = EmploymentService.GetClosedEmployments(payInformationHistory.UserInformationId ?? 0);
                foreach (var eachEmplyment in closedEmployments)
                {
                    if (eachEmplyment.OriginalHireDate.Value > payInformationHistory.EndDate && payInformationHistory.EmploymentId != eachEmplyment.Id)
                    {
                        ModelState.AddModelError("EndDate", "The pay Information end date can't be prior to Closed record hiring date.");
                        break;
                    }
                    if (eachEmplyment.TerminationDate.Value > payInformationHistory.StartDate && payInformationHistory.EmploymentId != eachEmplyment.Id)
                    {
                        ModelState.AddModelError("EndDate", "The pay Information end date can't be prior to  Closed record hiring end date.");
                        break;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                modelFromDb.EndDate = payInformationHistory.EndDate;
                modelFromDb.ApprovedDate = payInformationHistory.ApprovedDate;
                modelFromDb.ChangeReason = payInformationHistory.ChangeReason;
                List<string> AuthorizByIds = (payInformationHistory.SelectedAuthorizById ?? "").Split(',').ToList();
                if (modelFromDb.PayInformationHistoryAuthorizer.Count > 0)
                {
                    var removeAuthorizByList = modelFromDb.PayInformationHistoryAuthorizer.ToList();
                    foreach (var removeAuthorizBy in removeAuthorizByList)
                    {
                        if(!AuthorizByIds.Contains(removeAuthorizBy.AuthorizeById.ToString()))
                            modelFromDb.PayInformationHistoryAuthorizer.Remove(removeAuthorizBy);
                    }
                    db.SaveChanges();
                }
                foreach (var eachId in AuthorizByIds)
                {
                    int newId;
                    Int32.TryParse(eachId, out newId);
                    if (newId > 0 && modelFromDb.PayInformationHistoryAuthorizer.Where(n => n.AuthorizeById == newId).Count() == 0)
                        modelFromDb.PayInformationHistoryAuthorizer.Add(new PayInformationHistoryAuthorizer { AuthorizeById = Convert.ToInt32(eachId), PayInformationHistoryId = modelFromDb.Id });
                }
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = payInformationHistory.UserInformationId });
            }
            return GetErrors();
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
