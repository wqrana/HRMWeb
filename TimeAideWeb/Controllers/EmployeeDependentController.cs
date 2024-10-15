using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeDependentController : TimeAideWebControllers<EmployeeDependent>
    {
        [HttpGet]
        // GET: EmployeeDependent
        public ActionResult IndexByUser(int id)
        {
            var model = db.EmployeeDependent.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
        public ActionResult IndexByUserInsurance(int id, string insuranceType)
        {
            var model = db.EmployeeDependent.Where(w => w.DataEntryStatus == 1
                                                        && w.UserInformationId == id
                                                        && (insuranceType == "H" ? w.IsHealthInsurance : w.IsDentalInsurance))
                                                        .OrderByDescending(o => o.Id);
            return PartialView("IndexByUserInsurance", model);
        }
        public override ActionResult Create()
        {
            //var model = new EmployeeEducation();
            // ViewBag.DegreeId = new SelectList(db.Degree.Where(w => w.DataEntryStatus == 1), "Id", "DegreeName");
            ViewBag.RelationshipId = new SelectList(db.GetAll<Relationship>(SessionHelper.SelectedClientId), "Id", "RelationshipName");
            ViewBag.DependentStatusId = new SelectList(db.GetAll<DependentStatus>(SessionHelper.SelectedClientId), "Id", "StatusName");
            ViewBag.GenderId = new SelectList(db.GetAll<Gender>(SessionHelper.SelectedClientId), "Id", "GenderName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeDependent.Where(w => w.Id == id).FirstOrDefault();
            // ViewBag.DegreeId = new SelectList(db.Degree.Where(w => w.DataEntryStatus == 1), "Id", "DegreeName", model.DegreeId);
            ViewBag.RelationshipId = new SelectList(db.GetAll<Relationship>(SessionHelper.SelectedClientId), "Id", "RelationshipName", model.RelationshipId);
            ViewBag.DependentStatusId = new SelectList(db.GetAll<DependentStatus>(SessionHelper.SelectedClientId), "Id", "StatusName", model.DependentStatusId);
            ViewBag.GenderId = new SelectList(db.GetAll<Gender>(SessionHelper.SelectedClientId), "Id", "GenderName", model.GenderId);

            return PartialView(model);
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeDependent.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Dependent document is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    DateTime? expiryDate = null;
                    HttpPostedFileBase docFile = Request.Files[0];
                    string employeeDependentId = Request.Form["dependentDocRecordID"];
                    var tempDateStr = Request.Form["expiryDate"];
                    expiryDate = tempDateStr == "" ? expiryDate : DateTime.Parse(tempDateStr);
                    var employeeDependentEntity = db.EmployeeDependent.Find(int.Parse(employeeDependentId));
                    if (employeeDependentEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + employeeDependentEntity.UserInformationId + "_" + employeeDependentId + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("DependentDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        employeeDependentEntity.DocFilePath = filePathHelper.RelativePath;
                        employeeDependentEntity.DocName = docName;
                        employeeDependentEntity.ExpiryDate = expiryDate;
                        employeeDependentEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Dependent record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        public JsonResult AjaxCheckDocument(int id)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadDocFile;


            if (id > 0)
            {
                try
                {

                    var employeeDependentEntity = db.EmployeeDependent.Find(id);
                    if (employeeDependentEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeeDependentEntity.DocFilePath))
                        {
                            relativeFilePath = employeeDependentEntity.DocFilePath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadDocFile = new FileInfo(serverFilePath);
                            if (!downloadDocFile.Exists)
                            {
                                status = "Error";
                                message = "Document is not yet Uploaded!";
                            }
                        }
                        else
                        {
                            status = "Error";
                            message = "Document is not yet Uploaded!";
                        }
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Dependent record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {
                status = "Error";
                message = "Invalid Dependent record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var userDependent = db.EmployeeDependent.Find(id);
            if (userDependent != null)
            {
                if (!string.IsNullOrEmpty(userDependent.DocFilePath))
                {
                    relativeFilePath = userDependent.DocFilePath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadDocFile = new FileInfo(serverFilePath);

                    if (downloadDocFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadDocFile.Name);
                    }
                }

            }

            return null;

        }

        [HttpPost]
        public JsonResult CreateEdit(EmployeeDependent model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            if (ModelState.IsValid)
            {
                if (model.SSN != null)
                {
                    if (model.SSN.Length != 9)
                    {
                        ModelState.AddModelError("SSN", "Invalid format.");
                    }
                }
            }
            EmployeeDependent employeeDependentEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeDependentEntity = new EmployeeDependent();
                    employeeDependentEntity.UserInformationId = model.UserInformationId;
                    //employeeEducationEntity.CreatedBy = SessionHelper.LoginId;
                    //employeeEducationEntity.CreatedDate = DateTime.Now;
                    db.EmployeeDependent.Add(employeeDependentEntity);
                }
                else
                {
                    employeeDependentEntity = db.EmployeeDependent.Find(model.Id);
                    employeeDependentEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeDependentEntity.ModifiedDate = DateTime.Now;
                }
                employeeDependentEntity.FirstName = model.FirstName;
                employeeDependentEntity.LastName = model.LastName;
                employeeDependentEntity.BirthDate = model.BirthDate;
                employeeDependentEntity.GenderId = model.GenderId;
                employeeDependentEntity.RelationshipId = model.RelationshipId;
                employeeDependentEntity.DependentStatusId = model.DependentStatusId;
                employeeDependentEntity.SchoolAttending = model.SchoolAttending;
                employeeDependentEntity.IsDentalInsurance = model.IsDentalInsurance;
                employeeDependentEntity.IsFullTimeStudent = model.IsFullTimeStudent;
                employeeDependentEntity.IsHealthInsurance = model.IsHealthInsurance;
                employeeDependentEntity.IsTaxPurposes = model.IsTaxPurposes;

                if (model.SSN!=null)
                {
                    employeeDependentEntity.SSN = Encryption.Encrypt(model.SSN);
                }

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

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var employeeDependentEntity = db.EmployeeDependent.Find(id);
            try
            {
                employeeDependentEntity.ModifiedBy = SessionHelper.LoginId;
                employeeDependentEntity.ModifiedDate = DateTime.Now;
                employeeDependentEntity.DataEntryStatus = 0;
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

        [HttpPost]
        public ActionResult ChangeRequestEmployeeDependent(EmployeeDependent model)
        {
            if (ModelState.IsValid)
            {
                if (model.SSN != null)
                {
                    if (model.SSN.Length != 9)
                    {
                        ModelState.AddModelError("SSN", "Invalid format.");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeDependent dbObject = null;
                    if (model.Id > 0)
                    {
                        dbObject = db.EmployeeDependent.FirstOrDefault(u => u.Id == model.Id);
                        if (dbObject.ChangeRequestEmployeeDependent.Count > 0 && dbObject.ChangeRequestEmployeeDependent.Any(r => r.ChangeRequestStatusId == 1))
                        {
                            Dictionary<String, String> errors = new Dictionary<string, string>();
                            errors.Add("PopupMessage1", "There is already a pending change request for selcted emplyee.");
                            return GetErrors(errors);
                        }
                    }
                    else
                    {
                        dbObject = new EmployeeDependent();
                    }

                    ChangeRequestEmployeeDependent changeRequest = new ChangeRequestEmployeeDependent();
                    changeRequest = GetChanges(model, dbObject);

                    db.ChangeRequestEmployeeDependent.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 5);
                    workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmployeeDependent = changeRequest;
                    if (workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow.IsZeroLevel)
                    {
                        changeRequest.ChangeRequestStatusId = 2;
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestEmployeeDependent);
                        db.SaveChanges();
                        //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId ?? 0, SessionHelper.SelectedClientId);
                        //Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId.Value, changeRequest.Id, workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow, toEmails, db);

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
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }

            }
            return GetErrors();
        }

        private ChangeRequestEmployeeDependent GetChanges(EmployeeDependent model, EmployeeDependent dbObject)
        {
            ChangeRequestEmployeeDependent changeRequest = new ChangeRequestEmployeeDependent();
            changeRequest.UserInformationId = model.UserInformationId.Value;
            if (dbObject.Id > 0)
                changeRequest.EmployeeDependentId = dbObject.Id;
            changeRequest.ChangeRequestStatusId = 1;
            //changeRequest.RequestTypeId = model.RequestTypeId;
            if (dbObject.FirstName != model.FirstName)
            {
                changeRequest.NewFirstName = model.FirstName;
                changeRequest.FirstName = dbObject.FirstName;
            }
            if (dbObject.GenderId != model.GenderId)
            {
                changeRequest.NewGenderId = model.GenderId;
                changeRequest.GenderId = dbObject.GenderId;
            }
            if (dbObject.IsDentalInsurance != model.IsDentalInsurance)
            {
                changeRequest.NewIsDentalInsurance = model.IsDentalInsurance;
                changeRequest.IsDentalInsurance = dbObject.IsDentalInsurance;
            }
            if (dbObject.IsFullTimeStudent != model.IsFullTimeStudent)
            {
                changeRequest.NewIsFullTimeStudent = model.IsFullTimeStudent;
                changeRequest.IsFullTimeStudent = dbObject.IsFullTimeStudent;
            }
            if (dbObject.IsHealthInsurance != model.IsHealthInsurance)
            {
                changeRequest.NewIsHealthInsurance = model.IsHealthInsurance;
                changeRequest.IsHealthInsurance = dbObject.IsHealthInsurance;
            }
            if (dbObject.IsTaxPurposes != model.IsTaxPurposes)
            {
                changeRequest.NewIsTaxPurposes = model.IsTaxPurposes;
                changeRequest.IsTaxPurposes = dbObject.IsTaxPurposes;
            }
            if (dbObject.LastName != model.LastName)
            {
                changeRequest.NewLastName = model.LastName;
                changeRequest.LastName = dbObject.LastName;
            }
            if (dbObject.RelationshipId != model.RelationshipId)
            {
                changeRequest.NewRelationshipId = model.RelationshipId;
                changeRequest.RelationshipId = dbObject.RelationshipId;
            }
            if (dbObject.SchoolAttending != model.SchoolAttending)
            {
                changeRequest.NewSchoolAttending = model.SchoolAttending;
                changeRequest.SchoolAttending = dbObject.SchoolAttending;
            }
            if (Encryption.Decrypt(dbObject.SSN) != model.SSN)
            {
                changeRequest.NewSSN = Encryption.Encrypt(model.SSN);
                changeRequest.SSN = Encryption.Encrypt(dbObject.SSN);
            }
            if (dbObject.BirthDate != model.BirthDate)
            {
                changeRequest.NewBirthDate = model.BirthDate;
                changeRequest.BirthDate = dbObject.BirthDate;
            }
            if (dbObject.DependentStatusId != model.DependentStatusId)
            {
                changeRequest.NewDependentStatusId = model.DependentStatusId;
                changeRequest.DependentStatusId = dbObject.DependentStatusId;
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

        public ActionResult EmployeeDependentChangeRequest(int? id)
        {
            EmployeeDependent employeeDependent = null;
            if (id == -1)
            {
                employeeDependent = new EmployeeDependent();
                ViewBag.EmployeeDependentTitle = "Add new faimly member";
                ViewBag.EmployeeDependentStyle = "width:100%;";
            }
            else
            {
                employeeDependent = db.EmployeeDependent.FirstOrDefault(u => u.Id == id && u.DataEntryStatus != 0);
                ViewBag.EmployeeDependentTitle = "New Employee Dependent";
                ViewBag.EmployeeDependentStyle = "width:49%;margin-left:10px";

            }
            ViewBag.CanEmployeeDependantWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(SessionHelper.LoginId, 5);
            return PartialView("~/Views/UserInformation/EmployeeProfile/EmployeeDependent/_EmployeeDependentChangeRequest.cshtml", employeeDependent);
        }

        public ActionResult EmployeeDependentChangeRequestDelete(int? id)
        {
            EmployeeDependent employeeDependent = null;
            if (id == -1)
            {
                employeeDependent = new EmployeeDependent();
                ViewBag.EmployeeDependentTitle = "Add new faimly member";
                ViewBag.EmployeeDependentStyle = "width:100%;";
            }
            else
            {
                employeeDependent = db.EmployeeDependent.FirstOrDefault(u => u.Id == id && u.DataEntryStatus != 0);
                ViewBag.EmployeeDependentTitle = "New Employee Dependent";
                ViewBag.EmployeeDependentStyle = "width:49%;margin-left:10px";

            }
            ViewBag.CanEmployeeDependantWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(SessionHelper.LoginId, 5);
            return PartialView("~/Views/UserInformation/EmployeeProfile/EmployeeDependent/_EmployeeDependentDeleteView.cshtml", employeeDependent);
        }
        [HttpPost]
        public ActionResult EmployeeDependentChangeRequestDelete(EmployeeDependent model)
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
                    EmployeeDependent dbObject = null;
                    if (model.Id > 0)
                    {
                        dbObject = db.EmployeeDependent.FirstOrDefault(u => u.Id == model.Id);
                        if (dbObject.ChangeRequestEmployeeDependent.Count > 0 && dbObject.ChangeRequestEmployeeDependent.Any(r => r.ChangeRequestStatusId == 1))
                        {
                            Dictionary<String, String> errors = new Dictionary<string, string>();
                            errors.Add("PopupMessage1", "There is already a pending change request for selcted emplyee.");
                            return GetErrors(errors);
                        }
                    }
                    else
                    {
                        dbObject = new EmployeeDependent();
                    }

                    ChangeRequestEmployeeDependent changeRequest = new ChangeRequestEmployeeDependent();
                    changeRequest.UserInformationId = model.UserInformationId.Value;
                    changeRequest.EmployeeDependentId = dbObject.Id;
                    changeRequest.ChangeRequestStatusId = 1;
                    //changeRequest.RequestTypeId = model.RequestTypeId;
                    changeRequest.ReasonForDelete = model.ReasonForDelete;
                    db.ChangeRequestEmployeeDependent.Add(changeRequest);

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 5);
                    workflowTriggerRequestDetail.WorkflowTriggerRequest.ChangeRequestEmployeeDependent = changeRequest;
                    if (workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow.IsZeroLevel)
                    {
                        changeRequest.ChangeRequestStatusId = 2;
                        WorkflowService.ApplyChanges(db, changeRequest as ChangeRequestEmployeeDependent);
                        db.SaveChanges();
                        //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId ?? 0, SessionHelper.SelectedClientId);
                        //Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId.Value, changeRequest.Id, workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow, toEmails, db);

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
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }

            }
            return GetErrors();
        }

    }
}