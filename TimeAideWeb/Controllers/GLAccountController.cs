using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class GLAccountController : TimeAideWebControllers<GLAccount>
    {

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(GLAccount gLAccount)
        {
            if (ModelState.IsValid)
            {
                db.GLAccount.Add(gLAccount);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex) 
                {
                }
                return Json(gLAccount);
            }
            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(GLAccount gLAccountType)
        {
            if (ModelState.IsValid)
            {
                gLAccountType.SetUpdated<GLAccount>();
                db.Entry(gLAccountType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
