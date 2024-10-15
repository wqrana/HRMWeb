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
    public class EmployeeStatusController : TimeAideWebControllers<EmployeeStatus>
    {
        // POST: EmployeeStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeStatusName,EmployeeStatusDescription,Enabled,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeStatus EmployeeStatus)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeStatus.Add(EmployeeStatus);
                db.SaveChanges();
                return Json(EmployeeStatus);
            }
            return GetErrors();
        }

        // POST: EmployeeStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeStatusName,EmployeeStatusDescription,Enabled,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeStatus EmployeeStatus)
        {
            if (ModelState.IsValid)
            {
                EmployeeStatus.SetUpdated();
                db.Entry(EmployeeStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeStatus.Include(u => u.UserInformations)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 )
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
