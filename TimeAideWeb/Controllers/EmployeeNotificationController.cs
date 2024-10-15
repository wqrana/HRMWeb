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
    public class EmployeeNotificationController : TimeAideWebControllers<EmployeeNotification>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeNotification employeeNotification)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.EmployeeNotification.Add(employeeNotification);
                    db.SaveChanges();
                    return Json(employeeNotification);
                }
                catch(Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeNotification employeeNotification)
        {
            if (ModelState.IsValid)
            {
                employeeNotification.SetUpdated<EmployeeNotification>();
                db.Entry(employeeNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeNotification
                         .FirstOrDefault(c => c.Id == id);
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
