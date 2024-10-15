using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class PayRateMultiplierController : TimeAideWebControllers<PayRateMultiplier>
    {

        // POST: PayRateMultiplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(PayRateMultiplier payRateMultiplier)
        {
            if (ModelState.IsValid)
            {
                db.PayRateMultiplier.Add(payRateMultiplier);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex) 
                {
                }
                return Json(payRateMultiplier);
            }
            //ViewBag.ClientId = new SelectList(db.Client.Where(u => u.DataEntryStatus == 1), "ClientId", "Client");
            //ViewBag.CFSECodeId = new SelectList(db.CFSECode.Where(c=>c.DataEntryStatus==1), "Id", "CFSECodeDescription", payRateMultiplier.CFSECodeId);

            return GetErrors();
        }

        // POST: PayRateMultiplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(PayRateMultiplier payRateMultiplier)
        {
            if (ModelState.IsValid)
            {
                payRateMultiplier.SetUpdated<PayRateMultiplier>();
                db.Entry(payRateMultiplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(PayRateMultiplier model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.PayRateMultiplier.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.PayRateMultiplierName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var payRateMultiplier = db.PayRateMultiplier.Include(u => u.TransactionConfiguration)
                         .FirstOrDefault(c=>c.Id==id);
            if (payRateMultiplier.TransactionConfiguration.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }

        public JsonResult AjaxGetPayRateMultiplier(EmployeePrivilegeViewModel model)
        {
            var payRateMultiplierList = db.GetAllByCompany<PayRateMultiplier>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId)
                                .Select(s => new { id = s.Id, name = s.PayRateMultiplierName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = payRateMultiplierList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
            return jsonResult;
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
