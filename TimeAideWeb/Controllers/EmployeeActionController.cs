
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
    public class EmployeeActionController : TimeAideWebControllers<EmployeeAction>
    {
        [HttpGet]
        // GET: EmployeePerformance
        public ActionResult IndexByUser(int id)
        {
            var model = db.EmployeeAction.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }

        public override ActionResult Create()
        {
          
                ViewBag.ApprovedById = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "FullName");
                ViewBag.ActionTypeId = new SelectList(db.GetAll<ActionType>(SessionHelper.SelectedClientId), "Id", "ActionTypeName");

            return PartialView();
           
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeAction.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.ApprovedById = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "FullName",model.ApprovedById);
            ViewBag.ActionTypeId = new SelectList(db.GetAll<ActionType>(SessionHelper.SelectedClientId), "Id", "ActionTypeName",model.ActionTypeId);

            return PartialView(model);
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.EmployeeAction.Find(id);
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
                    string actionID = Request.Form["ActionDocRecordID"];
                    var employeeActionEntity = db.EmployeeAction.Find(int.Parse(actionID));
                    if (employeeActionEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + employeeActionEntity.UserInformationId + "_" + actionID + "-" + fileName;
                        
                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("actionDocs", docName); 
                        docFile.SaveAs(serverFilePath);
                        employeeActionEntity.DocFilePath = filePathHelper.RelativePath;
                        employeeActionEntity.DocName = docName;
                        employeeActionEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Action record data!";
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

                    var employeeActionEntity = db.EmployeeAction.Find(id);
                    if (employeeActionEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeeActionEntity.DocFilePath))
                        {
                            relativeFilePath = employeeActionEntity.DocFilePath;
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
                        message = "Invalid Action record data!";
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
            var userAction = db.EmployeeAction.Find(id);
            if (userAction != null)
            {
                if (!string.IsNullOrEmpty(userAction.DocFilePath))
                {
                    relativeFilePath = userAction.DocFilePath;
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
        public JsonResult CreateEdit(EmployeeAction model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAction employeeActionEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeActionEntity = new EmployeeAction();
                    employeeActionEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeAction.Add(employeeActionEntity);
                }
                else
                {
                    employeeActionEntity = db.EmployeeAction.Find(model.Id);
                    employeeActionEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeActionEntity.ModifiedDate = DateTime.Now;
                }
                employeeActionEntity.ActionTypeId = model.ActionTypeId;
                employeeActionEntity.ActionDate = model.ActionDate;
                employeeActionEntity.ActionExpiryDate = model.ActionExpiryDate;
                employeeActionEntity.ActionName = model.ActionName;
                employeeActionEntity.ActionDescription = model.ActionDescription;
                employeeActionEntity.ActionNotes = model.ActionNotes;

                employeeActionEntity.ActionEndDate = model.ActionEndDate;
                employeeActionEntity.ActionApprovedDate = model.ActionApprovedDate;
                employeeActionEntity.ApprovedById = model.ApprovedById;
                employeeActionEntity.ActionClosingInfo = model.ActionClosingInfo;

                db.SaveChanges();

            }
            catch (Exception ex)
            {
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
            var employeeActionEntity = db.EmployeeAction.Find(id);
            try
            {
                employeeActionEntity.ModifiedBy = SessionHelper.LoginId;
                employeeActionEntity.ModifiedDate = DateTime.Now;
                employeeActionEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
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
