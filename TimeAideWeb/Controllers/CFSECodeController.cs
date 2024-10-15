using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.DAL.UnitOfWork;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class CFSECodeController : TimeAideWebControllers<CFSECode>
    {
        // POST: CFSECode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CFSECodeDescription,CFSECodeName,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] CFSECode cFSECode)
        {
            if (ModelState.IsValid)
            {
                db.CFSECode.Add(cFSECode);
                db.SaveChanges();
                return Json(cFSECode);
            }

            return GetErrors();
        }

        // POST: CFSECode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CFSECodeDescription,CFSECodeName")] CFSECode cFSECode)
        {
            if (ModelState.IsValid)
            {
                cFSECode.SetUpdated<CFSECode>();
                db.Entry(cFSECode).State = EntityState.Modified;
                db.SaveChanges();
                return Json(cFSECode);
            }
            return View(cFSECode);
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.CFSECode.Include(u => u.Departments)
                                      .Include(u => u.SubDepartments)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.Departments.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.SubDepartments.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
