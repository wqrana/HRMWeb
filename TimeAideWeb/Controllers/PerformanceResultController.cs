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
    public class PerformanceResultController : TimeAideWebControllers<PerformanceResult>
    {
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(PerformanceResult model)
        {
            if (ModelState.IsValid)
            {
                db.PerformanceResult.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            // return PartialView(model);
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(PerformanceResult model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.PerformanceResult.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.PerformanceResultName });
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(PerformanceResult model)
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

        public override bool CheckBeforeDelete(int id)
        {
            var performanceResult = db.PerformanceResult.Include(u => u.EmployeePerformance)
                         .FirstOrDefault(c => c.Id == id);
            if (performanceResult.EmployeePerformance.Where(t => t.DataEntryStatus == 1).Count() > 0)
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