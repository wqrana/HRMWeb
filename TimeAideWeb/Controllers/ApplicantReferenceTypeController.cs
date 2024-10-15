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
    public class ApplicantReferenceTypeController : TimeAideWebControllers<ApplicantReferenceType>
    {
        // GET: Training
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantReferenceType applicantReferenceType)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantReferenceType.Add(applicantReferenceType);
                db.SaveChanges();
                return Json(applicantReferenceType);
            }

            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ApplicantReferenceType model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ApplicantReferenceType.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.ReferenceTypeName });
        }
        // POST: Training/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantReferenceType applicantReferenceType)
        {
           // ApplicantReferenceType updateReferenceTypeEntity = null;
            if (ModelState.IsValid)
            {
                //updateApplicantStatusEntity.ApplicantStatusName = applicantStatus.ApplicantStatusName;
                //updateApplicantStatusEntity.Description = applicantStatus.Description;
                //updateApplicantStatusEntity.UseAsHire = applicantStatus.UseAsHire;
                //updateApplicantStatusEntity.ModifiedBy = SessionHelper.LoginId;
                //updateApplicantStatusEntity.ModifiedDate = DateTime.Now;

                applicantReferenceType.ModifiedBy = SessionHelper.LoginId;
                applicantReferenceType.ModifiedDate = DateTime.Now;
                db.Entry(applicantReferenceType).State = EntityState.Modified;
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