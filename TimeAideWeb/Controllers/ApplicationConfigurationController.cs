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
    public class ApplicationConfigurationController : TimeAideWebControllers<ApplicationConfiguration>
    {
        public ApplicationConfigurationController()
        {
            ViewBag.AllowAdd = false;
            ViewBag.AllowDelete = false;
        }

        // GET: ActionTaken
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationConfiguration model)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationConfiguration.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            //return PartialView(model);
            return GetErrors();
        }
        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationConfiguration model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(model);
            return GetErrors();
        }
        //AjaxEditConfiguration
        public JsonResult AjaxEditConfiguration(ApplicationConfiguration model)
        {
            string status = "Success";
            string message = "Configuration is successfully Update!";
            var applicationConfigurationEntity = db.ApplicationConfiguration.Find(model.Id);
            try
            {
                applicationConfigurationEntity.ApplicationConfigurationValue = model.ApplicationConfigurationValue;
                applicationConfigurationEntity.ModifiedBy = SessionHelper.LoginId;
                applicationConfigurationEntity.ModifiedDate = DateTime.Now;                
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
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