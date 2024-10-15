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
    public class CredentialTypeController : TimeAideWebControllers<CredentialType>
    {
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(CredentialType credentialType)
        {
            if (ModelState.IsValid)
            {
                db.CredentialType.Add(credentialType);
                db.SaveChanges();
                return Json(credentialType);
            }

            return GetErrors();
        }

        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CredentialType credentialType)
        {
            if (ModelState.IsValid)
            {
                    db.Entry(credentialType).State = EntityState.Modified;
                    credentialType.SetUpdated<CredentialType>();
                    db.SaveChanges();
                    return Json(credentialType);
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
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
