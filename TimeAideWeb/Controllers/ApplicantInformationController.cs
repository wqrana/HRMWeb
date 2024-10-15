using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAide.Web.Extensions;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using TimeAide.Services;

namespace TimeAide.Web.Controllers
{
    public class ApplicantInformationController :BaseApplicantRoleRightsController<ApplicantInformation>
    {
       // private TimeAideContext db = new TimeAideContext();

        // GET: UserInformation
        public ActionResult Index(int? id)
        {
            var sessionFilters = SessionHelper.ApplicantFilterInSession;
            var emptyQACriteriaFilters = new List<QAFilterCriteriaViewModel>();
            ViewBag.PositionId = new SelectList(db.GetAll<Position>(SessionHelper.SelectedClientId), "Id", "PositionName", sessionFilters.PositionId).OrderBy(o=>o.Text);
            //ViewBag.ApplicantStatusId = new SelectList(db.ApplicantStatus.Where(w => w.DataEntryStatus == 1), "Id", "ApplicantStatusName", 1);
            ViewBag.GenderId = new SelectList(db.GetAll<Gender>(SessionHelper.SelectedClientId), "Id", "GenderName");
            ViewBag.ApplicantStatusId = new SelectList(db.GetAll<ApplicantStatus>(SessionHelper.SelectedClientId), "Id", "ApplicantStatusName", sessionFilters.ApplicantStatusId);
            ViewBag.ApplicantInterviewQuestionId = new SelectList(db.GetAll<ApplicantInterviewQuestion>(SessionHelper.SelectedClientId), "Id", "QuestionName");
            ViewBag.LocationId = new SelectList(db.GetAll<Location>(SessionHelper.SelectedClientId).OrderBy(o => o.LocationName), "Id", "LocationName", sessionFilters.LocationId);
            var qACriteriaFilters = sessionFilters.QACriteriaFilters == null? emptyQACriteriaFilters: sessionFilters.QACriteriaFilters;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ViewBag.QACriteriaFiltersInJson = jss.Serialize(qACriteriaFilters);


            return PartialView();
        }
        public ActionResult GetApplicantQAnswerType(int id)
        {
            var appQuestion = db.ApplicantInterviewQuestion.Find(id);
            if(appQuestion.ApplicantAnswerTypeId==2|| appQuestion.ApplicantAnswerTypeId == 3)
            {
                appQuestion.ApplicantQAnswerOptions = db.ApplicantQAnswerOption.Where(w => w.ApplicantInterviewQuestionId == id && w.DataEntryStatus == 1);
                ViewBag.FilterValue = new SelectList(appQuestion.ApplicantQAnswerOptions, "Id", "AnswerOptionName");
            }
           


            return PartialView("ApplicantQAnswerType", appQuestion);
            //return Json(cityList);
        }
        public ActionResult IndexByViewType(ApplicantFilterViewModel appFilter)
        {
            string returnViewName = "IndexByListView";           
            var appList = getViewBasedApplicantList(true, appFilter);          

            return PartialView(returnViewName, appList);
        }
        
        
        private List<ApplicantInformationViewModel> getViewBasedApplicantList(bool? isLoadingRequired, ApplicantFilterViewModel appFilter)
        {
            List<ApplicantInformationViewModel> retApplicantList;
            var setApplicantSessionFilters = new ApplicantFilterInSession();

            if (SessionHelper.SelectedClientId == 0)
            {
                SessionHelper.SelectedClientId = SessionHelper.ClientId;
            }

            if (SessionHelper.SelectedCompanyId == 0)
            {
                SessionHelper.SelectedCompanyId = SessionHelper.CompanyId;
                var company = (new TimeAideContext()).Find<Company>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (company != null)
                    SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
            }
          
                        
            try
            {                
                var applicantName = "";
                var positionId = 0;
                var applicantStatusId = 0;
                var locationId = 0;
                StringBuilder applicantQAQuery = new StringBuilder("") ;
                int applicantQACriteriaCount = 0;
                if (appFilter != null)
                {
                    applicantName = appFilter.ApplicantName == null ? applicantName : appFilter.ApplicantName;
                    setApplicantSessionFilters.PositionId = appFilter.PositionId;
                    positionId = appFilter.PositionId ?? 0;

                    setApplicantSessionFilters.ApplicantStatusId = appFilter.ApplicantStatusId;
                    applicantStatusId = appFilter.ApplicantStatusId ?? 0;

                    setApplicantSessionFilters.LocationId = appFilter.LocationId;
                    locationId = appFilter.LocationId ?? 0;

                    setApplicantSessionFilters.QACriteriaFilters = appFilter.QACriteriaFilters;
                    if (appFilter.QACriteriaFilters != null)
                    {
                        var baseQuery = $"Select ai.ApplicantInformationId, count(aiv.ApplicantInterviewQuestionId) \n" +
                                        $"From ApplicantInformation ai \n" +
                                        $"Inner Join ApplicantInterview aiv on ai.ApplicantInformationId = aiv.ApplicantInformationId \n" +
                                        $"where ai.CompanyId = {SessionHelper.SelectedCompanyId} \n";
                        applicantQAQuery.AppendLine(baseQuery);
                        applicantQACriteriaCount = appFilter.QACriteriaFilters.Count();
                        int count = 0;
                        foreach (var qAFilter in appFilter.QACriteriaFilters)
                        {
                            var whereClause = "";
                            var conditionOp = "OR";
                            ++count;
                            if (count == 1) conditionOp = "AND";
                            switch (qAFilter.FilterOpertor)
                            {
                                case "contains":
                                    whereClause = $" {conditionOp} aiv.ApplicantInterviewQuestionId = {qAFilter.ApplicantInterviewQuestionId} " +
                                                    $" And aiv.ApplicantAnswer like '%{qAFilter.FilterValue}%' ";
                                        break;
                                case "is":
                                    whereClause = $" {conditionOp} aiv.ApplicantInterviewQuestionId = {qAFilter.ApplicantInterviewQuestionId} " +
                                                    $" And  TRY_CONVERT(datetime, aiv.ApplicantAnswer, 110) = TRY_CONVERT(datetime,'{qAFilter.FilterValue}',110) ";
                                   
                                    break;
                                case "is after":
                                    whereClause = $" {conditionOp} aiv.ApplicantInterviewQuestionId = {qAFilter.ApplicantInterviewQuestionId} " +
                                                    $" And  TRY_CONVERT(datetime, aiv.ApplicantAnswer, 110) > TRY_CONVERT(datetime,'{qAFilter.FilterValue}',110) ";

                                    break;
                                case "is before":
                                    whereClause = $" {conditionOp} aiv.ApplicantInterviewQuestionId = {qAFilter.ApplicantInterviewQuestionId} " +
                                                    $" And  TRY_CONVERT(datetime, aiv.ApplicantAnswer, 110) < TRY_CONVERT(datetime,'{qAFilter.FilterValue}',110) ";

                                    break;
                            }
                            if (whereClause != "")
                                applicantQAQuery.AppendLine(whereClause);

                        }
                        var groupClause = " Group by ai.ApplicantInformationId";
                        applicantQAQuery.AppendLine(groupClause);
                    }
                }
                
                //Calling Db Procedure
                var appInformationList = db.SP_ApplicantInformation<ApplicantInformationViewModel> (applicantName, positionId, applicantStatusId,locationId ,applicantQAQuery.ToString(),applicantQACriteriaCount
                                        , SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);

                SessionHelper.ApplicantFilterInSession = setApplicantSessionFilters;

                retApplicantList = appInformationList;
              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retApplicantList;

        }            
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appInformation = db.SP_ApplicantInformationById<ApplicantInformationViewModel>(id??0);

            try
            {
                AllowView();             
                var picturePath = appInformation.PictureFilePath;
                //setting the default picture
                appInformation.PictureFilePath = string.IsNullOrEmpty(picturePath) ? "/images/no-profile-image.jpg" : picturePath;                                      
                               
                return PartialView(appInformation);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "Details");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public ActionResult PersonalInformation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appInformation = db.ApplicantInformation.Find(id);
            ViewBag.IsHired = IsApplicantHired(id ?? 0);
            try
            {               
                var resumeName = string.IsNullOrEmpty(appInformation.ResumeFilePath) ? "" : appInformation.ResumeFilePath;
                var startingIndex = resumeName.LastIndexOf("\\") + 1;
                appInformation.ResumeFilePath = startingIndex > 0 ? resumeName.Substring(startingIndex, resumeName.Length - startingIndex) : "";
               
                return PartialView(appInformation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult EditPersonalInformation(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicantInformation = db.ApplicantInformation.Find(id);

            try
            {
                AllowEdit();
                ViewBag.GenderId = new SelectList(db.GetAll<Gender>(SessionHelper.SelectedClientId), "Id", "GenderName", applicantInformation.GenderId);
                ViewBag.ApplicantStatusId = new SelectList(db.ApplicantStatus.Where(w => w.DataEntryStatus == 1), "Id", "ApplicantStatusName", applicantInformation.ApplicantStatusId);
                ViewBag.DisabilityId = new SelectList(db.Disability.Where(w => w.DataEntryStatus == 1), "Id", "DisabilityName", applicantInformation.DisabilityId);
                return PartialView(applicantInformation);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "Edit");
               return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult AjaxUploadApplicantCV()
        {
            dynamic retResult = null;
            string status = "Error";
            string message = "No file is selected";
            string cvName = "";
            string serverFilePath = "";
            string relativeFilePath = "";
            string ApplicantID = Request.Form["ApplicantID"];


            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase cvFile = Request.Files[0];
                    var applicant = db.ApplicantInformation.Find(int.Parse(ApplicantID));
                    if (applicant != null)
                    {
                        var fileExt = Path.GetExtension(cvFile.FileName);
                        cvName = "Applicant-" + ApplicantID + "-CV" + fileExt;


                        FilePathHelper filePathHelper = new FilePathHelper();

                        serverFilePath = filePathHelper.GetPath("Applicant\\resumes", cvName);
                        cvFile.SaveAs(serverFilePath);
                        relativeFilePath = filePathHelper.RelativePath;
                        applicant.ResumeFilePath = relativeFilePath;
                        db.SaveChanges();
                       
                        status = "Success";
                        message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Applicant record data!";
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
            retResult = new { status = status, message = message, cvName = cvName };
            return Json(retResult);
        }
        [HttpPost]
        public JsonResult AjaxCheckApplicantCV(int appID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadCVFile;
           

            if (appID > 0)
            {
                try
                {

                    var user = db.ApplicantInformation.Find(appID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.ResumeFilePath))
                        {
                            relativeFilePath = user.ResumeFilePath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadCVFile = new FileInfo(serverFilePath);
                            if (!downloadCVFile.Exists)
                            {
                                status = "Error";
                                message = "Applicant CV is not yet Uploaded!";
                            }
                        }
                        else
                        {
                            status = "Error";
                            message = "Applicant CV is not yet Uploaded!";
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
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
            }
            else
            {
                status = "Error";
                message = "Invalid Applicant record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult);
        }
        public ActionResult DownloadApplicantCV(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var app = db.ApplicantInformation.Find(id);
            if (app != null)
            {
                if (!string.IsNullOrEmpty(app.ResumeFilePath))
                {
                    relativeFilePath = app.ResumeFilePath;
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
        public JsonResult AjaxUploadApplicantPicture()
        {
            dynamic retResult = null;
            string serverFilePath = "";
            string relativeFilePath = "";
            string appID = Request.Form["ApplicantID"];
            string action = Request.Form["Action"];
            var profilePictureName = appID + '.' + "jpg";


            FilePathHelper filePathHelper = new FilePathHelper();
            serverFilePath = filePathHelper.GetPath("Applicant\\pictures", profilePictureName);
            retResult = new { status = "Error", message = "Invalid Operation", picturePath = "" };
            FileInfo checkFile = null;

            if (action == "U")
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        HttpPostedFileBase pictureFile = Request.Files[0];
                        var app = db.ApplicantInformation.Find(int.Parse(appID));
                        if (app != null)
                        {
                            pictureFile.SaveAs(serverFilePath);
                            relativeFilePath = filePathHelper.RelativePath;
                            app.PictureFilePath = relativeFilePath;
                            db.SaveChanges();
                            retResult = new { status = "Success", message = "Profile picture is successfully Uploaded!", picturePath = relativeFilePath };
                        }
                        else
                        {
                            retResult = new { status = "Error", message = "Invalid record data", picturePath = "" };
                        }

                    }
                    catch (Exception ex)
                    {
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        retResult = new { status = "Error", message = ex.Message, picturePath = "" };
                    }
                }
            }
            else if (action == "D")
            {

                var user = db.ApplicantInformation.Find(int.Parse(appID));
                if (user != null)
                {
                    checkFile = new FileInfo(serverFilePath);
                    var isExists = checkFile.Exists;
                    if (isExists) checkFile.Delete();

                    user.PictureFilePath = null;
                    relativeFilePath = "/images/no-profile-image.jpg";
                    db.SaveChanges();
                    retResult = new { status = "Success", message = "Applicant picture is successfully deleted!", picturePath = relativeFilePath };
                }
                else
                {
                    retResult = new { status = "Error", message = "Invalid record data", picturePath = "" };
                }

            }

            return Json(retResult);
        }
        [HttpPost]
        public JsonResult AjaxCreateEditApplicant(ApplicantInformation model)
        {
            //dynamic retResult = new {id=0, status = "", message = "" };
            dynamic retResult = null;
            ApplicantInformation appEntity = null;
            
            try
            {
                

                    if (!string.IsNullOrEmpty(model.SSN))
                    {
                        string checkSSN = Encryption.Encrypt(model.SSN);
                        // var existingCount = db.UserInformation.Where(w => (w.SSNEncrypted == checkSSN)).Count();
                        //SSN should by unique within specific company(Task 208)
                        var existingCount = db.ApplicantInformation.Where(w => ((w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId)
                                                                            && w.SSNEncrypted == checkSSN) && w.Id!= model.Id).Count();
                        if (existingCount > 0)
                        {
                            retResult = new { id = 0, status = "Error", message = "SSN is already exists" };
                            return Json(retResult);
                        }
                    }
                if (model.Id == 0)
                {
                    appEntity= new ApplicantInformation();
                    db.ApplicantInformation.Add(appEntity);
                }
                else
                {
                    appEntity = db.ApplicantInformation.Find(model.Id);
                    appEntity.ModifiedBy = SessionHelper.LoginId;
                    appEntity.ModifiedDate = DateTime.Now;
                }


                appEntity.FirstName = model.FirstName;
                appEntity.MiddleInitial = model.MiddleInitial;
                appEntity.FirstLastName = model.FirstLastName;
                appEntity.SecondLastName = model.SecondLastName;
                appEntity.ShortFullName = model.FullName;
                appEntity.BirthDate = model.BirthDate;
                appEntity.GenderId = model.GenderId;
                appEntity.DisabilityId = model.DisabilityId;
                appEntity.SSNEncrypted = model.SSNEncrypted;
                appEntity.ApplicantStatusId = model.ApplicantStatusId;             
                 
                db.SaveChanges();
                   

                retResult = new { id = appEntity.Id, status = "Success", message = "Applicant is added/Updated successfully!" };
                

            }
            catch (DbEntityValidationException ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //retResult.status = "Error";
                //retResult.message = ex.Message;
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }
            catch (Exception ex)
            {
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }

            return Json(retResult);
        }

       
        
        private dynamic validateUpdatedSSN(int appID, string SSN)
        {
            bool validateStatus = true;
            string validateUpdatedSSN = "";
            string validatedEncryptedSSN = "";
            string errorMess = "";
            //if (newSSNEnd.Length == 4)
            //{
            if (SSN != null)
            {

                // validateUpdatedSSN = SSN.Substring(0, 7) + newSSNEnd;
                validateUpdatedSSN = SSN;
                if (validateUpdatedSSN.Length == 11)
                {
                    validatedEncryptedSSN = Encryption.Encrypt(validateUpdatedSSN);
                    
                    //SSN should by unique within specific company(Task 208)
                    var existingCount = db.ApplicantInformation.Where(w => (w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId)
                                                                           && (w.SSNEncrypted == validatedEncryptedSSN && w.Id != appID)).Count();
                    if (existingCount > 0)
                    {
                        validateStatus = false;
                        errorMess = "SSN is already exists!";
                    }

                }
                else
                {
                    validateStatus = false;
                    errorMess = "Invalid New SSN Format!";
                }
            }
            else
            {
                validateStatus = false;
                errorMess = "SSN doesn't exist, Can't be updated!";
            }
           
            return new { isValid = validateStatus, newEncryptedSSN = validatedEncryptedSSN, errorMessage = errorMess };
        }
        // GET: UserInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicantInformation appInformation = db.ApplicantInformation.Find(id);
                if (appInformation == null)
                {
                    return HttpNotFound();
                }
                return View(appInformation);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "Delete");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: UserInformation/Delete/5
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var applicantEntity = db.ApplicantInformation.Find(id);
            try
            {
                applicantEntity.ModifiedBy = SessionHelper.LoginId;
                applicantEntity.ModifiedDate = DateTime.Now;
                applicantEntity.DataEntryStatus = 0;
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
        public ActionResult HireApplicant(int id)
        {
            try
            {
                AllowAdd();
                var model = new ApplicantHireViewModel();
                model.GenderList = db.GetAll<Gender>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.GenderName });
                model.CompanyList = db.GetAll<Company>(SessionHelper.SelectedClientId).Select(s => new  { id = s.Id, text = s.CompanyName });
                var applicantEntity = db.ApplicantInformation.Find(id);
                if (applicantEntity != null)
                {
                    model.FirstName = applicantEntity.FirstName;
                    model.MiddleInitial = applicantEntity.MiddleInitial;
                    model.FirstLastName = applicantEntity.FirstLastName;
                    model.SecondLastName = applicantEntity.SecondLastName;
                    model.SSN = applicantEntity.SSN;
                    model.BirthDate = applicantEntity.BirthDate;
                    model.GenderId = applicantEntity.GenderId;
                    model.CompanyId = applicantEntity.CompanyId ?? 0;
                    var applicantContactEntity = db.ApplicantContactInformation.Where(w => w.ApplicantInformationId == id).FirstOrDefault();
                    if (applicantContactEntity != null)
                    {
                        model.HomeAddress1 = applicantContactEntity.HomeAddress1;
                        model.HomeAddress2 = applicantContactEntity.HomeAddress2;
                        model.HomeCountryId = applicantContactEntity.HomeCountryId;
                        model.HomeStateId = applicantContactEntity.HomeStateId;
                        model.HomeCityId = applicantContactEntity.HomeCityId;
                        model.HomeZipCode = applicantContactEntity.HomeZipCode;

                        model.MailingAddress1 = applicantContactEntity.MailingAddress1;
                        model.MailingAddress2 = applicantContactEntity.MailingAddress2;
                        model.MailingCountryId = applicantContactEntity.MailingCountryId;
                        model.MailingStateId = applicantContactEntity.MailingStateId;
                        model.MailingCityId = applicantContactEntity.MailingCityId;
                        model.MailingZipCode = applicantContactEntity.MailingZipCode;

                        model.HomeNumber = applicantContactEntity.HomeNumber;
                        model.CelNumber = applicantContactEntity.CelNumber;
                        model.FaxNumber = applicantContactEntity.FaxNumber;
                        model.OtherNumber = applicantContactEntity.OtherNumber;

                        model.PersonalEmail = applicantContactEntity.PersonalEmail;
                        model.WorkEmail = applicantContactEntity.WorkEmail;
                        model.OtherEmail = applicantContactEntity.OtherEmail;

                    }
                }

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "HireApplicant");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult HireApplicant(ApplicantHireViewModel model)
        {
            string transactionPoint = "";
            string status = "";
            string message = "";
            string empIdUniqueLevelStr = "";
            using (var applicantHireDBTrans = db.Database.BeginTransaction())
            {
                try
                {
                    var isUniqueEmpIdAtClientLevel = ApplicationConfigurationService.GetConfigurationStatus("UseUniqueEmployeeIdClientLevel");
                    empIdUniqueLevelStr = isUniqueEmpIdAtClientLevel ? "Client" : "Company";
                    //var addUserEntity = db.UserInformation.Where(w => w.DataEntryStatus == 1 &&
                    //                                w.EmployeeId == model.EmployeeId &&
                    //                                w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId).FirstOrDefault();

                    UserInformation addUserEntity = null;
                    if (isUniqueEmpIdAtClientLevel)
                    {
                        addUserEntity = db.GetAll<UserInformation>(SessionHelper.SelectedClientId).Where(w => w.EmployeeId == model.EmployeeId).FirstOrDefault();
                    }
                    else
                    {
                        addUserEntity = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.EmployeeId == model.EmployeeId).FirstOrDefault();
                    }
                    if (addUserEntity == null)
                    {
                        if (!string.IsNullOrEmpty(model.SSN))
                        {
                            string checkSSN = Encryption.Encrypt(model.SSN);
                            //var existingCount = db.UserInformation.Where(w => ((w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId)
                            //                                                    && w.SSNEncrypted == checkSSN)).Count();
                            var existingCount = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.SSNEncrypted == checkSSN).Count();
                            if (existingCount > 0)
                            {
                                throw new Exception($"SSN is already exists.");
                            }
                        }
                        var applicantEntity = db.ApplicantInformation.Find(model.ApplicantInformationId); 

                        addUserEntity = new UserInformation();
                        addUserEntity.EmployeeId = model.EmployeeId;
                        addUserEntity.FirstName = model.FirstName;
                        addUserEntity.MiddleInitial = model.MiddleInitial;
                        addUserEntity.FirstLastName = model.FirstLastName;
                        addUserEntity.SecondLastName = model.SecondLastName;
                        addUserEntity.ShortFullName = addUserEntity.FullName;
                        addUserEntity.BirthDate = model.BirthDate;
                        addUserEntity.GenderId = model.GenderId;
                        addUserEntity.CompanyId = model.CompanyId;
                        addUserEntity.SSNEncrypted = string.IsNullOrEmpty(model.SSN)? model.SSN: Encryption.Encrypt(model.SSN);
                        addUserEntity.SSNEnd = addUserEntity.SSNEndComputed;
                        addUserEntity.EmployeeStatusId = 2;
                        addUserEntity.EmployeeStatusDate = DateTime.Now;
                        db.UserInformation.Add(addUserEntity);
                        db.SaveChanges();
                        //Update Applicant
                        applicantEntity.UserInformationId = addUserEntity.Id;
                        var hiredStatus= db.GetAll<ApplicantStatus>(SessionHelper.SelectedClientId).Where(w => w.UseAsHire == true).FirstOrDefault();
                        if (hiredStatus != null)
                        {
                            applicantEntity.ApplicantStatusId = hiredStatus.Id;
                        }
                        transactionPoint = "While Saving User Record:";
                        db.SaveChanges();
                        //Copy Picture Image to employee folder
                        FilePathHelper filePathHelper = new FilePathHelper();
                        if (applicantEntity.PictureFilePath != null)
                        {
                            transactionPoint = "Moving Applicant Picture:";
                            var tempPath = "~" + applicantEntity.PictureFilePath;
                            var SourcePictureServerFilePath = Server.MapPath(tempPath);
                            //var sourcePicture = new FileInfo(SourcePictureServerFilePath);
                            var profilePictureName = addUserEntity.Id + "." + "jpg";
                            var targetPictureServerFilePath = filePathHelper.GetPath("pictures", profilePictureName);
                            //sourcePicture.Open(FileMode.Open);
                            //sourcePicture.CopyTo(targetPictureServerFilePath);
                            System.IO.File.Copy(SourcePictureServerFilePath, targetPictureServerFilePath);
                            addUserEntity.PictureFilePath = filePathHelper.RelativePath;
                        }
                        //Copy CV file to employee folder
                        if (applicantEntity.ResumeFilePath != null)
                        {
                            transactionPoint = "Moving Applicant CV:";
                            var tempPath = "~" + applicantEntity.ResumeFilePath;
                            var SourceCVServerFilePath = Server.MapPath(tempPath);
                            var sourceCV = new FileInfo(SourceCVServerFilePath);
                            var fileExt = Path.GetExtension(sourceCV.Name);
                            var cvName = "Emp-" + addUserEntity.Id + "-CV" + fileExt;
                            var  targetCVFileServerPath = filePathHelper.GetPath("resumes", cvName);
                            //sourceCV.Open(FileMode.Open);
                            //sourceCV.CopyTo(targetCVFileServerPath);
                            System.IO.File.Copy(SourceCVServerFilePath, targetCVFileServerPath);
                            addUserEntity.ResumeFilePath = filePathHelper.RelativePath;
                        }

                        var userContactEntity = new UserContactInformation();

                        userContactEntity.UserInformationId = addUserEntity.Id;
                        userContactEntity.HomeAddress1 = model.HomeAddress1;
                        userContactEntity.HomeAddress2 = model.HomeAddress2;
                        userContactEntity.HomeCountryId = model.HomeCountryId;
                        userContactEntity.HomeStateId = model.HomeStateId;
                        userContactEntity.HomeCityId = model.HomeCityId;
                        userContactEntity.HomeZipCode = model.HomeZipCode;
                       
                        userContactEntity.MailingAddress1 = model.MailingAddress1;
                        userContactEntity.MailingAddress2 = model.MailingAddress2;
                        userContactEntity.MailingCountryId = model.MailingCountryId;
                        userContactEntity.MailingStateId = model.MailingStateId;
                        userContactEntity.MailingCityId = model.MailingCityId;
                        userContactEntity.MailingZipCode = model.MailingZipCode;

                        userContactEntity.HomeNumber = model.HomeNumber;
                        userContactEntity.CelNumber = model.CelNumber;
                        userContactEntity.FaxNumber = model.FaxNumber;
                        userContactEntity.OtherNumber = model.OtherNumber;

                        userContactEntity.PersonalEmail = model.PersonalEmail;
                        userContactEntity.WorkEmail = model.WorkEmail;
                        userContactEntity.OtherEmail = model.OtherEmail;
                        db.UserContactInformation.Add(userContactEntity);

                        transactionPoint = "While Saving User Contact Info:";
                        db.SaveChanges();
                        //Saving other tabs data
                        transactionPoint = "Before saving education data:";
                        CopyHireApplicantEducationDetail(model.ApplicantInformationId, addUserEntity.Id);
                        transactionPoint = "Before saving CustomField data:";
                        CopyHireApplicantCustomFieldDetail(model.ApplicantInformationId, addUserEntity.Id);
                        transactionPoint = "Before saving Document data:";
                        CopyHireApplicantDocumentDetail(model.ApplicantInformationId, addUserEntity.Id);
                        transactionPoint = "Before saving action history data:";
                        CopyHireApplicantActionDetail(model.ApplicantInformationId, addUserEntity.Id);
                        //end tabs data
                        applicantHireDBTrans.Commit();
                        status = "Success";
                        message = "Applicant is successfully hired!";

                }
                else
                {
                       
                        status = "Error";
                        message = $"Employee Id is already exists at {empIdUniqueLevelStr} Level.";
                }
            }


            catch (Exception ex)
            {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    applicantHireDBTrans.Rollback();
                    status = "Error";
                    message = transactionPoint + ex.Message;
                }
        }
             return Json(new { status =status, message = message });
        }
        private bool CopyHireApplicantEducationDetail(int applicantInformationId, int userInformationId)
        {
            var applicantEducationList = db.ApplicantEducation.Where(w => w.ApplicantInformationId == applicantInformationId && w.DataEntryStatus == 1);
            if (applicantEducationList.Count() > 0)
            {
                foreach(var applicantEducationEntity in applicantEducationList)
                {
                    var employeeEducationEntity = new EmployeeEducation();
                    employeeEducationEntity.UserInformationId = userInformationId;
                    employeeEducationEntity.DegreeId = applicantEducationEntity.DegreeId;
                    employeeEducationEntity.Title = applicantEducationEntity.Title;
                    employeeEducationEntity.InstitutionName = applicantEducationEntity.InstitutionName;
                    employeeEducationEntity.Note = applicantEducationEntity.Note;
                    employeeEducationEntity.DocFilePath = applicantEducationEntity.DocFilePath;
                    employeeEducationEntity.DocName = applicantEducationEntity.DocName;
                    employeeEducationEntity.DateCompleted = applicantEducationEntity.DateCompleted;
                    db.EmployeeEducation.Add(employeeEducationEntity);
                }
                db.SaveChanges();
            }

            return true;
        }
        private bool CopyHireApplicantCustomFieldDetail(int applicantInformationId, int userInformationId)
        {
            var applicantCustomFieldList = db.ApplicantCustomField.Where(w => w.ApplicantInformationId == applicantInformationId 
                                                                            && w.DataEntryStatus == 1);
            if (applicantCustomFieldList.Count() > 0)
            {
                foreach(var applicantCustomField in applicantCustomFieldList)
                {
                    var employeeCustomField = new EmployeeCustomField();
                    employeeCustomField.UserInformationId = userInformationId;
                    employeeCustomField.CustomFieldId = applicantCustomField.CustomFieldId;
                    employeeCustomField.CustomFieldValue = applicantCustomField.CustomFieldValue;
                    employeeCustomField.ExpirationDate = applicantCustomField.ExpirationDate;
                    employeeCustomField.CustomFieldNote = applicantCustomField.CustomFieldNote;
                    db.EmployeeCustomField.Add(employeeCustomField);
                }
                db.SaveChanges();
            }

            return true;
        }
        private bool CopyHireApplicantDocumentDetail(int applicantInformationId, int userInformationId)
        {
            var applicantDocumentList = db.ApplicantDocument.Where(w => w.ApplicantInformationId == applicantInformationId &&
                                                                    w.DataEntryStatus == 1);
            if (applicantDocumentList.Count() > 0)
            {
                foreach(var applicantDocument in applicantDocumentList)
                {
                    var employeeDocument = new EmployeeDocument();
                    employeeDocument.UserInformationId = userInformationId;
                    employeeDocument.DocumentId = applicantDocument.DocumentId;
                    employeeDocument.DocumentName = applicantDocument.DocumentName;
                    employeeDocument.DocumentNote = applicantDocument.DocumentNote;
                    employeeDocument.ExpirationDate = applicantDocument.ExpirationDate;
                    employeeDocument.DocumentPath = applicantDocument.DocumentPath;
                    db.EmployeeDocument.Add(employeeDocument);

                }
                db.SaveChanges();
            }

            return true;
        }
        private bool CopyHireApplicantActionDetail(int applicantInformationId, int userInformationId)
        {
            var applicantActionList = db.ApplicantAction.Where(w => w.ApplicantInformationId == applicantInformationId &&
                                                                    w.DataEntryStatus == 1);
            if (applicantActionList.Count() > 0)
            {
                foreach (var applicantAction in applicantActionList)
                {
                    var employeeAction = new EmployeeAction();
                    employeeAction.UserInformationId = userInformationId;
                    employeeAction.ActionTypeId = applicantAction.ActionTypeId;
                    employeeAction.ActionDate = applicantAction.ActionDate;
                    employeeAction.ActionName = applicantAction.ActionName;
                    employeeAction.ActionDescription = applicantAction.ActionDescription;
                    employeeAction.ActionExpiryDate = applicantAction.ActionExpiryDate;
                    employeeAction.ActionNotes = applicantAction.ActionNotes;
                    employeeAction.ActionEndDate = applicantAction.ActionEndDate;
                    employeeAction.ActionApprovedDate = applicantAction.ActionApprovedDate;
                    employeeAction.ApprovedById = applicantAction.ApprovedById;
                    employeeAction.ActionClosingInfo = applicantAction.ActionClosingInfo;
                    employeeAction.DocFilePath = applicantAction.DocFilePath;
                    employeeAction.DocName = applicantAction.DocName;

                    db.EmployeeAction.Add(employeeAction);

                }
                db.SaveChanges();
            }

            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: UserContactInformation/Edit/5
        
     
        
    }
}
