using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class TrainingController : TimeAideWebControllers<Training>
    {
        // GET: Training
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Training training)
        {
            if (ModelState.IsValid)
            {
                db.Training.Add(training);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(training);
            }

            //return PartialView(training);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Training model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Training.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.TrainingName });
        }
        // POST: Training/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Training training)
        {
            if (ModelState.IsValid)
            {
                training.ModifiedBy = SessionHelper.LoginId;
                training.ModifiedDate = DateTime.Now;
                db.Entry(training).State = EntityState.Modified;
                db.SaveChanges();
                 return RedirectToAction("Index");
               
            }
            //return PartialView(training);
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            StringBuilder str = new StringBuilder();
            var training = db.Training.Include(u => u.EmployeeTraining)
                         .FirstOrDefault(c => c.Id == id);
            if (training.EmployeeTraining.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            else if(db.PositionTraining.Where(w=>w.TrainingId==id && w.DataEntryStatus==1).Count()>0)
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