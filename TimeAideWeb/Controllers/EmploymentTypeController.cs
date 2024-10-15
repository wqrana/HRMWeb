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
    public class EmploymentTypeController : TimeAideWebControllers<EmploymentType>
    {

        // POST: EmploymentType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EmploymentType employmentType)
        {
            if (ModelState.IsValid)
            {
                db.EmploymentType.Add(employmentType);
                db.SaveChanges();
                return Json(employmentType);
            }

            return GetErrors();
        }


        // POST: EmploymentType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmploymentType employmentType)
        {
            if (ModelState.IsValid)
            {
                employmentType.SetUpdated<EmploymentType>();
                db.Entry(employmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(EmploymentType model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.EmploymentType.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.EmploymentTypeName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            try
            {
                var entity = db.EmploymentType.Include(u => u.EmploymentHistory)
                             .FirstOrDefault(c => c.Id == id);
                if (entity.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                    return false;
            }
            catch 
            {
                return false;
            }
            
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
