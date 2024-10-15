using Newtonsoft.Json;
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
    public class WorkflowLevelController : TimeAideWebControllers<WorkflowLevel>
    {

        public ActionResult IndexByWorkflow(int? id)
        {
            List<WorkflowLevel> model = null;
            var workflow = db.Workflow.FirstOrDefault(w => w.Id == (id ?? 0));
            ViewBag.WorkflowId = id ?? 0;
            if (id.HasValue)
            {
                try
                {
                    ViewBag.WorkflowName = workflow.WorkflowName;
                    ViewBag.IsZeroLevel = workflow.IsZeroLevel;
                    model = workflow.WorkflowLevel.Where(w=>w.DataEntryStatus==1).OrderByDescending(o => o.Id).ToList();
                    foreach (var each in workflow.WorkflowLevel)
                    {
                        each.NotificationMessage = db.NotificationMessage.FirstOrDefault(t => t.Id == each.NotificationMessageId);
                        each.WorkflowLevelType = db.WorkflowLevelType.FirstOrDefault(t => t.Id == each.WorkflowLevelTypeId);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return PartialView("Index", model);
        }
        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        //[ValidateAntiForgeryToken]
        public override void OnCreate()
        {
            SetViewBag();
        }
        public override WorkflowLevel OnEdit(WorkflowLevel workflowLevel)
        {
            workflowLevel.SelectedWorkflowLevelGroupId = String.Join(",", db.WorkflowLevelGroup.Where(w => w.DataEntryStatus == 1 && w.WorkflowLevelId == workflowLevel.Id).Select(c => c.EmployeeGroup.Id.ToString()));
            ViewBag.WorkflowLevelTypeId = new SelectList(db.WorkflowLevelType.Where(w => w.DataEntryStatus == 1), "Id", "WorkflowLevelTypeName", workflowLevel.WorkflowLevelTypeId);
            ViewBag.WorkflowLevelGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
            return workflowLevel;
        }
        private void SetViewBag()
        {
            ViewBag.WorkflowLevelTypeId = new SelectList(db.WorkflowLevelType.Where(w => w.DataEntryStatus == 1), "Id", "WorkflowLevelTypeName");
            ViewBag.WorkflowLevelGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
        }

        [HttpPost]
        public ActionResult Create(WorkflowLevel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {

                    List<string> SubCompanyIds = (model.SelectedWorkflowLevelGroupId ?? "").Split(',').ToList();
                    foreach (var eachId in SubCompanyIds)
                    {
                        int newId;
                        Int32.TryParse(eachId, out newId);
                        if (newId > 0)
                            model.WorkflowLevelGroup.Add(new WorkflowLevelGroup { EmployeeGroupId = Convert.ToInt32(eachId) });
                    }


                    //WorkflowLevel.NotificationMessageId = model.NotificationMessageId;
                    db.WorkflowLevel.Add(model);
                }
                else
                {
                    UpdateUserEmployeeGroups(model);

                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
                string list="";
                try
                {
                    
                    list = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }

                return Json(list);
            }
            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(WorkflowLevel workflowLevel)
        {
            if (ModelState.IsValid)
            {
                workflowLevel.SetUpdated<WorkflowLevel>();
                db.Entry(workflowLevel).State = EntityState.Modified;
                UpdateUserEmployeeGroups(workflowLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        private void UpdateUserEmployeeGroups(WorkflowLevel model)
        {
            var selectedWorkflowLevelGroupIds = (model.SelectedWorkflowLevelGroupId ?? "").Split(',').ToList();
            List<WorkflowLevelGroup> workflowLevelGroupAddList = new List<WorkflowLevelGroup>();
            List<WorkflowLevelGroup> workflowLevelGroupRemoveList = new List<WorkflowLevelGroup>();
            var existingWorkflowLevelGroupList = db.WorkflowLevelGroup.Where(w => w.WorkflowLevelId == model.Id).ToList();

            foreach (var eachExisting in existingWorkflowLevelGroupList)
            {
                var RecCnt = selectedWorkflowLevelGroupIds.Where(w => w == eachExisting.EmployeeGroupId.ToString()).Count();
                if (RecCnt == 0)
                {
                    workflowLevelGroupRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedWorkflowLevelGroupIds)
            {
                if (eachSelected == "") continue;
                int workflowLevelGroupId = int.Parse(eachSelected);
                var recExists = existingWorkflowLevelGroupList.Where(w => w.EmployeeGroupId == workflowLevelGroupId).Count();
                if (recExists == 0)
                {
                    workflowLevelGroupAddList.Add(new WorkflowLevelGroup { EmployeeGroupId = workflowLevelGroupId, WorkflowLevelId = model.Id, EmployeeGroup = db.EmployeeGroup.FirstOrDefault(g => g.Id == workflowLevelGroupId) });
                }
            }

            db.WorkflowLevelGroup.RemoveRange(workflowLevelGroupRemoveList);
            db.WorkflowLevelGroup.AddRange(workflowLevelGroupAddList);
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
