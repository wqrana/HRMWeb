
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
    public class ApplicantReferenceSourceController : TimeAideWebControllers<ApplicantReferenceSource>
    {
        // GET: Training
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantReferenceSource applicantReferenceSource)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantReferenceSource.Add(applicantReferenceSource);
                db.SaveChanges();
                return Json(applicantReferenceSource);
            }

            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ApplicantReferenceSource model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ApplicantReferenceSource.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.ReferenceSourceName });
        }
        // POST: Training/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantReferenceSource applicantReferenceSource)
        {
            // ApplicantReferenceType updateReferenceTypeEntity = null;
            if (ModelState.IsValid)
            {
                
                applicantReferenceSource.ModifiedBy = SessionHelper.LoginId;
                applicantReferenceSource.ModifiedDate = DateTime.Now;
                db.Entry(applicantReferenceSource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            //return PartialView(training);
            return GetErrors();
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