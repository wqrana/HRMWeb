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
    public class JobCodeController : TimeAideWebControllers<JobCode>
    {

        // POST: JobCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobCodeName,JobCodeDescription,Enabled,ProjectId,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] JobCode jobCode)
        {
            if (ModelState.IsValid)
            {
                db.JobCode.Add(jobCode);
                db.SaveChanges();
                return Json(jobCode);
            }
            return GetErrors();
        }

        // POST: JobCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobCodeName,JobCodeDescription,Enabled,ProjectId,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] JobCode jobCode)
        {
            if (ModelState.IsValid)
            {
                jobCode.SetUpdated<JobCode>();
                db.Entry(jobCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.JobCode.Include(u => u.UserInformations)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
