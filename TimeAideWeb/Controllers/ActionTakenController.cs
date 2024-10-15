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
    public class ActionTakenController : TimeAideWebControllers<ActionTaken>
    {
        // GET: ActionTaken
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Create(ActionTaken model)
        {
            if (ModelState.IsValid)
            {
                db.ActionTaken.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            //return PartialView(model);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ActionTaken model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ActionTaken.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.ActionTakenName });
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ActionTaken model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(model);
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.ActionTaken.Include(u => u.EmployeePerformance)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.EmployeePerformance.Where(t => t.DataEntryStatus == 1).Count() > 0)
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