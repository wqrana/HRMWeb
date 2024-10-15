using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class TerminationReasonController : TimeAideWebControllers<TerminationReason>
    {
        // POST: TerminationReason/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TerminationReasonName,TerminationReasonDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationReason terminationReason)
        {
            if (ModelState.IsValid)
            {
                db.TerminationReason.Add(terminationReason);
                db.SaveChanges();
                return Json(terminationReason);
            }
            return GetErrors();
        }

        // POST: TerminationReason/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TerminationReasonName,TerminationReasonDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationReason terminationReason)
        {
            if (ModelState.IsValid)
            {
                terminationReason.SetUpdated<TerminationReason>();
                db.Entry(terminationReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var terminationReason = db.TerminationReason.Include(u => u.Employments)
                         .FirstOrDefault(c => c.Id == id);
            if (terminationReason.Employments.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            
            return true;
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(TerminationReason model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.TerminationReason.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.TerminationReasonName });
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
