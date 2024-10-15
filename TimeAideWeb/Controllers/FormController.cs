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
    public class FormController : TimeAideWebControllers<Form>
    {
        public FormController() : base()
        {
            ViewBag.AllowAdd = false;
            ViewBag.AllowDelete = false;
        }
        // POST: Form/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ModuleId,FormName,Url,Label,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Form form)
        {
            if (ModelState.IsValid)
            {
                //Int32? maxId = db.Form.Max(f => f.Id);
                //form.Id = (maxId ?? 0) + 1;
                db.Add<Form>(form);
                db.SaveChanges();
                return Json(form);
            }

            return GetErrors();
        }

        // POST: Form/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ModuleId,FormName,Url,Label,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Form form)
        {
            if (ModelState.IsValid)
            {
                form.SetUpdated<Form>();
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return Json(form);
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Form.Include(u => u.RoleFormPrivileges)
                                .Include(u => u.RoleTypeFormPrivilege)
                                .Include(u => u.InterfaceControlForms)
                                .Include(u => u.UserMenus)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.RoleFormPrivileges.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.RoleTypeFormPrivilege.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.InterfaceControlForms.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.UserMenus.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
