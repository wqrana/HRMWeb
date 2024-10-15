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
    public class EmployeePerformanceController : TimeAideWebControllers<EmployeePerformance>
    {
        [HttpGet]
        // GET: EmployeePerformance
        public ActionResult IndexByUser(int id)
        {
            var model = db.EmployeePerformance.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }

        public override ActionResult Create()
        {

            // ViewBag.TrainingId = new SelectList(db.Training.Where(w => w.DataEntryStatus == 1), "Id", "TrainingName");
            //ViewBag.SupervisorId = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "FullName");
            ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "FullName");
            ViewBag.ActionTakenId= new SelectList(db.GetAll <ActionTaken>(SessionHelper.SelectedClientId), "Id","ActionTakenName");
            ViewBag.PerformanceDescriptionId= new SelectList(db.GetAll<PerformanceDescription>(SessionHelper.SelectedClientId), "Id", "PerformanceDescriptionName");
            ViewBag.PerformanceResultId = new SelectList(db.GetAll<PerformanceResult>(SessionHelper.SelectedClientId), "Id", "PerformanceResultName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeePerformance.Where(w => w.Id == id).FirstOrDefault();
            //ViewBag.SupervisorId = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "FullName",model.SupervisorId);
            ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "FullName", model.SupervisorId);
            ViewBag.ActionTakenId = new SelectList(db.GetAll<ActionTaken>(SessionHelper.SelectedClientId), "Id", "ActionTakenName",model.ActionTakenId);
            ViewBag.PerformanceDescriptionId = new SelectList(db.GetAll<PerformanceDescription>(SessionHelper.SelectedClientId), "Id", "PerformanceDescriptionName",model.PerformanceDescriptionId);
            ViewBag.PerformanceResultId = new SelectList(db.GetAll<PerformanceResult>(SessionHelper.SelectedClientId), "Id", "PerformanceResultName",model.PerformanceResultId);
            // ViewBag.TrainingId = new SelectList(db.Training.Where(w => w.DataEntryStatus == 1), "Id", "TrainingName", model.TrainingId);
            return PartialView(model);
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeePerformance.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Performance document is Successfully uploaded!";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string performanceID = Request.Form["performanceDocRecordID"];
                    var employeePerformanceEntity = db.EmployeePerformance.Find(int.Parse(performanceID));
                    if (employeePerformanceEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        var fileExt = Path.GetExtension(docFile.FileName);
                        string docName = "" + employeePerformanceEntity.UserInformationId + "_" + performanceID + "-" + fileName;
                        
                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("performanceDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        employeePerformanceEntity.DocFilePath = filePathHelper.RelativePath;
                        employeePerformanceEntity.DocName = docName;
                        employeePerformanceEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Performance record data!";
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

                    var employeePerformanceEntity = db.EmployeePerformance.Find(id);
                    if (employeePerformanceEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeePerformanceEntity.DocFilePath))
                        {
                            relativeFilePath = employeePerformanceEntity.DocFilePath;
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
                        message = "Invalid Performance record data!";
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
                message = "Invalid Performance record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var userPerformance = db.EmployeePerformance.Find(id);
            if (userPerformance != null)
            {
                if (!string.IsNullOrEmpty(userPerformance.DocFilePath))
                {
                    relativeFilePath = userPerformance.DocFilePath;
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
        public JsonResult CreateEdit(EmployeePerformance model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeePerformance employeePerformanceEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeePerformanceEntity = new EmployeePerformance();
                    employeePerformanceEntity.UserInformationId = model.UserInformationId;
                    db.EmployeePerformance.Add(employeePerformanceEntity);
                }
                else
                {
                    employeePerformanceEntity = db.EmployeePerformance.Find(model.Id);
                    employeePerformanceEntity.ModifiedBy = SessionHelper.LoginId;
                    employeePerformanceEntity.ModifiedDate = DateTime.Now;
                }
                employeePerformanceEntity.ReviewDate = model.ReviewDate;
                employeePerformanceEntity.SupervisorId = model.SupervisorId;
                employeePerformanceEntity.PerformanceDescriptionId = model.PerformanceDescriptionId;
                employeePerformanceEntity.PerformanceResultId = model.PerformanceResultId;
                employeePerformanceEntity.ActionTakenId = model.ActionTakenId;
                employeePerformanceEntity.ExpiryDate = model.ExpiryDate;
                employeePerformanceEntity.ReviewSummary = model.ReviewSummary;
                employeePerformanceEntity.ReviewNote = model.ReviewNote;
                
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
            var employeePerformanceEntity = db.EmployeePerformance.Find(id);
            try
            {
                employeePerformanceEntity.ModifiedBy = SessionHelper.LoginId;
                employeePerformanceEntity.ModifiedDate = DateTime.Now;
                employeePerformanceEntity.DataEntryStatus = 0;
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
        public JsonResult AjaxGetAutoCompleteData(string term, string fieldName)
        {
            IList<string> autoCompleteDataList = null;
            //switch (fieldName)
            //{
            //    case "Type":
            //        autoCompleteDataList = db.EmployeePerformance
            //                               .Where(w => w.Type.Contains(term))
            //                               .Select(s => s.Type).Distinct()
            //                               .ToList();
            //        break;

            //}

            return Json(autoCompleteDataList, JsonRequestBehavior.AllowGet);
        }
    }
}