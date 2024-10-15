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
    public class CertificationTypeController : TimeAideWebControllers<CertificationType>
    {
        // POST: CertificationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CertificationTypeName,Description,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] CertificationType certificationType)
        {
            if (ModelState.IsValid)
            {
                db.CertificationType.Add(certificationType);
                db.SaveChanges();
                return Json(certificationType);
            }

            return GetErrors();
        }

        // POST: CertificationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CertificationTypeName,Description,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] CertificationType certificationType)
        {
            if (ModelState.IsValid)
            {
                    db.Entry(certificationType).State = EntityState.Modified;
                    certificationType.SetUpdated<CertificationType>();
                    db.SaveChanges();
                    return Json(certificationType);
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.CertificationType.Include(u => u.Certification)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.Certification.Where(t => t.DataEntryStatus == 1).Count() > 0 )
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
