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
    public class GenderController : TimeAideWebControllers<Gender>
    {

        // POST: Gender/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GenderName,GenderDescription")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                db.Gender.Add(gender);
                db.SaveChanges();
                return Json(gender);
            }

            return GetErrors();
        }

        // POST: Gender/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GenderName,GenderDescription")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                gender.SetUpdated<Gender>();
                db.Entry(gender).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Gender model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Gender.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.GenderName });
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Gender.Include(u => u.EmployeeDependent)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeeDependent.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
