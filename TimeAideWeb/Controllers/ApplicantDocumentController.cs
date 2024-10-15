
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
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ApplicantDocumentController : BaseApplicantRoleRightsController<ApplicantDocument>
    {
      
        [HttpGet]
        // GET: EmployeeEducation
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantDocument.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id)
                                            .Where(d => d.Document.DocumentRequiredById == 2 || d.Document.DocumentRequiredById == 3).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
       
        public ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.DocumentId = new SelectList(DocumentService.GetDocuments(false), "Id", "DocumentName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantDocument", "Create");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantDocument.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.DocumentId = new SelectList(DocumentService.GetDocuments(false), "Id", "DocumentName", model.DocumentId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantDocument", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {                
                throw  ex;
            }
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.ApplicantDocument.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Document is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string documentId = Request.Form["docRecordID"];
                    var applicantDocumentEntity = db.ApplicantDocument.Find(int.Parse(documentId));
                    if (applicantDocumentEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + applicantDocumentEntity.ApplicantInformationId + "_" + documentId + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("Applicant\\ApplicantDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        applicantDocumentEntity.DocumentName = docName;
                        applicantDocumentEntity.DocumentPath = filePathHelper.RelativePath;
                        applicantDocumentEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();

                    }
                    else
                    {

                        status = "Error";
                        message = "Invalid Document record data!";
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

                    var applicantDocEntity = db.ApplicantDocument.Find(id);
                    if (applicantDocEntity != null)
                    {
                        if (!string.IsNullOrEmpty(applicantDocEntity.DocumentPath))
                        {
                            relativeFilePath = applicantDocEntity.DocumentPath;
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
                        message = "Invalid Education record data!";
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
                message = "Invalid Education record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AjaxCheckExpirableDocument(int id)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            bool isExpirable = false;

            if (id > 0)
            {
                try
                {
                    isExpirable = db.Document.Where(w => w.Id == id)
                                    .Select(s => s.IsExpirable)
                                    .FirstOrDefault();
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
                message = "Invalid record data!";
            }
            retResult = new { status = status, message = message, isExpirable = isExpirable };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var applicantDoc = db.ApplicantDocument.Find(id);
            if (applicantDoc != null)
            {
                if (!string.IsNullOrEmpty(applicantDoc.DocumentPath))
                {
                    relativeFilePath = applicantDoc.DocumentPath;
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
        public JsonResult CreateEdit(ApplicantDocument model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantDocument applicantDocumentEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    applicantDocumentEntity = new ApplicantDocument();
                    applicantDocumentEntity.ApplicantInformationId = model.ApplicantInformationId;
                    db.ApplicantDocument.Add(applicantDocumentEntity);
                }
                else
                {
                    applicantDocumentEntity = db.ApplicantDocument.Find(model.Id);
                    applicantDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantDocumentEntity.ModifiedDate = DateTime.Now;
                }
                applicantDocumentEntity.DocumentId = model.DocumentId;
                //applicantDocumentEntity.DocumentName = model.DocumentName;
                applicantDocumentEntity.DocumentNote = model.DocumentNote;
                applicantDocumentEntity.ExpirationDate = model.ExpirationDate;

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();
                var model = db.ApplicantDocument.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantDocument", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantDocumentEntity = db.ApplicantDocument.Find(id);
            try
            {
                applicantDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                applicantDocumentEntity.ModifiedDate = DateTime.Now;
                applicantDocumentEntity.DataEntryStatus = 0;
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
     
    }


}