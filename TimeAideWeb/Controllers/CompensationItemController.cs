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
    public class CompensationItemController : TimeAideWebControllers<CompensationItem>
    {
        [HttpPost]
        public ActionResult Create(CompensationItem CompensationItem)
        {
            if (ModelState.IsValid)
            {
                db.CompensationItem.Add(CompensationItem);
                db.SaveChanges();
                return Json(CompensationItem);
            }

            return GetErrors();
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CompensationItem compensationItem)
        {
            if (ModelState.IsValid)
            {
                compensationItem.SetUpdated<CompensationItem>();
                db.Entry(compensationItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.CompensationItem.Include(u => u.)
            //                          .Include(u => u.MailingCountryUserContactInformation)
            //                          .Include(u => u.HomeCountryUserContactInformation)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.States.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.MailingCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.HomeCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
