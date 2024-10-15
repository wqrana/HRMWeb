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
    public class ApplicantInterviewAnswerController : TimeAideWebControllers<ApplicantInterviewAnswer>
    {
        // GET: ActionType
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantInterviewAnswer model)
        {
            if (ModelState.IsValid)
            {
                if (model.AnswerMaxValue >= model.AnswerValue)
                {

                    db.ApplicantInterviewAnswer.Add(model);
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                    return Json(model);
                }
                else
                {
                    ModelState.AddModelError("AnswerValue", "Anwser value shouldn't be greater than Max Value Limit.");
                }
            }

            //return PartialView(model);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ApplicantInterviewAnswer model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ApplicantInterviewAnswer.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.AnswerName });
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantInterviewAnswer model)
        {
            if (ModelState.IsValid)
            {
                if (model.AnswerMaxValue >= model.AnswerValue)
                {
                    model.ModifiedBy = SessionHelper.LoginId;
                    model.ModifiedDate = DateTime.Now;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("AnswerValue", "Anwser value shouldn't be greater than Max Value Limit.");
                }
            }
            // return PartialView(model);
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

