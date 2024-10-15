using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class VeteranStatusController : TimeAideWebControllers<VeteranStatus>
    {
        // POST: VeteranStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VeteranStatusName,VeteranStatusDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] VeteranStatus veteranStatus)
        {
            if (ModelState.IsValid)
            {
                db.VeteranStatus.Add(veteranStatus);
                db.SaveChanges();
                return Json(veteranStatus);
            }
            return GetErrors();
        }

        // POST: VeteranStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VeteranStatusName,VeteranStatusDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] VeteranStatus veteranStatus)
        {
            if (ModelState.IsValid)
            {
                veteranStatus.SetUpdated<VeteranStatus>();
                db.Entry(veteranStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            StringBuilder str = new StringBuilder();
            var veteranStatus = db.VeteranStatus.Include(u => u.EmployeeVeteranStatus)

                         .FirstOrDefault(c => c.Id == id);
            if (veteranStatus.EmployeeVeteranStatus.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
