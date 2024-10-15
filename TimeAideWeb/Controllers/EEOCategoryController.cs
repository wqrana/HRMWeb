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
    public class EEOCategoryController : TimeAideWebControllers<EEOCategory>
    {
       
        // POST: EEOCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EEOCategory eEOCategory)
        {
            if (ModelState.IsValid)
            {
                db.EEOCategory.Add(eEOCategory);
                db.SaveChanges();
                return Json(eEOCategory);
            }

            return GetErrors();
        }
        
        // POST: EEOCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EEOCategory eEOCategory)
        {
            if (ModelState.IsValid)
            {
                eEOCategory.SetUpdated<EEOCategory>();
                db.Entry(eEOCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(EEOCategory model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.EEOCategory.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.EEOCategoryName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EEOCategory.Include(u => u.PayInformationHistory)
                                       .Include(u => u.Position)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.PayInformationHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.Position.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
