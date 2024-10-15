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
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class EmergencyContactController : Controller
    {
        public TimeAideContext db = new TimeAideContext();

        string FormName
        {
            get;
            set;
        }
        RoleFormPrivilegeViewModel1 privileges;
        public EmergencyContactController()
        {
            FormName = typeof(EmergencyContact).Name;
            var form = db.Form.FirstOrDefault(p => p.FormName == FormName && p.DataEntryStatus == 1);
            if (form == null)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, IsFormDeleted = true };

            if (SecurityHelper.IsSuperAdmin)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = true, AllowDelete = true, AllowEdit = true, AllowView = true, AllowChangeHistory = true };
            else
            {
                var userRole = db.UserInformationRole.FirstOrDefault(p => p.UserInformationId == SessionHelper.LoginId);
                if (userRole == null)
                    privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = true };
                else
                {
                    var roleFormPrivilege = db.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == form.FormName).ToList();
                    privileges = (new RoleFormPrivilegeService()).GetView(roleFormPrivilege, form, userRole.RoleId);
                }
            }
            ViewBag.AllowEdit = privileges.AllowEdit;
            ViewBag.AllowAdd = privileges.AllowAdd;
            ViewBag.AllowView = privileges.AllowView;
            ViewBag.AllowDelete = privileges.AllowDelete;
            ViewBag.AllowChangeHistory = privileges.AllowChangeHistory;
            ViewBag.FormName = FormName;
            ViewBag.Title = UtilityHelper.Pluralize(FormName);
            ViewBag.Label = form.Label;
            ViewBag.LabelPlural = form.LabelPlural;
        }

        // GET: ContactPerson
        public ActionResult Index(int? id)
        {
            var model = db.UserInformation.FirstOrDefault(u => u.Id == id.Value);
            return PartialView(model);
        }

        // GET: ContactPerson/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact contactPerson = db.EmergencyContact.Find(id);
            if (contactPerson == null)
            {
                return HttpNotFound();
            }
            return View(contactPerson);
        }

        // GET: ContactPerson/Create
        public ActionResult Create(int? id)
        {
            EmergencyContact emergencyContact = db.EmergencyContact.FirstOrDefault(u => u.UserInformationId == id && u.DataEntryStatus != 0);
            if (emergencyContact == null)
            {
                emergencyContact = new EmergencyContact();
                ViewBag.Label = ViewBag.Label + " - Add";
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit";
            }

            EmergencyContact address = new EmergencyContact();
            address.UserInformationId = id;
            ViewBag.RelationshipId = new SelectList(db.Relationship, "Id", "RelationshipName");
            return PartialView(address);
        }

        // POST: ContactPerson/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserInformationId,RelationshipId,RelationshipName,ContactPersonName,IsDefault,MainNumber,AlternateNumber")] EmergencyContact emergencyContact)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(emergencyContact.MainNumber) && emergencyContact.MainNumber.Length < 10)
                {
                    ModelState.AddModelError("MainNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(emergencyContact.AlternateNumber) && emergencyContact.AlternateNumber.Length < 10)
                {
                    ModelState.AddModelError("AlternateNumber", "Invalid format.");
                }
            }
            if (ModelState.IsValid)
            {
                //var relationship = db.Find<Relationship>(emergencyContact.RelationshipId ?? 0);
                //if (relationship != null)
                //    emergencyContact.RelationshipName = relationship.RelationshipName;
                db.EmergencyContact.Add(emergencyContact);
                db.SaveChanges();

                //return Json(emergencyContact);
                //return PartialView("Index");
                return RedirectToAction("Index", new { id = emergencyContact.UserInformationId });
            }
            return GetErrors();
        }

        // GET: ContactPerson/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
            if (emergencyContact == null)
            {
                return HttpNotFound();
            }
            ViewBag.RelationshipId = new SelectList(db.Relationship, "Id", "RelationshipName", emergencyContact.RelationshipId);
            return PartialView(emergencyContact);
        }

        // POST: ContactPerson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,UserInformationId,RelationshipId,RelationshipName,ContactPersonName,IsDefault,MainNumber,AlternateNumber,CreatedBy,CreatedDate,DataEntryStatus")] EmergencyContact emergencyContact)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(emergencyContact.MainNumber) && emergencyContact.MainNumber.Length < 10)
                {
                    ModelState.AddModelError("MainNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(emergencyContact.AlternateNumber) && emergencyContact.AlternateNumber.Length < 10)
                {
                    ModelState.AddModelError("AlternateNumber", "Invalid format.");
                }
            }
            if (ModelState.IsValid)
            {

                emergencyContact.ModifiedBy = SessionHelper.LoginId;
                emergencyContact.ModifiedDate = DateTime.Now;
                db.Entry(emergencyContact).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = emergencyContact.UserInformationId });
            }

            return GetErrors();
        }

        // GET: ContactPerson/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
            if (emergencyContact == null)
            {
                return HttpNotFound();
            }
            return PartialView(emergencyContact);
        }

        // POST: ContactPerson/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
            //db.EmergencyContact.Remove(emergencyContact);
            emergencyContact.ModifiedBy = SessionHelper.LoginId;
            emergencyContact.ModifiedDate = DateTime.Now;
            emergencyContact.DataEntryStatus = 0;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = emergencyContact.UserInformationId });
        }

        [HttpPost]
        public ActionResult ChangeRequestEmergencyContact(EmergencyContact model)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(model.MainNumber) && model.MainNumber.Length < 10)
                {
                    ModelState.AddModelError("MainNumber", "Invalid format.");
                }
                if (!String.IsNullOrEmpty(model.AlternateNumber) && model.AlternateNumber.Length < 10)
                {
                    ModelState.AddModelError("AlternateNumber", "Invalid format.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    EmergencyContact dbObject = null;
                    if (model.Id > 0)
                    {
                        dbObject = db.Find<EmergencyContact>(model.Id, SessionHelper.SelectedClientId);
                        if (dbObject.ChangeRequestEmergencyContact.Count > 0 && dbObject.ChangeRequestEmergencyContact.Any(r => r.ChangeRequestStatusId == 1 && r.RequestTypeId == model.RequestTypeId))
                        {
                            Dictionary<String, String> errors = new Dictionary<string, string>();
                            errors.Add("PopupMessage1", "There is already a pending change request for selcted emplyee.");
                            return GetErrors(errors);
                        }
                    }
                    else
                        dbObject = new EmergencyContact();
                    ChangeRequestEmergencyContact changeRequest = new ChangeRequestEmergencyContact();
                    changeRequest = GetChanges(model, dbObject);
                    db.ChangeRequestEmergencyContact.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 4);
                    workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmergencyContact = changeRequest;
                    if (workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow.IsZeroLevel)
                    {
                        changeRequest.ChangeRequestStatusId = 2;
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestEmergencyContact);
                        db.SaveChanges();
                        //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId ?? 0, SessionHelper.SelectedClientId);
                        //Dictionary<string, string> toEmails = userInformationList.Where(u=>!string.IsNullOrEmpty(u.LoginEmail)).ToDictionary(v => v.LoginEmail, k => k.ShortFullName);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId.Value, changeRequest.Id, workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow, workflowTriggerRequestDetail, toEmails, db);
                        WorkflowService.ProcesstWorkflowClosingNotification(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, changeRequest);
                    }
                    else
                    {
                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(model.UserInformationId.Value, workflowTriggerRequestDetail, changeRequest.Id, db);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    return Json(model);
                }
                catch (Exception ex)
                {
                }

            }
            return GetErrors();
        }

        //
        [HttpPost]
        public ActionResult ChangeRequestDeleteEmergencyContact(EmergencyContact model)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.ReasonForDelete))
                {
                    ModelState.AddModelError("ReasonForDelete", "Reason For Delete is required.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    EmergencyContact dbObject = db.Find<EmergencyContact>(model.Id, SessionHelper.SelectedClientId);
                    if (dbObject.ChangeRequestEmergencyContact.Count > 0 && dbObject.ChangeRequestEmergencyContact.Any(r => r.ChangeRequestStatusId == 1 && r.RequestTypeId == model.RequestTypeId))
                    {
                        Dictionary<String, String> errors = new Dictionary<string, string>();
                        errors.Add("PopupMessage1", "There is already a pending change request for selcted emplyee.");
                        return GetErrors(errors);
                    }
                    ChangeRequestEmergencyContact changeRequest = new ChangeRequestEmergencyContact();
                    changeRequest.UserInformationId = model.UserInformationId.Value;
                    changeRequest.EmergencyContactId = dbObject.Id;
                    changeRequest.ChangeRequestStatusId = 1;
                    changeRequest.RequestTypeId = model.RequestTypeId;
                    changeRequest.ReasonForDelete = model.ReasonForDelete;
                    db.ChangeRequestEmergencyContact.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 4);
                    workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmergencyContact = changeRequest;
                    if (workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow.IsZeroLevel)
                    {
                        changeRequest.ChangeRequestStatusId = 2;
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestEmergencyContact);
                        db.SaveChanges();
                        //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId ?? 0, SessionHelper.SelectedClientId);
                        //Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId.Value, changeRequest.Id, workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow, workflowTriggerRequestDetail, toEmails, db);
                        WorkflowService.ProcesstWorkflowClosingNotification(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, changeRequest);
                    }
                    else
                    {
                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(model.UserInformationId.Value, workflowTriggerRequestDetail, changeRequest.Id, db);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    return Json(model);
                }
                catch (Exception ex)
                {
                }

            }
            return GetErrors();
        }
        private ChangeRequestEmergencyContact GetChanges(EmergencyContact model, EmergencyContact dbObject)
        {
            ChangeRequestEmergencyContact changeRequest = new ChangeRequestEmergencyContact();
            changeRequest.UserInformationId = model.UserInformationId.Value;
            if (dbObject.Id > 0)
                changeRequest.EmergencyContactId = dbObject.Id;
            changeRequest.ChangeRequestStatusId = 1;
            changeRequest.RequestTypeId = model.RequestTypeId;
            if (dbObject.AlternateNumber != model.AlternateNumber)
            {
                changeRequest.NewAlternateNumber = model.AlternateNumber;
                changeRequest.AlternateNumber = dbObject.AlternateNumber;
            }
            if (dbObject.MainNumber != model.MainNumber)
            {
                changeRequest.NewMainNumber = model.MainNumber;
                changeRequest.MainNumber = dbObject.MainNumber;
            }
            if (dbObject.ContactPersonName != model.ContactPersonName)
            {
                changeRequest.NewContactPersonName = model.ContactPersonName;
                changeRequest.ContactPersonName = dbObject.ContactPersonName;
            }
            if (dbObject.RelationshipId != model.RelationshipId)
            {
                changeRequest.NewRelationshipId = model.RelationshipId;
                changeRequest.RelationshipId = dbObject.RelationshipId;
            }
            if (dbObject.IsDefault != model.IsDefault)
            {
                changeRequest.NewIsDefault = model.IsDefault;
                changeRequest.IsDefault = dbObject.IsDefault;
            }
            if (dbObject.RelationshipName != model.RelationshipName)
            {
                changeRequest.NewRelationshipName = model.RelationshipName;
                changeRequest.RelationshipName = dbObject.RelationshipName;
            }
            return changeRequest;
        }

        protected ActionResult GetErrors(Dictionary<String, String> errorList)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in errorList)
            {
                if (eachState.Value != null)
                {
                    errors.Add(eachState.Key, eachState.Value);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }

        protected ActionResult GetErrors()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in ModelState)
            {
                if (eachState.Value != null && eachState.Value.Errors != null && eachState.Value.Errors.Count > 0)
                {
                    errors.Add(eachState.Key, eachState.Value.Errors[0].ErrorMessage);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }

        public ActionResult EmergencyContactChangeRequest(int? id)
        {
            EmergencyContact emergencyContact = null;
            if (id == -1)
            {
                emergencyContact = new EmergencyContact();

                ViewBag.EmergencyContactTitle = "Add new Emergency Contact";
                ViewBag.EmergencyContactStyle = "width:100%;";
            }
            else
            {
                emergencyContact = db.EmergencyContact.FirstOrDefault(u => u.Id == id && u.DataEntryStatus != 0);

                ViewBag.EmergencyContactTitle = "New Emergency Contact";
                ViewBag.EmergencyContactStyle = "width:49%;margin-left:10px";
            }
            ViewBag.CanEmergencyContactWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(SessionHelper.LoginId, 4);
            return PartialView("~/Views/UserInformation/EmployeeProfile/EmergencyContact/_EmergencyContactChangeRequest.cshtml", emergencyContact);
        }

        public ActionResult EmergencyContactChangeRequestDelete(int? id)
        {
            EmergencyContact emergencyContact = null;
            if (id == -1)
            {
                emergencyContact = new EmergencyContact();

                ViewBag.EmergencyContactTitle = "Add new Emergency Contact";
                ViewBag.EmergencyContactStyle = "width:100%;";
            }
            else
            {
                emergencyContact = db.EmergencyContact.FirstOrDefault(u => u.Id == id && u.DataEntryStatus != 0);

                ViewBag.EmergencyContactTitle = "New Emergency Contact";
                ViewBag.EmergencyContactStyle = "width:49%;margin-left:10px";
            }
            ViewBag.CanEmergencyContactWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(SessionHelper.LoginId, 4);
            return PartialView("~/Views/UserInformation/EmployeeProfile/EmergencyContact/_EmergencyContactDeleteView.cshtml", emergencyContact);
        }

        public virtual ActionResult ChangeHistory(int refrenceId)
        {
            try
            {
                AllowView();
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == "EmergencyContact" && d.ClientId == SessionHelper.SelectedClientId).ToList();
                if (entitySet.Count > 0)
                {
                    //string tablename = entitySet.FirstOrDefault().AuditLog.TableName;
                    //int referenceId = entitySet.FirstOrDefault().AuditLog.ReferenceId;
                    //var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + tablename + " where " + tablename + "Id = " + referenceId.ToString() + "").FirstOrDefault();
                    //if (refrenceObject != null)
                    //{
                    //    ViewBag.ReferenceObject = refrenceObject;
                    //    ViewBag.TableName = tablename;
                    //    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                    //}
                }
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from EmergencyContact where EmergencyContactId = " + refrenceId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = "EmergencyContact";
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Emergency Contact", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public void AllowDelete()
        {
            if (!privileges.AllowDelete)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowEdit()
        {
            if (!privileges.AllowEdit)
            {
                throw new AuthorizationException();
            }
        }
        public virtual void AllowAdd()
        {
            if (!privileges.AllowAdd)
            {
                throw new AuthorizationException();
            }
        }
        public virtual void AllowView()
        {
            if (!privileges.AllowView)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowChangeHistory()
        {
            if (!privileges.AllowChangeHistory)
            {
                throw new AuthorizationException();
            }
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
