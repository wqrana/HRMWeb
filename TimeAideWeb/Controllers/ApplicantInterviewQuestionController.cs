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
    public class ApplicantInterviewQuestionController : TimeAideWebControllers<ApplicantInterviewQuestion>
    {
        // GET: ActionType
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantInterviewQuestion model)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantInterviewQuestion.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            //return PartialView(model);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(ApplicantInterviewQuestion model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.ApplicantInterviewQuestion.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.QuestionName });
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicantInterviewQuestion model)
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
        public ActionResult ApplicantQAnswerIndex(int id)
        {
            
            var model = db.ApplicantInterviewQuestion.Find(id);
            return PartialView(model);
        }
        public ActionResult ApplicantQAnswerList(int id)
        {
          var model=  db.ApplicantQAnswerOption.Where(w => w.ApplicantInterviewQuestionId == id && w.DataEntryStatus == 1).OrderBy(o=>o.Id);
          return PartialView(model);
        }
        public ActionResult ApplicantQAnswerCreateEdit(int id)
        {
            var model = db.ApplicantQAnswerOption.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult ApplicantQAnswerCreateEdit(ApplicantQAnswerOption model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated.";

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == 0)
                    {
                        db.ApplicantQAnswerOption.Add(model);
                    }
                    else
                    {
                        model.ModifiedBy = SessionHelper.LoginId;
                        model.ModifiedDate = DateTime.Now;
                        db.Entry(model).State = EntityState.Modified;
                    }
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

            return Json(new { status = status, message = message});

        }
        public ActionResult ApplicantQAnswerDelete(int id)
        {
            var model = db.ApplicantQAnswerOption.Find(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ApplicantQAnswerConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var model = db.ApplicantQAnswerOption.Find(id);
            try
            {
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                model.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
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

