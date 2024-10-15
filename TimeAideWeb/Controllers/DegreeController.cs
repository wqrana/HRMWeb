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
    public class DegreeController : TimeAideWebControllers<Degree>
    {
        // GET: Degree
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Degree degree)
        {
            if (ModelState.IsValid)
            {
                db.Degree.Add(degree);
                db.SaveChanges();
                return Json(degree);
                // return RedirectToAction("Index");
            }

            // return PartialView(degree);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Degree model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Degree.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.DegreeName });
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Degree degree)
        {
            if (ModelState.IsValid)
            {
                degree.ModifiedBy = SessionHelper.LoginId;
                degree.ModifiedDate = DateTime.Now;
                db.Entry(degree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(degree);
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Degree.Include(u => u.EmployeeEducation)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeeEducation.Where(t => t.DataEntryStatus == 1).Count() > 0)
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