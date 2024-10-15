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
    public class ApplicantEducationController : BaseApplicantRoleRightsController<ApplicantEducation>
    {
        

        [HttpGet]
        // GET: EmployeeEducation
        public ActionResult Index(int id)
        {
            ViewBag.IsHired = IsApplicantHired(id);
            var model = db.ApplicantEducation.Where(w => w.DataEntryStatus == 1 && w.ApplicantInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }
        public  ActionResult MostRecentRecord(int? id)
        {
            try
            {
                var model = db.ApplicantEducation.Where(w=>w.ApplicantInformationId==id && w.DataEntryStatus==1)
                                                .OrderByDescending(u => u.CreatedDate)
                                                    .FirstOrDefault();
                
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult Details(int? id)
        {
            try
            {
                var model = db.ApplicantEducation.Find(id ?? 0);
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
                ViewBag.DegreeId = new SelectList(db.GetAll<Degree>(SessionHelper.SelectedClientId), "Id", "DegreeName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEducation", "Create");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public  ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantEducation.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.DegreeId = new SelectList(db.GetAll<Degree>(SessionHelper.SelectedClientId), "Id", "DegreeName", model.DegreeId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEducation", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.ApplicantEducation.Find(id);
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
                    var applicantEducationEntity = db.ApplicantEducation.Find(int.Parse(educationID));
                    if (applicantEducationEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        string docName = "" + applicantEducationEntity.ApplicantInformationId + "_" + educationID + "-" + fileName;

                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("Applicant\\educationDocs", docName);
                        docFile.SaveAs(serverFilePath);
                        applicantEducationEntity.DocFilePath = filePathHelper.RelativePath;
                        applicantEducationEntity.DocName = docName;
                        applicantEducationEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                       
                    }
                    else
                    {
                      
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

                    var applicantEduEntity = db.ApplicantEducation.Find(id);
                    if (applicantEduEntity != null)
                    {
                        if (!string.IsNullOrEmpty(applicantEduEntity.DocFilePath))
                        {
                            relativeFilePath = applicantEduEntity.DocFilePath;
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
            var applicantEdu = db.ApplicantEducation.Find(id);
            if (applicantEdu != null)
            {
                if (!string.IsNullOrEmpty(applicantEdu.DocFilePath))
                {
                    relativeFilePath = applicantEdu.DocFilePath;
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
        public JsonResult CreateEdit(ApplicantEducation model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantEducation applicantEducationEntity = null;
            try
            {
                if (model.Id == 0)
                {
                   applicantEducationEntity = new ApplicantEducation();
                   applicantEducationEntity.ApplicantInformationId = model.ApplicantInformationId;
                   db.ApplicantEducation.Add(applicantEducationEntity);
                }
                else
                {
                    applicantEducationEntity = db.ApplicantEducation.Find(model.Id);
                    applicantEducationEntity.ModifiedBy = SessionHelper.LoginId;
                    applicantEducationEntity.ModifiedDate = DateTime.Now;
                }
                applicantEducationEntity.DegreeId = model.DegreeId;
                applicantEducationEntity.DateCompleted = model.DateCompleted;
                applicantEducationEntity.InstitutionName = model.InstitutionName;
                applicantEducationEntity.Title = model.Title;
                applicantEducationEntity.Note = model.Note;

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
                var model = db.ApplicantEducation.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantEducation", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantEducationEntity = db.ApplicantEducation.Find(id);
            try
            {
                applicantEducationEntity.ModifiedBy = SessionHelper.LoginId;
                applicantEducationEntity.ModifiedDate = DateTime.Now;
                applicantEducationEntity.DataEntryStatus = 0;
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
                    autoCompleteDataList = db.ApplicantEducation
                                           .Where(w => w.InstitutionName.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.InstitutionName).Distinct()
                                           .ToList();
                    break;
                case "Title":
                    autoCompleteDataList = db.ApplicantEducation
                                           .Where(w => w.Title.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.Title).Distinct()
                                           .ToList();
                    break;
            }

            return Json(autoCompleteDataList, JsonRequestBehavior.AllowGet);
        }
    }


}