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
    public class ModuleController : TimeAideWebControllers<Module>
    {
        public ModuleController() : base()
        {
            ViewBag.AllowAdd = false;
            ViewBag.AllowDelete = false;
        }
        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Module module)
        {
            if (ModelState.IsValid)
            {
                db.Module.Add(module);
                db.SaveChanges();
                return Json(module);
            }
            return GetErrors();
        }



        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Module module)
        {
            if (ModelState.IsValid)
            {
                module.SetUpdated<Module>();
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(module);
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Module.Include(u => u.Forms)
                                  .Include(u => u.InterfaceControlForms)
                                  .Include(u => u.Module1)
                                  .FirstOrDefault(c => c.Id == id);
            if (entity.Forms.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.InterfaceControlForms.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.Module1.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
