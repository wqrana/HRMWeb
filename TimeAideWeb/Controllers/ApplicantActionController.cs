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
    public class ApplicantActionController : BaseApplicantRoleRightsController<ApplicantAction>
    {
        [HttpGet]
        // GET: EmployeePerformance
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantAction.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
        public ActionResult MostRecentRecord(int? id)
        {
            try
            {
                var model = db.ApplicantAction.Where(w => w.ApplicantInformationId == id && w.DataEntryStatus == 1)
                                                .OrderByDescending(u => u.CreatedDate)
                                                    .FirstOrDefault();

                return PartialView(model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                 throw ex;
            }
        }
        public ActionResult Details(int? id)
        {
            try
            {
                var model = db.ApplicantAction.Find(id ?? 0);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.ApprovedById= new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                    .Where(w => w.CompanyId == SessionHelper.SelectedCompanyId), "Id", "ShortFullName") ;

                ViewBag.ActionTypeId = new SelectList(db.GetAll<ActionType>(SessionHelper.SelectedClientId), "Id", "ActionTypeName");

                return PartialView();
            }
            catch (AuthorizationException ex)
            {               
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantAction", "Create");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
        }
        public  ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantAction.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.ApprovedById = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                   .Where(w => w.CompanyId == SessionHelper.SelectedCompanyId), "Id", "ShortFullName", model.ApprovedById);
                ViewBag.ActionTypeId = new SelectList(db.GetAll<ActionType>(SessionHelper.SelectedClientId), "Id", "ActionTypeName", model.ActionTypeId);

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantAction", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.ApplicantAction.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Action document is Successfully uploaded!";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string actionID = Request.Form["ActionDocRecordID"];
                    var applicantActionEntity = db.ApplicantAction.Find(int.Parse(actionID));
                    if (applicantActionEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + applicantActionEntity.ApplicantInformationId + "_" + actionID + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("Applicant\\actionDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        applicantActionEntity.DocFilePath = filePathHelper.RelativePath;
                        applicantActionEntity.DocName = docName;
                        applicantActionEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Applicant Action record data!";
                    }

                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
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

                    var applicantActionEntity = db.ApplicantAction.Find(id);
                    if (applicantActionEntity != null)
                    {
                        if (!string.IsNullOrEmpty(applicantActionEntity.DocFilePath))
                        {
                            relativeFilePath = applicantActionEntity.DocFilePath;
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
                        message = "Invalid Applicant Action record data!";
                    }

                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {
                status = "Error";
                message = "Invalid Action record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var applicantAction = db.ApplicantAction.Find(id);
            if (applicantAction != null)
            {
                if (!string.IsNullOrEmpty(applicantAction.DocFilePath))
                {
                    relativeFilePath = applicantAction.DocFilePath;
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
        public JsonResult CreateEdit(ApplicantAction model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantAction applicantActionEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    applicantActionEntity = new ApplicantAction();
                    applicantActionEntity.ApplicantInformationId = model.ApplicantInformationId;
                    db.ApplicantAction.Add(applicantActionEntity);
                }
                else
                {
                    applicantActionEntity = db.ApplicantAction.Find(model.Id);
                    applicantActionEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantActionEntity.ModifiedDate = DateTime.Now;
                }
                applicantActionEntity.ActionTypeId = model.ActionTypeId;
                applicantActionEntity.ActionDate = model.ActionDate;
                applicantActionEntity.ActionExpiryDate = model.ActionExpiryDate;
                applicantActionEntity.ActionName = model.ActionName;
                applicantActionEntity.ActionDescription = model.ActionDescription;
                applicantActionEntity.ActionNotes = model.ActionNotes;

                applicantActionEntity.ActionEndDate = model.ActionEndDate;
                applicantActionEntity.ActionApprovedDate = model.ActionApprovedDate;
                applicantActionEntity.ApprovedById = model.ApprovedById;
                applicantActionEntity.ActionClosingInfo = model.ActionClosingInfo;

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

        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();
                var model = db.ApplicantAction.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantAction", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantActionEntity = db.ApplicantAction.Find(id);
            try
            {
                applicantActionEntity.ModifiedBy = SessionHelper.LoginId;
                applicantActionEntity.ModifiedDate = DateTime.Now;
                applicantActionEntity.DataEntryStatus = 0;
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
