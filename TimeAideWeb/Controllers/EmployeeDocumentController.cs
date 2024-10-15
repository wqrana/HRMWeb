using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeDocumentController : TimeAideWebControllers<EmployeeDocument>
    {
        public override void OnCreate()
        {
            ViewBag.DocumentList = DocumentService.GetDocuments(true);
        }
        // POST: EmployeeDocument/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserInformationId,DocumentId,ExpirationDate,DocumentName,DocumentPath,DocumentNote")] EmployeeDocument employeeDocument)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeDocument.Add(employeeDocument);
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employeeDocument.UserInformationId });
            }

            return GetErrors();
        }

        public override EmployeeDocument OnEdit(EmployeeDocument entity, int id)
        {
            ViewBag.DocumentList = DocumentService.GetDocuments(true);
            if (!string.IsNullOrEmpty(entity.DocumentPath) && string.IsNullOrEmpty(entity.DocumentName))
            {
                string[] folders = entity.DocumentPath.Split('\\');
                if(folders.Count()>0)
                {
                    entity.DocumentName = folders[folders.Length - 1];
                }
            }
            if (id == 0)
                entity = new EmployeeDocument() { };
            return entity;
        }
        public ActionResult EditEmployeeDocument(int? id, int documentId, int userInformationId)
        {
            try
            {
                AllowEdit();
                if (id == null)
                {
                    return PartialView();
                }
                EmployeeDocument entity = db.Find<EmployeeDocument>(id.Value, SessionHelper.SelectedClientId);
                if (id == 0)
                {
                    Document document = db.Find<Document>(documentId, SessionHelper.SelectedClientId);
                    entity = new EmployeeDocument() { DocumentId = documentId, Document = document, UserInformationId = userInformationId, ClientId = SessionHelper.SelectedClientId };
                }

                if (!string.IsNullOrEmpty(entity.DocumentPath) && string.IsNullOrEmpty(entity.DocumentName))
                {
                    try
                    {
                        string[] folders = entity.DocumentPath.Split('\\');
                        if (folders.Count() > 0)
                        {
                            entity.DocumentName = folders[folders.Length - 1];
                        }
                    }
                    catch (Exception ex)
                    {
                        entity.DocumentName = entity.DocumentPath;
                    }
                }

                ViewBag.DocumentList = DocumentService.GetDocuments(true);

                ViewBag.Label = ViewBag.Label + " - Edit";
                ViewBag.IsBaseCompanyObject = false;
                ViewBag.IsAllCompanies = false;
                ViewBag.CanBeAssignedToCurrentCompany = false;
                return PartialView("Edit", entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override List<EmployeeDocument> OnIndexByUser(List<EmployeeDocument> model, int userId)
        {
            var list = DocumentService.GetDocuments(true);
            foreach (var each in list)
            {
                if (model == null || !model.Any(e => e.DocumentId == each.Id))
                    model.Add(new EmployeeDocument() { Document = each, DocumentId = each.Id, UserInformationId = userId });
            }
            return model;
        }
        // POST: EmployeeDocument/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserInformationId,DocumentId,DocumentName,ExpirationDate,DocumentPath,DocumentNote")] EmployeeDocument employeeDocument)
        {
            if (ModelState.IsValid)
            {
                var employeeDocumentDb = (new TimeAideContext()).EmployeeDocument.FirstOrDefault(ec => ec.Id == employeeDocument.Id);

                employeeDocument.SetUpdated<EmployeeDocument>();
                db.Entry(employeeDocument).State = EntityState.Modified;
                if (!string.IsNullOrEmpty(employeeDocumentDb.DocumentName))
                    employeeDocument.DocumentName = employeeDocumentDb.DocumentName;
                if (!string.IsNullOrEmpty(employeeDocumentDb.DocumentPath))
                    employeeDocument.DocumentPath = employeeDocumentDb.DocumentPath;
                //if (employeeDocumentDb.ExpirationDate != employeeDocument.ExpirationDate)
                //    db.NotificationLog.Where(l => l.EmployeeDocumentId == employeeDocument.Id).ToList().ForEach(c => c.DataEntryStatus = 0);
                //    db.NotificationLog.Where(l => l.EmployeeDocumentId == employeeDocument.Id).ToList().ForEach(c => c.DataEntryStatus = 0);
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employeeDocument.UserInformationId });
            }

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

        public FileResult GetReport()
        {
            string ReportURL = "{Your File Path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }


        [HttpPost]
        public JsonResult UploadSubmitDocument()
        {
            var employeeDocument = new EmployeeDocument();
            int employeeDocumentId = 0;
            if (!String.IsNullOrEmpty(Request.Form["employeeDocumentId"]))
            {
                employeeDocumentId = Convert.ToInt32(Request.Form["employeeDocumentId"]);
                if (employeeDocumentId > 0)
                {
                    employeeDocument = db.EmployeeDocument.FirstOrDefault(ec => ec.Id == employeeDocumentId);
                }
            }
            //employeeDocument.DocumentName = Request.Form["DocumentName"].ToString();
            employeeDocument.DocumentNote = Request.Form["DocumentNote"].ToString();
            if (Request.Form["ExpirationDate"] != "undefined" && !String.IsNullOrEmpty(Request.Form["ExpirationDate"]))
                employeeDocument.ExpirationDate = Convert.ToDateTime(Request.Form["ExpirationDate"].ToString());
            if (Request.Form["SubmissionDate"] != null && !String.IsNullOrEmpty(Request.Form["SubmissionDate"]))
                employeeDocument.SubmissionDate = Convert.ToDateTime(Request.Form["SubmissionDate"].ToString());
            if (Request.Form["DocumentId"] != null)
                employeeDocument.DocumentId = Convert.ToInt32(Request.Form["DocumentId"].ToString());

            employeeDocument.DocumentName = Request.Form["DocumentName"].ToString();
            employeeDocument.DocumentPath = Request.Form["DocumentPath"].ToString();

            int userInformationId = 0;
            if (!String.IsNullOrEmpty(Request.Form["UserInformationId"]))
            {
                userInformationId = Convert.ToInt32(Request.Form["UserInformationId"]);
                if (userInformationId > 0)
                    employeeDocument.UserInformationId = userInformationId;
            }

            if (employeeDocument != null && employeeDocumentId == 0)
            {
                db.EmployeeDocument.Add(employeeDocument);
            }
            else
            {
                employeeDocument.SetUpdated<EmployeeDocument>();
                db.Entry(employeeDocument).State = EntityState.Modified;
            }

            db.SaveChanges();

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase employeeDocumentFile = Request.Files[0];
                    var fileName = Path.GetFileName(employeeDocumentFile.FileName);
                    var fileExt = Path.GetExtension(employeeDocumentFile.FileName);
                    string docName = fileName;

                    //FilePathHelper filePathHelper = new FilePathHelper();
                    //string serverFilePath = filePathHelper.GetPath("EmployeeSelfService\\Employee_" + SessionHelper.LoginId + "\\" + "EmployeeDocuments" + "\\", ref docName, SessionHelper.LoginId, employeeDocument.Id);
                    //employeeDocumentFile.SaveAs(serverFilePath);
                    //employeeDocument.DocumentPath = filePathHelper.RelativePath;
                    //employeeDocument.DocumentName = docName;
                    FilePathHelper filePathHelper = new FilePathHelper();
                    string serverFilePath = filePathHelper.GetPath("EmployeeDocuments", ref docName, employeeDocument.UserInformationId ?? 0, employeeDocument.Id);
                    employeeDocument.DocumentPath = docName;
                    employeeDocument.DocumentName = fileName;
                    employeeDocumentFile.SaveAs(serverFilePath);
                    employeeDocument.DocumentPath = filePathHelper.RelativePath;
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

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeDocument.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Document is Successfully uploaded!";
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string employeeDocumentId = Request.Form["employeeDocumentId"];
                    var employeeDocument = db.EmployeeDocument.Find(int.Parse(employeeDocumentId));
                    if (employeeDocument != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        var fileExt = Path.GetExtension(docFile.FileName);
                        string docName = fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("EmployeeDocuments", ref docName, employeeDocument.UserInformationId ?? 0, int.Parse(employeeDocumentId));
                        docFile.SaveAs(serverFilePath);
                        employeeDocument.DocumentPath = filePathHelper.RelativePath;
                        employeeDocument.DocumentName = docName;
                        employeeDocument.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid document record data!";
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

                    var user = db.EmployeeDocument.Find(userID);
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
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.EmployeeDocument.Find(id);
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
        public ActionResult EmployeeDocumentUploadHistory(int id,int documentId)
        {
            var employeeDocument = db.EmployeeDocument.FirstOrDefault(c => c.Id == id);
            if (employeeDocument == null)
            {
                var document = db.Document.FirstOrDefault(c => c.Id == documentId);
                if (document != null)
                {
                    ViewBag.DocumentTitle = document.DocumentName;
                }
                return PartialView(new List<EmployeeDocument>());
            }
            string title = employeeDocument.Document.DocumentName;
            ViewBag.DocumentTitle = title;
            List<int> employeeDocuments = new List<int>();
            // SelfServiceDocuments(id, employeeDocuments);

            List<EmployeeDocument> selfServiceDocuments = new List<EmployeeDocument>();
            FilePathHelper filePathHelper = new FilePathHelper();
            string serverFilePath = filePathHelper.GetPath("EmployeeDocuments");
            string[] files = Directory.GetFiles(serverFilePath, "*.*", SearchOption.AllDirectories);
            //if (employeeDocuments.Count() > 0)
            {
                int userId = employeeDocument.UserInformationId ?? 0;
                foreach (string s in files)
                {
                    //foreach (int documentId in employeeDocuments)
                    {
                        string documentName = Path.GetFileName(s);
                        if (s.Contains(userId.ToString() + "_" + id.ToString() + "_"))
                        {
                            string[] fileNameParts = s.Split('_');
                            DateTime documentUploadDate = DateTime.Now;
                            if (fileNameParts.Length >= 3)
                            {
                                try
                                {
                                    documentUploadDate = DateTime.ParseExact(fileNameParts[2], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                    //documentUploadDate = Convert.ToDateTime(fileNameParts[2]);
                                }
                                catch { }
                            }
                            selfServiceDocuments.Add(new EmployeeDocument() { Id = id, DocumentName = documentName, DocumentPath = documentName + "\\" + documentName, CreatedDate = documentUploadDate });
                        }
                    }
                }
            }

            return PartialView(selfServiceDocuments);
        }
        //public bool SelfServiceDocuments(int employeeDocumentId, List<int> employeeDocuments)
        //{
        //    employeeDocuments.Add(employeeDocumentId);
        //    var selfServiceDocuments = db.SelfServiceEmployeeDocument.FirstOrDefault(c => c.EmployeeDocumentId == employeeDocumentId);
        //    if (selfServiceDocuments != null && selfServiceDocuments.NewEmployeeDocumentId.HasValue)
        //        SelfServiceDocuments(selfServiceDocuments.NewEmployeeDocumentId.Value, employeeDocuments);
        //    return false;
        //}

        public override EmployeeDocument OnDelete(EmployeeDocument entity)
        {
            entity.DocumentName = "";
            entity.DocumentNote = "";
            entity.DocumentPath = "";
            entity.ExpirationDate = null;
            entity.SubmissionDate = null;
            entity.DataEntryStatus = 1;
            return base.OnDelete(entity);
        }
    }
}
