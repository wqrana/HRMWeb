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
    public class ApplicantStatusController : TimeAideWebControllers<ApplicantStatus>
    {
        // GET: Training
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantStatus applicantStatus)
        {
            if (ModelState.IsValid)
            {
                if (applicantStatus.UseAsHire == true)
                {
                    var existingUseAsHireEntity = db.GetAll<ApplicantStatus>(applicantStatus.ClientId??0).Where(w => w.UseAsHire == true).FirstOrDefault();

                    if (existingUseAsHireEntity != null)
                    {
                        existingUseAsHireEntity.UseAsHire = false;
                        existingUseAsHireEntity.ModifiedBy = SessionHelper.LoginId;
                        existingUseAsHireEntity.ModifiedDate = DateTime.Now;
                    }
                }
                applicantStatus.UseAsApply = applicantStatus.UserAsApplyNP;
                if (applicantStatus.UseAsApply == true)
                {
                    var existingUseAsApplyEntity = db.GetAll<ApplicantStatus>(applicantStatus.ClientId ?? 0).Where(w => w.UseAsApply == true).FirstOrDefault();

                    if (existingUseAsApplyEntity != null)
                    {
                        existingUseAsApplyEntity.UseAsApply = false;
                        existingUseAsApplyEntity.ModifiedBy = SessionHelper.LoginId;
                        existingUseAsApplyEntity.ModifiedDate = DateTime.Now;
                    }
                }

                db.ApplicantStatus.Add(applicantStatus);
                db.SaveChanges();               
                return Json(applicantStatus);
            }
           
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ApplicantStatus model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ApplicantStatus.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.ApplicantStatusName });
        }
        // POST: Training/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantStatus applicantStatus)
        {
            ApplicantStatus updateApplicantStatusEntity = null;
            if (ModelState.IsValid)
            {
                updateApplicantStatusEntity = db.ApplicantStatus.Find(applicantStatus.Id);
                if (applicantStatus.UseAsHire == true)
                {
                    var existingUseAsHireEntity = db.GetAll<ApplicantStatus>(applicantStatus.ClientId ?? 0).Where(w => w.UseAsHire == true && w.Id!= applicantStatus.Id).FirstOrDefault();

                    if (existingUseAsHireEntity != null)
                    {
                        
                            existingUseAsHireEntity.UseAsHire = false;
                            existingUseAsHireEntity.ModifiedBy = SessionHelper.LoginId;
                            existingUseAsHireEntity.ModifiedDate = DateTime.Now;
                      
                    }
                }
                applicantStatus.UseAsApply = applicantStatus.UserAsApplyNP;
                if (applicantStatus.UseAsApply == true)
                {
                    var existingUseAsApplyEntity = db.GetAll<ApplicantStatus>(applicantStatus.ClientId ?? 0).Where(w => w.UseAsApply == true).FirstOrDefault();

                    if (existingUseAsApplyEntity != null)
                    {
                        existingUseAsApplyEntity.UseAsApply = false;
                        existingUseAsApplyEntity.ModifiedBy = SessionHelper.LoginId;
                        existingUseAsApplyEntity.ModifiedDate = DateTime.Now;
                    }
                }
                updateApplicantStatusEntity.ApplicantStatusName = applicantStatus.ApplicantStatusName;
                updateApplicantStatusEntity.Description = applicantStatus.Description;
                updateApplicantStatusEntity.UseAsApply = applicantStatus.UseAsApply;
                updateApplicantStatusEntity.UseAsHire = applicantStatus.UseAsHire;
                updateApplicantStatusEntity.ModifiedBy = SessionHelper.LoginId;
                updateApplicantStatusEntity.ModifiedDate = DateTime.Now;

                //applicantStatus.ModifiedBy = SessionHelper.LoginId;
                //applicantStatus.ModifiedDate = DateTime.Now;
                //db.Entry(applicantStatus).State = EntityState.Modified;
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