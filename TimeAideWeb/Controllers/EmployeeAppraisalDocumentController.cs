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
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeAppraisalDocumentController : TimeAideWebControllers<EmployeeAppraisalDocument>
    {

        public ActionResult IndexByAppraisal(int? id)
        {
            try
            {
                AllowView();
                var model = db.EmployeeAppraisalDocument.Where(w => w.DataEntryStatus == 1 && w.EmployeeAppraisalId == id);
                return PartialView("Index", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(AppraisalTemplate).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
       
        [HttpPost]
        public JsonResult CreateEdit()
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";       
            EmployeeAppraisalDocument model = new EmployeeAppraisalDocument();
            model.Id= int.Parse(Request.Form["Id"]);
            model.EmployeeAppraisalId = int.Parse(Request.Form["EmployeeAppraisalId"]);
            model.AppraisalDocumentName = Request.Form["AppraisalDocumentName"];
            HttpPostedFileBase docFile = Request.Files[0];
            EmployeeAppraisalDocument employeeAppraisalDocumentEntity = null;
            try
            {
                var isExist = db.EmployeeAppraisalDocument
                    .Where(w => w.EmployeeAppraisalId == model.EmployeeAppraisalId
                            && w.DataEntryStatus == 1 && 
                            w.AppraisalDocumentName.ToLower().Contains(model.AppraisalDocumentName.ToLower().Trim()) && 
                            w.Id != model.Id).Count();
                if (isExist == 0)
                {
                    if (model.Id == 0)
                    {
                        employeeAppraisalDocumentEntity = new EmployeeAppraisalDocument();
                        employeeAppraisalDocumentEntity.EmployeeAppraisalId = model.EmployeeAppraisalId;
                        db.EmployeeAppraisalDocument.Add(employeeAppraisalDocumentEntity);
                    }
                    else
                    {
                        employeeAppraisalDocumentEntity = db.EmployeeAppraisalDocument.Find(model.Id);
                        employeeAppraisalDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                        employeeAppraisalDocumentEntity.ModifiedDate = DateTime.Now;
                    }
                    employeeAppraisalDocumentEntity.AppraisalDocumentName = model.AppraisalDocumentName;

                    db.SaveChanges();
                    if (employeeAppraisalDocumentEntity.Id > 0)
                    {
                        var fileExt = Path.GetExtension(docFile.FileName);
                        var userId = db.EmployeeAppraisal.Where(w => w.Id == model.EmployeeAppraisalId).Select(s => s.UserInformationId).FirstOrDefault();
                        string internalDocName = userId + "_" + employeeAppraisalDocumentEntity.EmployeeAppraisalId + "_" + employeeAppraisalDocumentEntity.Id + "-" + employeeAppraisalDocumentEntity.AppraisalDocumentName + fileExt;
                        
                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("appraisalDocs", internalDocName);
                        docFile.SaveAs(serverFilePath);
                        employeeAppraisalDocumentEntity.DocumentFilePath = filePathHelper.RelativePath;
                        employeeAppraisalDocumentEntity.DocumentFileName = internalDocName;
                        db.SaveChanges();

                    }
                    else
                    {
                        status = "Error";
                        message = "Error in saving Appraisal Document";
                    }
                }
                else
                {
                    status = "Error";
                    message = "Document is already exist in appraisal";
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

        public JsonResult AjaxCheckAppraisalDocument(int id)
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
                    var employeeAppraisalDocumentEntity = db.EmployeeAppraisalDocument.Find(id);
                    if (employeeAppraisalDocumentEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeeAppraisalDocumentEntity.DocumentFilePath))
                        {
                            relativeFilePath = employeeAppraisalDocumentEntity.DocumentFilePath;
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
                        message = "Invalid Appraisal Document record data!";
                    }

                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {
                status = "Error";
                message = "Invalid Training record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadAppraisalDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var employeeAppraisalDocument = db.EmployeeAppraisalDocument.Find(id);
            if (employeeAppraisalDocument != null)
            {
                if (!string.IsNullOrEmpty(employeeAppraisalDocument.DocumentFilePath))
                {
                    relativeFilePath = employeeAppraisalDocument.DocumentFilePath;
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
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var employeeAppraisalDocumentEntity = db.EmployeeAppraisalDocument.Find(id);
            try
            {
                employeeAppraisalDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                employeeAppraisalDocumentEntity.ModifiedDate = DateTime.Now;
                employeeAppraisalDocumentEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }



    }
}