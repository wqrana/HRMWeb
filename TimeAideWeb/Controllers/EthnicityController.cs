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
    public class EthnicityController : TimeAideWebControllers<Ethnicity>
    {

        // POST: Ethnicity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EthnicityName,EthnicityDescription")] Ethnicity ethnicity)
        {
            if (ModelState.IsValid)
            {
                db.Ethnicity.Add(ethnicity);
                db.SaveChanges();
                return Json(ethnicity);
            }

            return GetErrors();
        }

        // POST: Ethnicity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EthnicityName,EthnicityDescription")] Ethnicity ethnicity)
        {
            if (ModelState.IsValid)
            {
                ethnicity.SetUpdated<Ethnicity>();
                db.Entry(ethnicity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Ethnicity.Include(u => u.UserInformation)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
