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
    public class WorkflowTriggerController : TimeAideWebControllers<WorkflowTrigger>
    {

        
        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //[ValidateAntiForgeryToken]
        public override void OnCreate()
        {
            SetViewBag();
        }
        public override WorkflowTrigger OnEdit(WorkflowTrigger WorkflowTrigger)
        {
            //WorkflowTrigger.SelectedWorkflowTriggerGroupId = String.Join(",", db.WorkflowTriggerGroup.Where(w => w.DataEntryStatus == 1 && w.WorkflowTriggerId == WorkflowTrigger.Id).Select(c => c.EmployeeGroup.Id.ToString()));
            ViewBag.WorkflowTriggerTypeId = new SelectList(db.WorkflowTriggerType.Where(w => w.DataEntryStatus == 1), "Id", "WorkflowTriggerTypeName", WorkflowTrigger.WorkflowTriggerTypeId);
            ViewBag.WorkflowId = new SelectList(db.GetAllByCompany<Workflow>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "WorkflowName", WorkflowTrigger.WorkflowId);
            return WorkflowTrigger;
        }
        private void SetViewBag()
        {
            ViewBag.WorkflowTriggerTypeId = new SelectList(db.WorkflowTriggerType.Where(w => w.DataEntryStatus == 1), "Id", "WorkflowTriggerTypeName");
            ViewBag.WorkflowId = new SelectList(db.GetAllByCompany<Workflow>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "WorkflowName");
        }

        //public override List<WorkflowTrigger> OnIndex(List<WorkflowTrigger> model)
        //{
        //    foreach (var each in model)
        //    {
        //        each.Workflow = db.Workflow.FirstOrDefault(t => t.Id == each.WorkflowId);
        //    }

        //    return model;
        //}

        [HttpPost]
        public ActionResult Create(WorkflowTrigger model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {

                    //WorkflowTrigger = new WorkflowTrigger();
                    //WorkflowTrigger.WorkflowId = model.WorkflowId;
                    //WorkflowTrigger.WorkflowTriggerName = model.WorkflowTriggerName;
                    //WorkflowTrigger.WorkflowTriggerTypeId = model.WorkflowTriggerTypeId;

                    //List<string> SubCompanyIds = (model.SelectedWorkflowTriggerGroupId ?? "").Split(',').ToList();
                    //foreach (var eachId in SubCompanyIds)
                    //{
                    //    int newId;
                    //    Int32.TryParse(eachId, out newId);
                    //    if (newId > 0)
                    //        model.WorkflowTriggerGroup.Add(new WorkflowTriggerGroup { EmployeeGroupId = Convert.ToInt32(eachId) });
                    //}


                    //WorkflowTrigger.NotificationMessageId = model.NotificationMessageId;
                    db.WorkflowTrigger.Add(model);
                }
                else
                {
                    //WorkflowTrigger = db.WorkflowTrigger.Find(model.Id);
                    //WorkflowTrigger.WorkflowId = model.WorkflowId;
                    //WorkflowTrigger.WorkflowTriggerName = model.WorkflowTriggerName;
                    //WorkflowTrigger.WorkflowTriggerTypeId = model.WorkflowTriggerTypeId;

                    //List<string> SubCompanyIds = (model.SelectedWorkflowTriggerGroupId ?? "").Split(',').ToList();
                    //foreach (var eachId in SubCompanyIds)
                    //{
                    //    int newId;
                    //    Int32.TryParse(eachId, out newId);
                    //    if (newId > 0)
                    //        model.WorkflowTriggerGroup.Add(new WorkflowTriggerGroup { EmployeeGroupId = Convert.ToInt32(eachId) });
                    //}

                    db.Entry(model).State = EntityState.Modified;
                    //WorkflowTrigger.NotificationMessageId = model.NotificationMessageId;
                    //db.WorkflowTrigger.Add(WorkflowTrigger);
                }
                db.SaveChanges();
                return Json(model);
            }
            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(WorkflowTrigger WorkflowTrigger)
        {
            if (ModelState.IsValid)
            {
                WorkflowTrigger.SetUpdated<WorkflowTrigger>();
                db.Entry(WorkflowTrigger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.NotificationMessage.Include(u => u.UserInformations)
            //                                .Include(u => u.EmploymentHistory)
            //                                .Include(u => u.SupervisorEmployeeType)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.SupervisorEmployeeType.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
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
