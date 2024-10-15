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
    public class CertificationController : TimeAideWebControllers<Certification>
    {
        // POST: Certification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,CertificationName,Description,CertificationTypeId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Certification certification)
        {
            if (ModelState.IsValid)
            {
                db.Certification.Add(certification);
                db.SaveChanges();
                return Json(certification);
            }

            return GetErrors();
        }

        

        // POST: Certification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,CertificationName,Description,CertificationTypeId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Certification certification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certification).State = EntityState.Modified;
                certification.SetUpdated<Certification>();
                db.SaveChanges();
                return Json(certification);
            }

            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.Certification.Include(u => u.EmployeeCredential)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.EmployeeCredential.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
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
