using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class SenderEmailConfigurationController : TimeAideWebControllers<SenderEmailConfiguration>
    {

        public override void OnCreate()
        {
            ModelState.Clear();
            base.OnCreate();
        }
        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(SenderEmailConfiguration senderEmailConfiguration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.SenderEmailConfiguration.Add(senderEmailConfiguration);
                    db.SaveChanges();
                    return Json(senderEmailConfiguration);
                }
                catch(Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
            }

            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ValidateModel(SenderEmailConfiguration senderEmailConfiguration)
        {
            if (ModelState.IsValid)
            {
                return Json("Success");
            }
            return GetErrors();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult TestEmail(SenderEmailConfiguration senderEmailConfiguration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UtilityHelper.TestEmail(senderEmailConfiguration);
                    return Json(senderEmailConfiguration);
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    return Json("Error");
                }
            }

            return GetErrors();
        }

        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(SenderEmailConfiguration senderEmailConfiguration)
        {
            if (ModelState.IsValid)
            {
                senderEmailConfiguration.SetUpdated<SenderEmailConfiguration>();
                db.Entry(senderEmailConfiguration).State = EntityState.Modified;
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
