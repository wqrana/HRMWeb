using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class RateFrequencyController : TimeAideWebControllers<RateFrequency>
    {
        // POST: RateFrequency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RateFrequencyName,RateFrequencyDescription,HourlyMultiplier,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] RateFrequency rateFrequency)
        {
            if (ModelState.IsValid)
            {
                db.RateFrequency.Add(rateFrequency);
                db.SaveChanges();
                return Json(rateFrequency);
            }
            return GetErrors();
        }


        // POST: RateFrequency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RateFrequencyName,RateFrequencyDescription,HourlyMultiplier,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] RateFrequency rateFrequency)
        {
            if (ModelState.IsValid)
            {
                rateFrequency.SetUpdated<RateFrequency>();
                db.Entry(rateFrequency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(RateFrequency model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.RateFrequency.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.RateFrequencyName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var rateFrequency = db.RateFrequency.Include(u => u.PayInformationHistory)
                .Include(u => u.PayScale)
                         .FirstOrDefault(c => c.Id == id);
            if (rateFrequency.PayInformationHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || rateFrequency.PayScale.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
