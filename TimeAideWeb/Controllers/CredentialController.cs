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
    public class CredentialController :  TimeAideWebControllers<Credential>
    {

        // POST: Credential/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Credential credential)
        {
            if (ModelState.IsValid)
            {
                db.Credential.Add(credential);
                db.SaveChanges();
                return Json(credential);
            }

            return GetErrors();
        }

        // POST: Credential/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Credential credential)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credential).State = EntityState.Modified;
                credential.SetUpdated<Credential>();
                db.SaveChanges();
                return Json(credential);
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Credential.Include(u => u.EmployeeCredential)
                                      //.Include(u => u.EmployeeRequiredCredential)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeeCredential.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            else if (db.PositionCredential.Where(w => w.CredentialId == id && w.DataEntryStatus == 1).Count() > 0)
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
