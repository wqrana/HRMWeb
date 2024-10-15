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
    public class EmployeeEducationController : TimeAideWebControllers<EmployeeEducation>
    {
        [HttpGet]
        // GET: EmployeeEducation
        public ActionResult IndexByUser(int id)
        {
            var model = db.EmployeeEducation.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o=> o.Id);
            return PartialView("Index", model);
        }

        public override ActionResult Create()
        {
            //var model = new EmployeeEducation();
            ViewBag.DegreeId = new SelectList(db.GetAll<Degree>(SessionHelper.SelectedClientId), "Id", "DegreeName" );
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            var model = db.EmployeeEducation.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.DegreeId = new SelectList(db.GetAll<Degree>(SessionHelper.SelectedClientId), "Id", "DegreeName",model.DegreeId);
            return PartialView(model);
        }

        public ActionResult UploadDocument(int? id)
        {
         var model= db.EmployeeEducation.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Education document is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string educationID = Request.Form["eduDocRecordID"];
                    var employeeEducationEntity = db.EmployeeEducation.Find(int.Parse(educationID));
                    if (employeeEducationEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + employeeEducationEntity.UserInformationId+ "_" + educationID+"-"+ fileName ;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("educationDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        employeeEducationEntity.DocFilePath = filePathHelper.RelativePath;
                        employeeEducationEntity.DocName = docName;
                        employeeEducationEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
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

                    var employeeEduEntity = db.EmployeeEducation.Find(id);
                    if (employeeEduEntity != null)
                    {
                        if (!string.IsNullOrEmpty(employeeEduEntity.DocFilePath))
                        {
                            relativeFilePath = employeeEduEntity.DocFilePath;
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
        public ActionResult DownloadDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var userEdu = db.EmployeeEducation.Find(id);
            if (userEdu != null)
            {
                if (!string.IsNullOrEmpty(userEdu.DocFilePath))
                {
                    relativeFilePath = userEdu.DocFilePath;
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
        public JsonResult CreateEdit(EmployeeEducation model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeEducation employeeEducationEntity = null;
            try
            {
                if (model.Id == 0)
                {
                    employeeEducationEntity = new EmployeeEducation();
                    employeeEducationEntity.UserInformationId = model.UserInformationId;
                    //employeeEducationEntity.CreatedBy = SessionHelper.LoginId;
                    //employeeEducationEntity.CreatedDate = DateTime.Now;
                    db.EmployeeEducation.Add(employeeEducationEntity);
                }
                else
                {
                    employeeEducationEntity = db.EmployeeEducation.Find(model.Id);
                    employeeEducationEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeEducationEntity.ModifiedDate = DateTime.Now;
                }
                employeeEducationEntity.DegreeId = model.DegreeId;
                employeeEducationEntity.DateCompleted = model.DateCompleted;
                employeeEducationEntity.InstitutionName = model.InstitutionName;
                employeeEducationEntity.Title = model.Title;
                employeeEducationEntity.Note = model.Note;

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
           var  employeeEducationEntity = db.EmployeeEducation.Find(id);
            try
            {
                employeeEducationEntity.ModifiedBy = SessionHelper.LoginId;
                employeeEducationEntity.ModifiedDate = DateTime.Now;
                employeeEducationEntity.DataEntryStatus = 0;
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
            switch (fieldName)
            {
                case "InstitutionName":
                     autoCompleteDataList = db.EmployeeEducation
                                            .Where(w => w.InstitutionName.ToLower().Contains(term.ToLower()))
                                            .Select(s => s.InstitutionName).Distinct()
                                            .ToList();
                 break;
                case "Title":
                    autoCompleteDataList = db.EmployeeEducation
                                           .Where(w => w.Title.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.Title).Distinct()
                                           .ToList();
                break;
            }
                                    
            return Json(autoCompleteDataList, JsonRequestBehavior.AllowGet);
        }
    }

   
}