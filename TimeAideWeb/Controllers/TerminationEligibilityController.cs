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

namespace WebApplication4.Controllers
{
    public class TerminationEligibilityController : TimeAideWebControllers<TerminationEligibility>
    {
        // POST: TerminationEligibility/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TerminationEligibilityName,TerminationEligibilityDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationEligibility terminationEligibility)
        {
            if (ModelState.IsValid)
            {
                db.TerminationEligibility.Add(terminationEligibility);
                db.SaveChanges();
                return Json(terminationEligibility);
            }
            return GetErrors();
        }

        // POST: TerminationEligibility/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TerminationEligibilityName,TerminationEligibilityDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] TerminationEligibility terminationEligibility)
        {
            if (ModelState.IsValid)
            {
                terminationEligibility.SetUpdated<TerminationEligibility>();
                db.Entry(terminationEligibility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var terminationEligibility = db.TerminationEligibility.Include(u => u.Employments)
                         .FirstOrDefault(c => c.Id == id);
            if (terminationEligibility.Employments.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
