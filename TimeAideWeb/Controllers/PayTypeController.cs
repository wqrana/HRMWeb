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
    public class PayTypeController : TimeAideWebControllers<PayType>
    {
        // POST: PayType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "PayTypeId,PayTypeName,PayTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayType payType)
        {
            if (ModelState.IsValid)
            {
                db.PayType.Add(payType);
                db.SaveChanges();
                return Json(payType);
            }
            return GetErrors();
        }

        // POST: PayType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,PayTypeName,PayTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayType payType)
        {
            if (ModelState.IsValid)
            {
                payType.SetUpdated<PayType>();
                db.Entry(payType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");  
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(PayType model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.PayType.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.PayTypeName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.PayType.Include(u => u.PayInformationHistory)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.PayInformationHistory.Count > 0)
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
