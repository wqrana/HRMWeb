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
    public class PrivilegeController : TimeAideWebControllers<Privilege>
    {
        public PrivilegeController() : base()
        {
            ViewBag.AllowAdd = false;
            ViewBag.AllowDelete = false;
            ViewBag.AllowEdit = false;
        }
        // POST: Privilege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PrivilegeName,Code,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                db.Privilege.Add(privilege);
                db.SaveChanges();
                return Json(privilege);
            }

            return GetErrors();
        }

        // POST: Privilege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PrivilegeName,Code,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                privilege.ModifiedBy = SessionHelper.LoginId;
                privilege.ModifiedDate = DateTime.Now;
                db.Entry(privilege).State = EntityState.Modified;
                db.SaveChanges();
                return Json(privilege);
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var privilege = db.Privilege.Include(u => u.InterfaceControlForms)
                         .Include(u => u.RoleInterfaceControlPrivileges)
                         .Include(u => u.RoleFormPrivileges)
                         .Include(u => u.RoleTypeFormPrivileges)
                         .FirstOrDefault(c => c.Id == id);
            if (privilege.InterfaceControlForms.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (privilege.RoleInterfaceControlPrivileges.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (privilege.RoleFormPrivileges.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (privilege.RoleTypeFormPrivileges.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
