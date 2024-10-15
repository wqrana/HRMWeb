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
    public class EmployeeVeteranStatusController : TimeAideWebControllers<EmployeeVeteranStatus>
    {
        // POST: EmployeeVeteranStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserInformationId,VeteranStatusId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeVeteranStatus employeeVeteranStatus)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeVeteranStatus.Add(employeeVeteranStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserInformationId = new SelectList(db.UserInformation, "UserInformationId", "IdNumber", employeeVeteranStatus.UserInformationId);
            ViewBag.VeteranStatusId = new SelectList(db.VeteranStatus, "VeteranStatusId", "VeteranStatusName", employeeVeteranStatus.VeteranStatusId);
            return View(employeeVeteranStatus);
        }

        // POST: EmployeeVeteranStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserInformationId,VeteranStatusId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeVeteranStatus employeeVeteranStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeVeteranStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInformationId = new SelectList(db.UserInformation, "UserInformationId", "IdNumber", employeeVeteranStatus.UserInformationId);
            ViewBag.VeteranStatusId = new SelectList(db.VeteranStatus, "VeteranStatusId", "VeteranStatusName", employeeVeteranStatus.VeteranStatusId);
            return View(employeeVeteranStatus);
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
