using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class PayFrequencyController : TimeAideWebControllers<PayFrequency>
    {
        // POST: PayFrequency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(PayFrequency payFrequency)
        {
            if (ModelState.IsValid)
            {
                db.PayFrequency.Add(payFrequency);
                db.SaveChanges();
                return Json(payFrequency);
            }
            return GetErrors();
        }

        // POST: PayFrequency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(PayFrequency payFrequency)
        {
            if (ModelState.IsValid)
            {
                payFrequency.SetUpdated<PayFrequency>();
                db.Entry(payFrequency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(PayFrequency model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.PayFrequency.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.PayFrequencyName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.PayFrequency.Include(u => u.PayInformationHistory)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.PayInformationHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
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
