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
    public class SupervisoryLevelController : TimeAideWebControllers<SupervisoryLevel>
    {
        // POST: SupervisoryLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SupervisoryLevelName,SupervisoryLevelDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] SupervisoryLevel supervisoryLevel)
        {
            if (ModelState.IsValid)
            {
                db.SupervisoryLevel.Add(supervisoryLevel);
                db.SaveChanges();
                return Json(supervisoryLevel);
            }
            return GetErrors();
        }

       

        // POST: SupervisoryLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SupervisoryLevelName,SupervisoryLevelDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] SupervisoryLevel supervisoryLevel)
        {
            if (ModelState.IsValid)
            {
                supervisoryLevel.SetUpdated();
                db.Entry(supervisoryLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
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
