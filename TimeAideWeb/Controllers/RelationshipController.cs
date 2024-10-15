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
    public class RelationshipController : TimeAideWebControllers<Relationship>
    {
        // POST: Relationship/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RelationshipName,RelationshipDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Relationship relationship)
        {
            if (ModelState.IsValid)
            {
                db.Relationship.Add(relationship);
                db.SaveChanges();
                return Json(relationship);
            }
            return GetErrors();
        }

        // POST: Relationship/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RelationshipName,RelationshipDescription")] Relationship relationship)
        {
            if (ModelState.IsValid)
            {
                relationship.SetUpdated<Relationship>();
                db.Entry(relationship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Relationship model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Relationship.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.RelationshipName });
        }

        public override bool CheckBeforeDelete(int id)
        {
            var relationship = db.Relationship.Include(u => u.EmergencyContact)
                         .Include(u => u.EmployeeDependent)
                         .FirstOrDefault(c => c.Id == id);
            if (relationship.EmergencyContact.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (relationship.EmployeeDependent.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
