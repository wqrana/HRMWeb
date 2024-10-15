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
    public class WorkflowController : TimeAideWebControllers<Workflow>
    {

        public override List<Workflow> OnIndex(List<Workflow> model)
        {
            foreach (var each in model)
            {
                each.ClosingNotification = db.ClosingNotificationType.FirstOrDefault(t => t.Id == each.ClosingNotificationId);
            }

            return model;
        }

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public override void OnCreate()
        {
            ViewBag.ClosingNotificationId = new SelectList(db.ClosingNotificationType.Where(w => w.DataEntryStatus == 1), "Id", "ClosingNotificationTypeName");
        }
        public override Workflow OnEdit(Workflow workflow)
        {
            ViewBag.ClosingNotificationId = new SelectList(db.ClosingNotificationType.Where(w => w.DataEntryStatus == 1), "Id", "ClosingNotificationTypeName", workflow.ClosingNotificationId);
            return workflow;
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Workflow Workflow)
        {
            if (ModelState.IsValid)
            {
                db.Workflow.Add(Workflow);
                db.SaveChanges();
                return Json(Workflow);
            }

            return GetErrors();
        }

        [HttpPost]
        public JsonResult CreateEdit(Workflow model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            Workflow WorkflowEntity = null;
            try
            {
                var isAlreadyExist = db.Workflow
                                        .Where(w => w.DataEntryStatus == 1 && (w.Id != model.Id) && (w.WorkflowName.ToLower() == model.WorkflowName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Schedule Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        //WorkflowEntity = new Workflow();
                        db.Workflow.Add(model);
                    }
                    else
                    {
                        model.SetUpdated<Workflow>();
                        db.Entry(model).State = EntityState.Modified;
                    }
                    //model.WorkflowName = model.WorkflowName;
                    //model.WorkflowDescription = model.WorkflowDescription;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Workflow Workflow)
        {
            if (ModelState.IsValid)
            {
                Workflow.SetUpdated<Workflow>();
                db.Entry(Workflow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.Workflow.Include(u => u.WorkflowTrigger)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.WorkflowTrigger.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
