
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
    public class BenefitController : TimeAideWebControllers<Benefit>
    {
        // GET: Degree
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Benefit benefit)
        {
            if (ModelState.IsValid)
            {
                db.Benefit.Add(benefit);
                db.SaveChanges();
                return Json(benefit);
                // return RedirectToAction("Index");
            }

            // return PartialView(degree);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Benefit model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Benefit.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.BenefitName });
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Benefit benefit)
        {
            if (ModelState.IsValid)
            {
                benefit.ModifiedBy = SessionHelper.LoginId;
                benefit.ModifiedDate = DateTime.Now;
                db.Entry(benefit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(degree);
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Benefit.Include(u => u.EmployeeBenefitEnlisted)
                                   .Include(u => u.EmployeeBenefitHistory)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeeBenefitEnlisted.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmployeeBenefitHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
