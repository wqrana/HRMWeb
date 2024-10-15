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
    public class TrainingTypeController : TimeAideWebControllers<TrainingType>
    {

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                db.TrainingType.Add(trainingType);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex) 
                {
                }
                return Json(trainingType);
            }
            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                trainingType.SetUpdated<TrainingType>();
                db.Entry(trainingType).State = EntityState.Modified;
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
