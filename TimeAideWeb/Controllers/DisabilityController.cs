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
    public class DisabilityController : TimeAideWebControllers<Disability>
    {

        // POST: Disability/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DisabilityName,DisabilityDescription")] Disability disability)
        {
            if (ModelState.IsValid)
            {
                db.Disability.Add(disability);
                db.SaveChanges();
                return Json(disability);
            }

            return GetErrors();
        }


        // POST: Disability/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DisabilityName,DisabilityDescription")] Disability disability)
        {
            if (ModelState.IsValid)
            {
                disability.SetUpdated<Disability>();
                db.Entry(disability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Disability.Include(u => u.UserInformation)
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
