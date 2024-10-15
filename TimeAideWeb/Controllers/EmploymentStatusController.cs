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
    public class EmploymentStatusController : TimeAideWebControllers<EmploymentStatus>
    {
        // POST: EmploymentStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmploymentStatusName,EmploymentStatusDescription,Enabled,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmploymentStatus employmentStatus)
        {
            if (ModelState.IsValid)
            {
                db.EmploymentStatus.Add(employmentStatus);
                db.SaveChanges();
                return Json(employmentStatus);
            }
            return GetErrors();
        }

        // POST: EmploymentStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmploymentStatusName,EmploymentStatusDescription,Enabled,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmploymentStatus employmentStatus)
        {
            if (ModelState.IsValid)
            {
                employmentStatus.SetUpdated<EmploymentStatus>();
                db.Entry(employmentStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(EmploymentStatus model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.EmploymentStatus.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.EmploymentStatusName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmploymentStatus.Include(u => u.UserInformations)
                                            .Include(u => u.Employment)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.Employment.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
