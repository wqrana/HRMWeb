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
    public class CustomFieldController : TimeAideWebControllers<CustomField>
    {

        // POST: Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(CustomField customField)
        {
            if (ModelState.IsValid)
            {
                var CustomFields = db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (CustomFields.Count > 0 && CustomFields.Select(c => c.FieldDisplayOrder).Contains(customField.FieldDisplayOrder))
                {
                    ModelState.AddModelError("FieldDisplayOrder", "Field Display Order must be unique, This order key is aleady in system");
                }
            }
            if (ModelState.IsValid)
            {
                db.CustomField.Add(customField);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return GetErrors();
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CustomField customField)
        {
            int id = customField.Id;
            if (ModelState.IsValid)
            {
                var CustomFields = (new TimeAideContext()).GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (CustomFields.Count > 0 && CustomFields.Where(c => c.Id != id).Select(c => c.FieldDisplayOrder).Contains(customField.FieldDisplayOrder))
                {
                    ModelState.AddModelError("FieldDisplayOrder", "Field Display Order must be unique, This order key is aleady in system");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(customField).State = EntityState.Modified;
                customField.SetUpdated<CustomField>();
                db.Entry(customField).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.CustomField.Include(u => u.EmployeeCustomField)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeeCustomField.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;

        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(CustomField model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.CustomField.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.CustomFieldName });
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
