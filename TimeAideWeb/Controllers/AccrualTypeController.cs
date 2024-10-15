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
    public class AccrualTypeController : TimeAideWebControllers<AccrualType>
    {

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(AccrualType accrualType)
        {
            if (ModelState.IsValid)
            {
                db.AccrualType.Add(accrualType);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex) 
                {
                }
                return Json(accrualType);
            }
            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(AccrualType accrualType)
        {
            if (ModelState.IsValid)
            {
                accrualType.SetUpdated<AccrualType>();
                db.Entry(accrualType).State = EntityState.Modified;
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
