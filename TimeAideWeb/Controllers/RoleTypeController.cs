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
    public class RoleTypeController : TimeAideWebControllers<RoleType>
    {
        public RoleTypeController() : base()
        {
            ViewBag.AllowAdd = false;
            ViewBag.AllowDelete = false;
            ViewBag.AllowEdit = false;
        }
        // POST: RoleType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,RoleTypeName,CompanyId,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] RoleType roleType)
        {
            if (ModelState.IsValid)
            {
                db.RoleType.Add(roleType);
                db.SaveChanges();
                return Json(roleType);
            }
            return GetErrors();
        }
        

        // POST: RoleType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleTypeName,CompanyId,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] RoleType roleType)
        {
            if (ModelState.IsValid)
            {
                roleType.SetUpdated<RoleType>();
                db.Entry(roleType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var roleType = db.RoleType.Include(u => u.Role)
                         .Include(u => u.RoleTypeFormPrivilege)
                         .FirstOrDefault(c => c.Id == id);
            if (roleType.Role.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (roleType.RoleTypeFormPrivilege.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
