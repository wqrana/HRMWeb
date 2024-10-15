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
    public class TerminationTypeController : TimeAideWebControllers<TerminationType>
    {

        // POST: TerminationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TerminationTypeName,TerminationTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationType terminationType)
        {
            if (ModelState.IsValid)
            {
                db.TerminationType.Add(terminationType);
                db.SaveChanges();
                return Json(terminationType);
            }
            return GetErrors();
        }

        // POST: TerminationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TerminationTypeName,TerminationTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationType terminationType)
        {
            if (ModelState.IsValid)
            {
                terminationType.SetUpdated<TerminationType>();
                db.Entry(terminationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var terminationType = db.TerminationType.Include(u => u.Employments)
                         .FirstOrDefault(c => c.Id == id);
            if (terminationType.Employments.Where(t=>t.DataEntryStatus==1).Count() > 0)
                return false;
            
            return true;
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(TerminationType model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.TerminationType.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.TerminationTypeName });
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
