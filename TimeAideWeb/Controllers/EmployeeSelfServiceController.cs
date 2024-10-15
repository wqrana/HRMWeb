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
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeSelfServiceController : TimeAideWebControllers<UserInformation>
    {
        public ActionResult UploadDocument(int? id)
        {
            SelfServiceEmployeeDocument selfServiceEmployeeDocument = new SelfServiceEmployeeDocument();
            EmployeeDocument employeeDocument;
            ViewBag.DocumentList = DocumentService.GetDocuments(true);
            if (id.HasValue)
            {
                employeeDocument = db.Find<EmployeeDocument>(id.Value, SessionHelper.SelectedClientId);
                selfServiceEmployeeDocument.EmployeeDocument = employeeDocument;
                selfServiceEmployeeDocument.EmployeeDocumentId = id.Value;
                selfServiceEmployeeDocument.DocumentNote = employeeDocument.DocumentNote;
                selfServiceEmployeeDocument.DocumentId = employeeDocument.DocumentId;
                selfServiceEmployeeDocument.Document = employeeDocument.Document;

            }
            return PartialView(selfServiceEmployeeDocument);
        }

        [HttpPost]
        public JsonResult UploadDocument(SelfServiceEmployeeDocument model)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "Notification alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));
                    HttpPostedFileBase employeeDocumentFile = Request.Files[0];
                    var selfServiceEmployeeDocument = new SelfServiceEmployeeDocument();
                    int employeeDocumentId = 0;
                    if (!String.IsNullOrEmpty(Request.Form["employeeDocumentId"]))
                    {
                        employeeDocumentId = Convert.ToInt32(Request.Form["employeeDocumentId"]);
                    }

                    selfServiceEmployeeDocument.DocumentName = Request.Form["DocumentName"].ToString();
                    selfServiceEmployeeDocument.DocumentNote = Request.Form["DocumentNote"].ToString();
                    if (Request.Form["ExpirationDate"] != null && !String.IsNullOrEmpty(Request.Form["ExpirationDate"]))
                        selfServiceEmployeeDocument.ExpirationDate = Convert.ToDateTime(Request.Form["ExpirationDate"].ToString());

                    selfServiceEmployeeDocument.UserInformationId = SessionHelper.LoginId;
                    if (employeeDocumentId > 0)
                    {
                        var employeeDocument = db.EmployeeDocument.FirstOrDefault(c => c.Id == employeeDocumentId);
                        selfServiceEmployeeDocument.EmployeeDocumentId = employeeDocument.Id;
                        selfServiceEmployeeDocument.DocumentId = employeeDocument.DocumentId;
                        if (string.IsNullOrEmpty(selfServiceEmployeeDocument.DocumentNote))
                            selfServiceEmployeeDocument.DocumentNote = employeeDocument.DocumentNote;
                    }
                    else
                    {
                        if (Request.Form["DocumentId"] != null)
                            selfServiceEmployeeDocument.DocumentId = Convert.ToInt32(Request.Form["DocumentId"].ToString());
                    }
                    //SelfServiceEmployeeDocument.ChatConversationId = conversationId;
                    selfServiceEmployeeDocument.ChangeRequestStatusId = 1;
                    db.SelfServiceEmployeeDocument.Add(selfServiceEmployeeDocument);
                    db.SaveChanges();


                    var fileName = Path.GetFileName(employeeDocumentFile.FileName);
                    var fileExt = Path.GetExtension(employeeDocumentFile.FileName);
                    string docName = fileName;
                    selfServiceEmployeeDocument.OriginalDocumentName = fileName;
                    FilePathHelper filePathHelper = new FilePathHelper();
                    string serverFilePath = filePathHelper.GetPath("EmployeeSelfService\\Employee_" + SessionHelper.LoginId + "\\" + "EmployeeDocuments" + "\\", ref docName, SessionHelper.LoginId, selfServiceEmployeeDocument.Id);
                    employeeDocumentFile.SaveAs(serverFilePath);
                    selfServiceEmployeeDocument.DocumentPath = filePathHelper.RelativePath;
                    selfServiceEmployeeDocument.DocumentName = docName;
                    db.SaveChanges();

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 8);
                    if (workflowTriggerRequestDetail.WorkflowLevel.Workflow.IsZeroLevel)
                    {
                        WorkflowService.ApplyChanges(db, selfServiceEmployeeDocument as SelfServiceEmployeeDocument);
                        var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(selfServiceEmployeeDocument.UserInformationId, SessionHelper.SelectedClientId);
                        db.SaveChanges();
                        Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        WorkflowService.ProcesstWorkflowClosingNotification<SelfServiceEmployeeDocument>(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, selfServiceEmployeeDocument);
                    }
                    else
                    {
                        workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeDocument = selfServiceEmployeeDocument;
                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(selfServiceEmployeeDocument.UserInformationId, workflowTriggerRequestDetail, selfServiceEmployeeDocument.Id, db);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        }
                    }



                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    //status = "Error";
                    //message = ex.Message;
                }
            }
            return Json(new { status = "", message = "success" });
        }

        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.SelfServiceEmployeeDocument.Find(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.DocumentPath))
                {
                    relativeFilePath = user.DocumentPath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadCVFile = new FileInfo(serverFilePath);

                    if (downloadCVFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadCVFile.Name);
                    }
                }

            }

            return null;

        }

        [HttpPost]
        public JsonResult AjaxCheckDocument(int userID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadCVFile;
            // serverFilePath = Path.Combine(Server.MapPath("~/Content/doc/resumes"), profilePictureName);
            //retResult = new { status = "Error", message = "No file is selected" };

            if (userID > 0)
            {
                try
                {

                    var user = db.SelfServiceEmployeeDocument.Find(userID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.DocumentPath))
                        {
                            relativeFilePath = user.DocumentPath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadCVFile = new FileInfo(serverFilePath);
                            if (!downloadCVFile.Exists)
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
                        message = "Invalid User record data!";
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
                message = "Invalid User record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult);
        }




        public ActionResult UploadCredential(int? id)
        {
            SelfServiceEmployeeCredential selfServiceEmployeeCredential = new SelfServiceEmployeeCredential();
            EmployeeCredential employeeCredential;
            ViewBag.DocumentList = DocumentService.GetDocuments(true);
            if (id.HasValue)
            {
                employeeCredential = db.Find<EmployeeCredential>(id.Value, SessionHelper.SelectedClientId);
                selfServiceEmployeeCredential.EmployeeCredential = employeeCredential;
                selfServiceEmployeeCredential.EmployeeCredentialId = id.Value;
                selfServiceEmployeeCredential.ExpirationDate = employeeCredential.ExpirationDate;
                selfServiceEmployeeCredential.EmployeeCredentialName = employeeCredential.EmployeeCredentialName;
                selfServiceEmployeeCredential.EmployeeCredentialDescription = employeeCredential.EmployeeCredentialDescription;
                selfServiceEmployeeCredential.IssueDate = employeeCredential.IssueDate;
                selfServiceEmployeeCredential.Note = employeeCredential.Note;

            }
            return PartialView(selfServiceEmployeeCredential);
        }

        [HttpPost]
        public JsonResult UploadCredential(SelfServiceEmployeeCredential model)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "Notification alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));
                    HttpPostedFileBase employeeDocumentFile = Request.Files[0];

                    var SelfServiceEmployeeCredential = new SelfServiceEmployeeCredential();

                    int employeeCredentialId;
                    int.TryParse(Request.Form["EmployeeCredentialId"], out employeeCredentialId);

                    if (Request.Form["IssueDate"] != null && !String.IsNullOrEmpty(Request.Form["IssueDate"]))
                        SelfServiceEmployeeCredential.IssueDate = Convert.ToDateTime(Request.Form["IssueDate"].ToString());
                    SelfServiceEmployeeCredential.EmployeeCredentialName = Request.Form["EmployeeCredentialName"].ToString();
                    if (Request.Form["ExpirationDate"] != null && !String.IsNullOrEmpty(Request.Form["ExpirationDate"]))
                        SelfServiceEmployeeCredential.ExpirationDate = Convert.ToDateTime(Request.Form["ExpirationDate"].ToString());
                    SelfServiceEmployeeCredential.Note = Request.Form["Note"].ToString();
                    SelfServiceEmployeeCredential.EmployeeCredentialDescription = Request.Form["EmployeeCredentialDescription"].ToString();
                    SelfServiceEmployeeCredential.CredentialId = Convert.ToInt32(Request.Form["CredentialId"]);
                    if (Request.Form["CredentialTypeId"] != null && !String.IsNullOrEmpty(Request.Form["CredentialTypeId"]))
                        SelfServiceEmployeeCredential.CredentialTypeId = Convert.ToInt32(Request.Form["CredentialTypeId"]);
                    SelfServiceEmployeeCredential.UserInformationId = SessionHelper.LoginId;
                    if (employeeCredentialId > 0)
                    {
                        var employeeCredential = db.EmployeeCredential.FirstOrDefault(c => c.Id == employeeCredentialId);
                        SelfServiceEmployeeCredential.EmployeeCredentialId = employeeCredential.Id;
                        SelfServiceEmployeeCredential.CredentialTypeId = employeeCredential.CredentialTypeId;
                        if (string.IsNullOrEmpty(SelfServiceEmployeeCredential.EmployeeCredentialDescription))
                            SelfServiceEmployeeCredential.EmployeeCredentialDescription = employeeCredential.EmployeeCredentialDescription;
                    }

                    //SelfServiceEmployeeDocument.ChatConversationId = conversationId;

                    SelfServiceEmployeeCredential.ChangeRequestStatusId = 1;
                    db.SelfServiceEmployeeCredential.Add(SelfServiceEmployeeCredential);
                    db.SaveChanges();

                    var fileName = Path.GetFileName(employeeDocumentFile.FileName);
                    var fileExt = Path.GetExtension(employeeDocumentFile.FileName);
                    string docName = fileName;

                    FilePathHelper filePathHelper = new FilePathHelper();
                    string serverFilePath = filePathHelper.GetPath("EmployeeSelfService\\Employee_" + SessionHelper.LoginId + "\\" + "EmployeeCredentials" + "\\", ref docName, SessionHelper.LoginId, SelfServiceEmployeeCredential.Id);
                    employeeDocumentFile.SaveAs(serverFilePath);
                    SelfServiceEmployeeCredential.DocumentPath = filePathHelper.RelativePath;
                    SelfServiceEmployeeCredential.DocumentName = docName;
                    SelfServiceEmployeeCredential.OriginalDocumentName = fileName;
                    db.SaveChanges();

                    WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(db, 7);
                    if (workflowTriggerRequestDetail.WorkflowLevel.Workflow.IsZeroLevel)
                    {
                        WorkflowService.ApplyChanges(db, SelfServiceEmployeeCredential as SelfServiceEmployeeCredential);
                        var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(SelfServiceEmployeeCredential.UserInformationId, SessionHelper.SelectedClientId);
                        db.SaveChanges();
                        Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                        WorkflowService.ProcesstWorkflowClosingNotification<SelfServiceEmployeeCredential>(db, workflowTriggerRequestDetail.WorkflowTriggerRequest, SelfServiceEmployeeCredential);
                    }
                    else
                    {
                        workflowTriggerRequestDetail.WorkflowTriggerRequest.SelfServiceEmployeeCredential = SelfServiceEmployeeCredential;
                        db.SaveChanges();
                        try
                        {
                            TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflow(SelfServiceEmployeeCredential.UserInformationId, workflowTriggerRequestDetail, SelfServiceEmployeeCredential.Id, db);
                        }
                        catch (Exception ex)
                        {
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        }
                    }



                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    //status = "Error";
                    //message = ex.Message;
                }
            }
            return Json(new { status = "", message = "success" });
        }

        public ActionResult DownloadCredential(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.SelfServiceEmployeeCredential.Find(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.DocumentPath))
                {
                    relativeFilePath = user.DocumentPath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadCVFile = new FileInfo(serverFilePath);

                    if (downloadCVFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadCVFile.Name);
                    }
                }

            }

            return null;

        }

        [HttpPost]
        public JsonResult AjaxCheckCredential(int userID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadCVFile;
            // serverFilePath = Path.Combine(Server.MapPath("~/Content/doc/resumes"), profilePictureName);
            //retResult = new { status = "Error", message = "No file is selected" };

            if (userID > 0)
            {
                try
                {

                    var user = db.SelfServiceEmployeeCredential.Find(userID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.DocumentPath))
                        {
                            relativeFilePath = user.DocumentPath.Replace("\\\\", "\\"); 
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadCVFile = new FileInfo(serverFilePath);

                            if (!downloadCVFile.Exists)
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
                        message = "Invalid User record data!";
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
                message = "Invalid User record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult);
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
