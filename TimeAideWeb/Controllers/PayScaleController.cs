using Newtonsoft.Json;
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
    public class PayScaleController : TimeAideWebControllers<PayScale>
    {
        // POST: PayScale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,PayScaleName,RateFrequencyId,PayScaleLevel,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayScale payScale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PayScale.Add(payScale);
                    db.SaveChanges();
                }
                catch 
                {
                }

                var list = JsonConvert.SerializeObject(payScale, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                //return Json(payScale);
                return Content(list, "application/json");
            }
            return GetErrors();
        }

        [HttpPost]
        public ActionResult CreatePopup([Bind(Include = "Id,PayScaleName,RateFrequencyId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayScale payScale)
        {
            if (ModelState.IsValid)
            {
                db.PayScale.Add(payScale);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return Json(payScale);
            }

            return PartialView(payScale);
        }




        // POST: PayScale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PayScaleName,RateFrequencyId,PayScaleLevel,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayScale payScale)
        {
            if (ModelState.IsValid)
            {
                payScale.SetUpdated<PayScale>();
                db.Entry(payScale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(PayScale model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.PayScale.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.PayScaleName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.PayScale.Include(u => u.PayScaleLevel)
                .Include(u => u.Position)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.PayScaleLevel.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.Position.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
