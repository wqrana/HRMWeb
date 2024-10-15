using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Models.ViewModel;
using TimeAide.Common.Helpers;
using System.IO;
using System.Web.Script.Serialization;

namespace TimeAide.Web.Controllers
{
    public class JobPortalController : Controller
    {
        private TimeAideContext db;
        // GET: JobMangement
        public JobPortalController()
        {
            db = new TimeAideContext();
        }
        public JsonResult AjaxKeepPortalSessionAlive()
        {
           
            JsonResult jsonResult = new JsonResult()
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public ActionResult IndexByClient(int? id)
        {
            List<JobPostingDetail> jobList = null;
            try
            {
                if (id != null)
                {
                    var clientObj = db.Client.Where(w => w.Id == id).FirstOrDefault();
                    if (clientObj != null)
                    {
                        //JobPortalSession.SelectedClientId = clientObj.Id;
                        //JobPortalSession.SelectedCompanyId = 0;
                        //JobPortalSession.SelectedCompanyName = clientObj.ClientName;
                        //JobPortalSession.LoginId = 0;
                        JobPortalSession.JobPortalClientId = clientObj.Id;
                        JobPortalSession.JobPortalEntityName = clientObj.ClientName;
                    }
                    else
                    {
                        throw new Exception("Job Portal, selected client is not available");
                    }
                }
                else
                {
                    throw new Exception("Job Portal, Invalid Company");
                }
                var searchingDate = DateTime.Today;
                jobList = db.GetAll<JobPostingDetail>(JobPortalSession.JobPortalClientId)
                            .Where(w => w.JobPostingStatusId == 1 && (searchingDate >= w.JobPostingStartDate.Value && searchingDate <= (w.JobPostingExpiringDate ?? searchingDate))).
                            ToList();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.Message);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "JobPortal", "IndexByClient");
                return PartialView("~/Views/JobPortal/_JobPortalError.cshtml", handleErrorInfo);
            }

            return PartialView(jobList);
        }
        public ActionResult Index(int? id,int? selectedJobId)
        {
            List<JobPostingDetail> jobList = null;
            try
            {
                if (id != null)
                {
                    var companyObj = db.Company.Where(w => w.Id == id).FirstOrDefault();
                    if (companyObj != null)
                    {
                        //JobPortalSession.SelectedClientId = companyObj.Client.Id;
                        //JobPortalSession.SelectedCompanyId = companyObj.Id;
                        //JobPortalSession.SelectedCompanyName = companyObj.CompanyName;
                        //JobPortalSession.LoginId = 0;
                        JobPortalSession.JobPortalClientId = companyObj.Client.Id;
                        JobPortalSession.JobPortalCompanyId= companyObj.Id;
                        JobPortalSession.JobPortalEntityName = companyObj.CompanyName;
                    }
                    else
                    {
                        throw new Exception("Job Portal, selected company is not available");
                    }
                }
                else
                {
                    throw new Exception("Job Portal, Invalid Company");
                }
                var searchingDate = DateTime.Today;
                jobList = db.GetAllByCompany<JobPostingDetail>(JobPortalSession.JobPortalCompanyId, JobPortalSession.JobPortalClientId)
                            .Where(w => w.JobPostingStatusId == 1 && (searchingDate >= w.JobPostingStartDate.Value && searchingDate <= (w.JobPostingExpiringDate ?? searchingDate))).ToList();
                ViewBag.selectedJobId = selectedJobId??0;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.Message);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "JobPortal", "Index");
                return PartialView("~/Views/JobPortal/_JobPortalError.cshtml", handleErrorInfo);
            }
            
            return PartialView(jobList);
        }
        public  ActionResult JobDetail(int? id)
        {
            try
            {
                JobPostingDetail model = null;               
                model = db.JobPostingDetail.Find(id);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.Message);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "JobPosting", "Details");
                return View("~/Views/JobPortal/_JobPortalError.cshtml", handleErrorInfo);
            }
        }
        public ActionResult ApplyJob(int? id)
        {
            try
            {
            ApplicantApplyJobViewModel model = new ApplicantApplyJobViewModel();
            ViewBag.GenderId = new SelectList(db.GetAll<Gender>(JobPortalSession.JobPortalClientId), "Id", "GenderName");
            ViewBag.ApplicantReferenceTypeId = new SelectList(db.GetAll<ApplicantReferenceType>(JobPortalSession.JobPortalClientId), "Id", "ReferenceTypeName");
            ViewBag.ApplicantReferenceSourceId = new SelectList(db.GetAll<ApplicantReferenceSource>(JobPortalSession.JobPortalClientId), "Id", "ReferenceSourceName", model.ApplicantReferenceSourceId);
            ViewBag.DisabilityId = new SelectList(db.Disability.Where(w => w.DataEntryStatus == 1), "Id", "DisabilityName");
            ViewBag.HomeCountryId = new SelectList(db.GetAll<Country>(JobPortalSession.JobPortalClientId), "Id", "CountryName");
            ViewBag.MailingCountryId = ViewBag.HomeCountryId;
            model.JobDetail = db.JobPostingDetail.Find(id);
            model.JobPostingDetailId = model.JobDetail.Id;
            model.InterviewQuestionList = db.GetAll<ApplicantInterviewQuestion>(JobPortalSession.JobPortalClientId)
                                        .Where(w => w.IsPositionSpecific == false)
                                        .Union(db.PositionQuestion
                                           .Where(w => w.PositionId == model.JobDetail.PositionId).Select(s => s.ApplicantInterviewQuestion)
                                         );
            var jobLocations= db.GetAll<JobPostingLocation>(JobPortalSession.JobPortalClientId).Where(w => w.JobPostingDetailId == id).Select(s => s.Location).OrderBy(o=>o.LocationName);
            ViewBag.JobLocationId = new SelectList(jobLocations, "Id", "LocationName");
            ViewBag.HomeCountryId = new SelectList(db.GetAll<Country>(JobPortalSession.JobPortalClientId), "Id", "CountryName");
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.Message);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplyJob", "Details");
                return View("~/Views/JobPortal/_JobPortalError.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public JsonResult ApplyJob()
        {
            string status = "Success";
            string message = "Your application is submitted Successfully!";
            ApplicantApplyJobViewModel model = new ApplicantApplyJobViewModel();
            try
            {
                model.JobPostingDetailId = int.Parse(Request.Form["JobPostingDetailId"]);
                model.JobDetail = db.JobPostingDetail.Find(model.JobPostingDetailId);
                model.FirstName = Request.Form["FirstName"];
                model.MiddleInitial = Request.Form["MiddleInitial"];
                model.FirstLastName = Request.Form["FirstLastName"];
                model.SecondLastName = Request.Form["SecondLastName"];                
              
                int refGenderId;
                int refDisablityId;
                int reftypeId;
                int refSourceId;
                int refJobLocationId;
                
                var retRes1 = int.TryParse(Request.Form["ApplicantReferenceTypeId"], out reftypeId);
                model.ApplicantReferenceTypeId = retRes1 == true ? reftypeId : model.ApplicantReferenceTypeId;
                retRes1 = int.TryParse(Request.Form["ApplicantReferenceSourceId"], out refSourceId);
                model.ApplicantReferenceSourceId = retRes1 == true ? refSourceId : model.ApplicantReferenceSourceId;
                //retRes1 = int.TryParse(Request.Form["JobLocationId"], out refJobLocationId);
                model.JobLocationIds = Request.Form["JobLocationIds"];

                retRes1 = int.TryParse(Request.Form["GenderId"], out refGenderId);
                model.GenderId = retRes1 == true ? refGenderId : model.GenderId;
                retRes1 = int.TryParse(Request.Form["DisabilityId"], out refDisablityId);
                model.DisabilityId = retRes1 == true ? refDisablityId : model.DisabilityId;

                model.CellNumber = Request.Form["CellNumber"];
                model.PersonalEmail = Request.Form["PersonalEmail"];
                DateTime dateAvail;
                retRes1 = DateTime.TryParse(Request.Form["DateAvailable"], out dateAvail);
                model.DateAvailable = retRes1 == true ? dateAvail : model.DateAvailable;
                model.IsWorkedBefore = bool.Parse(Request.Form["IsWorkedBefore"]);
                model.IsOvertime = bool.Parse(Request.Form["IsOvertime"]);
                model.IsRelativeInCompany = bool.Parse(Request.Form["IsRelativeInCompany"]);
                model.RelativeName = Request.Form["RelativeName"];

                //CV Upload
                //if (Request.Files.Count > 0)
                //{
                //    HttpPostedFileBase uploadCVFile = Request.Files[0];
                //    using (var binaryReader = new BinaryReader(uploadCVFile.InputStream))
                //    {
                //        model.ApplicantCV = binaryReader.ReadBytes(uploadCVFile.ContentLength);
                //    }
                //}
                model.HomeAddress1 = Request.Form["HomeAddress1"];
                model.HomeAddress2 = Request.Form["HomeAddress2"];
                int homecityId, homeStateId, homeCountryId;
                var retRes = int.TryParse(Request.Form["HomeCityId"], out homecityId);
                model.HomeCityId = retRes == true ? homecityId : model.HomeCityId;
                retRes = int.TryParse(Request.Form["HomeStateId"], out homeStateId);
                model.HomeStateId = retRes == true ? homeStateId : model.HomeStateId;
                retRes = int.TryParse(Request.Form["HomeCountryId"], out homeCountryId);
                model.HomeCountryId = retRes == true ? homeCountryId : model.HomeCountryId;
                model.HomeZipCode = Request.Form["HomeZipCode"];

                model.MailingAddress1 = Request.Form["MailingAddress1"];
                model.MailingAddress2 = Request.Form["MailingAddress2"];
                int mailingcityId, mailingStateId, mailingCountryId;
                retRes = int.TryParse(Request.Form["MailingCityId"], out mailingcityId);
                model.MailingCityId = retRes == true ? mailingcityId : model.MailingCityId;
                retRes = int.TryParse(Request.Form["MailingStateId"], out mailingStateId);
                model.MailingStateId = retRes == true ? mailingStateId : model.MailingStateId;
                retRes = int.TryParse(Request.Form["MailingCountryId"], out mailingCountryId);
                model.MailingCountryId = retRes == true ? homeCountryId : model.MailingCountryId;
                model.MailingZipCode = Request.Form["MailingZipCode"];
                //Interview question list
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var records = ser.Deserialize<List<QAViewModel>>(Request.Form["QADataList"]);
                var applicantShiftInfo = ser.Deserialize<ApplicantApplication>(Request.Form["ApplicantShiftData"]);
                var applicantEmploymentList = ser.Deserialize<List<ApplicantEmployment>>(Request.Form["ApplicantEmploymentData"]);
                var applicantEducationList = ser.Deserialize<List<ApplicantEducation>>(Request.Form["ApplicantEducationData"]);
                var applicantLocationList = model.JobLocationIds.Split(',');
                
                model.QAList = records;
                //throw new Exception("Testing phase");
                using (var applyJobDBTrans = db.Database.BeginTransaction())
                {
                    try
                    {
                        ApplicantInformation appInfoEntity = new ApplicantInformation();

                        appInfoEntity.FirstName = model.FirstName;
                        appInfoEntity.MiddleInitial = model.MiddleInitial;
                        appInfoEntity.FirstLastName = model.FirstLastName;
                        appInfoEntity.SecondLastName = model.SecondLastName;
                        appInfoEntity.ShortFullName = appInfoEntity.FullName;
                        appInfoEntity.GenderId = model.GenderId;
                        appInfoEntity.DisabilityId = model.DisabilityId;
                        var appStatusId = db.GetAll<ApplicantStatus>().Where(w => w.UseAsApply == true).Select(s=>s.Id).FirstOrDefault();
                        appInfoEntity.ApplicantStatusId = appStatusId;
                        appInfoEntity.JobPostingDetailId = model.JobPostingDetailId;
                        appInfoEntity.ClientId = JobPortalSession.JobPortalClientId;
                        appInfoEntity.CompanyId = JobPortalSession.JobPortalCompanyId;
                        appInfoEntity.CreatedBy = 0;
                        db.ApplicantInformation.Add(appInfoEntity);
                        db.SaveChanges();
                        if (appInfoEntity.Id > 0)
                        {  // applicant contact info
                            ApplicantContactInformation appContactInfo = new ApplicantContactInformation();
                            appContactInfo.ApplicantInformationId = appInfoEntity.Id;
                            appContactInfo.CelNumber = model.CellNumber;
                            appContactInfo.PersonalEmail = model.PersonalEmail;

                            appContactInfo.HomeAddress1 = model.HomeAddress1;
                            appContactInfo.HomeAddress2 = model.HomeAddress2;
                            appContactInfo.HomeCityId = model.HomeCityId;
                            appContactInfo.HomeStateId = model.HomeStateId;
                            appContactInfo.HomeCountryId = model.HomeCountryId;
                            appContactInfo.HomeZipCode = model.HomeZipCode;

                            appContactInfo.MailingAddress1 = model.MailingAddress1;
                            appContactInfo.MailingAddress2 = model.MailingAddress2;
                            appContactInfo.MailingCityId = model.MailingCityId;
                            appContactInfo.MailingStateId = model.MailingStateId;
                            appContactInfo.MailingCountryId = model.MailingCountryId;
                            appContactInfo.MailingZipCode = model.MailingZipCode;
                            appContactInfo.ClientId = JobPortalSession.JobPortalClientId;
                            appContactInfo.CreatedBy = 0;
                            db.ApplicantContactInformation.Add(appContactInfo);
                            db.SaveChanges();
                            //applicant Application
                            ApplicantApplication appApplicationEntity = new ApplicantApplication();
                            appApplicationEntity.ApplicantInformationId = appInfoEntity.Id;
                            appApplicationEntity.PositionId = model.JobDetail.PositionId.Value;
                            appApplicationEntity.DateApplied = DateTime.Today;
                            appApplicationEntity.DateAvailable = model.DateAvailable;
                            appApplicationEntity.ApplicantReferenceTypeId = model.ApplicantReferenceTypeId;
                            appApplicationEntity.ApplicantReferenceSourceId = model.ApplicantReferenceSourceId;
                            //appApplicationEntity.JobLocationId = model.JobLocationId;
                            appApplicationEntity.IsWorkedBefore = model.IsWorkedBefore;
                            appApplicationEntity.IsOvertime = model.IsOvertime;
                            appApplicationEntity.IsRelativeInCompany = model.IsRelativeInCompany;
                            appApplicationEntity.RelativeName = model.RelativeName;
                            appApplicationEntity.ClientId = JobPortalSession.JobPortalClientId;
                            appApplicationEntity.CreatedBy = 0;
                            //Shift data
                            if (applicantShiftInfo != null)
                            {
                                appApplicationEntity.IsSundayShift = applicantShiftInfo.IsSundayShift;
                                appApplicationEntity.SundayStartShift = applicantShiftInfo.SundayStartShift;
                                appApplicationEntity.SundayEndShift = applicantShiftInfo.SundayEndShift;

                                appApplicationEntity.IsMondayShift = applicantShiftInfo.IsMondayShift;
                                appApplicationEntity.MondayStartShift = applicantShiftInfo.MondayStartShift;
                                appApplicationEntity.MondayEndShift = applicantShiftInfo.MondayEndShift;

                                appApplicationEntity.IsTuesdayShift = applicantShiftInfo.IsTuesdayShift;
                                appApplicationEntity.TuesdayStartShift = applicantShiftInfo.TuesdayStartShift;
                                appApplicationEntity.TuesdayEndShift = applicantShiftInfo.TuesdayEndShift;

                                appApplicationEntity.IsWednesdayShift = applicantShiftInfo.IsWednesdayShift;
                                appApplicationEntity.WednesdayStartShift = applicantShiftInfo.WednesdayStartShift;
                                appApplicationEntity.WednesdayEndShift = applicantShiftInfo.WednesdayEndShift;

                                appApplicationEntity.IsThursdayShift = applicantShiftInfo.IsThursdayShift;
                                appApplicationEntity.ThursdayStartShift = applicantShiftInfo.ThursdayStartShift;
                                appApplicationEntity.ThursdayEndShift = applicantShiftInfo.ThursdayEndShift;

                                appApplicationEntity.IsFridayShift = applicantShiftInfo.IsFridayShift;
                                appApplicationEntity.FridayStartShift = applicantShiftInfo.FridayStartShift;
                                appApplicationEntity.FridayEndShift = applicantShiftInfo.FridayEndShift;

                                appApplicationEntity.IsSaturdayShift = applicantShiftInfo.IsSaturdayShift;
                                appApplicationEntity.SaturdayStartShift = applicantShiftInfo.SaturdayStartShift;
                                appApplicationEntity.SaturdayEndShift = applicantShiftInfo.SaturdayEndShift;
                            }
                            db.ApplicantApplication.Add(appApplicationEntity);
                            db.SaveChanges();
                            //Applicant Application Location
                            foreach(var locId in applicantLocationList)
                            {
                                if (String.IsNullOrEmpty(locId)) continue;
                                var appApplicationLocEntity = new ApplicantApplicationLocation()
                                {
                                    ApplicantApplicationId = appApplicationEntity.Id,
                                    LocationId = int.Parse(locId)
                                };
                                db.ApplicantApplicationLocation.Add(appApplicationLocEntity);    
                            }
                            db.SaveChanges();
                            //applicant previous employment
                            foreach (var appEmploymentEntity in applicantEmploymentList)
                            {

                                if (!string.IsNullOrEmpty(appEmploymentEntity.CompanyName))
                                {
                                    ApplicantCompany appCmpEntity = null;
                                    ApplicantPosition appPositionEntity = null;
                                    appEmploymentEntity.ApplicantInformationId = appInfoEntity.Id;
                                    appEmploymentEntity.ClientId = JobPortalSession.JobPortalClientId;
                                    appEmploymentEntity.CreatedBy = 0;
                                    appCmpEntity = db.GetAll<ApplicantCompany>(JobPortalSession.JobPortalClientId)
                                                        .Where(w => w.CompanyName.ToLower() == appEmploymentEntity.CompanyName.Trim().ToLower())
                                                        .FirstOrDefault();
                                    if (appCmpEntity == null)
                                    {
                                        appCmpEntity = new ApplicantCompany()
                                        {
                                            CompanyName = appEmploymentEntity.CompanyName,
                                            ClientId = JobPortalSession.JobPortalClientId,
                                            CreatedBy = 0
                                        };
                                        db.ApplicantCompany.Add(appCmpEntity);
                                        db.SaveChanges();
                                    }
                                    appEmploymentEntity.ApplicantCompanyId = appCmpEntity.Id;
                                    if (!string.IsNullOrEmpty(appEmploymentEntity.PositionName))
                                    {
                                        appPositionEntity = db.GetAll<ApplicantPosition>(JobPortalSession.JobPortalClientId)
                                                        .Where(w => w.PositionName.ToLower() == appEmploymentEntity.PositionName.Trim().ToLower())
                                                        .FirstOrDefault();
                                        if (appPositionEntity == null)
                                        {
                                            appPositionEntity = new ApplicantPosition()
                                            {
                                                PositionName = appEmploymentEntity.PositionName,
                                                ClientId = JobPortalSession.JobPortalClientId,
                                                CreatedBy= 0
                                            };
                                            db.ApplicantPosition.Add(appPositionEntity);
                                            db.SaveChanges();
                                        }
                                        appEmploymentEntity.ApplicantPositionId = appPositionEntity.Id;
                                    }
                                    db.ApplicantEmployment.Add(appEmploymentEntity);
                                    db.SaveChanges();
                                }
                            }
                            //Applicant Education data
                            foreach (var appEducationEntity in applicantEducationList)
                            {
                                if (appEducationEntity.DegreeId!=null)
                                {                                   
                                    appEducationEntity.ApplicantInformationId = appInfoEntity.Id;
                                    appEducationEntity.ClientId = JobPortalSession.JobPortalClientId;
                                    appEducationEntity.CreatedBy = 0;
                                    db.ApplicantEducation.Add(appEducationEntity);
                                    db.SaveChanges();
                                }
                            }
                            //Q & A data
                            foreach (var qaRecord in model.QAList)
                            {
                                ApplicantInterview appInterviewEntity = new ApplicantInterview();
                                appInterviewEntity.ApplicantInformationId = appInfoEntity.Id;
                                appInterviewEntity.ApplicantInterviewQuestionId = qaRecord.QId;
                                appInterviewEntity.ClientId = JobPortalSession.JobPortalClientId;
                                appInterviewEntity.CreatedBy = 0;
                                var ansOptValList = qaRecord.AnswerOptions.Select(s => s.AOptionValue).ToArray();
                                var ansTxt = string.Join("|", ansOptValList);
                                 appInterviewEntity.ApplicantAnswer = ansTxt;
                                db.ApplicantInterview.Add(appInterviewEntity);

                                db.SaveChanges();

                                if (appInterviewEntity.Id > 0)
                                {                                    
                                    foreach(var qAnswerOption in qaRecord.AnswerOptions)
                                    {
                                        ApplicantInterviewQAnswer appInterviewAnsEntity = new ApplicantInterviewQAnswer();
                                        appInterviewAnsEntity.ApplicantInterviewId = appInterviewEntity.Id;
                                        appInterviewAnsEntity.ApplicantInterviewQuestionId = qAnswerOption.QId;
                                        appInterviewAnsEntity.ApplicantQAnswerOptionId = qAnswerOption.AOptionId;
                                        appInterviewAnsEntity.AnswerValue = qAnswerOption.AOptionValue;
                                        appInterviewAnsEntity.ClientId = JobPortalSession.JobPortalClientId;
                                        appInterviewAnsEntity.CreatedBy = 0;
                                        db.ApplicantInterviewQAnswer.Add(appInterviewAnsEntity);
                                    }

                                }
                            }
                            db.SaveChanges();

                            //Cv Upload
                            if (Request.Files.Count > 0)
                            {
                                HttpPostedFileBase cvFile = Request.Files[0];
                                var fileExt = Path.GetExtension(cvFile.FileName);
                                var cvName = "Applicant-" + appInfoEntity.Id + "-CV" + fileExt;
                                var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                                FilePathHelper filePathHelper = new FilePathHelper(JobPortalSession.JobPortalClientId,JobPortalSession.JobPortalCompanyId, applicationPath);

                                var  serverFilePath = filePathHelper.GetPath("Applicant\\resumes", cvName);
                                cvFile.SaveAs(serverFilePath);
                                var relativeFilePath = filePathHelper.RelativePath;
                                appInfoEntity.ResumeFilePath = relativeFilePath;
                                db.SaveChanges();
                            }
                        }

                        applyJobDBTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        applyJobDBTrans.Rollback();
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }

                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

           
            return Json(new {status= status, message=message });
        }
        public JsonResult AjaxGetCountryState(int? id)
        {
            var stateList = db.State
                                .Where(w => w.CountryId == id && w.DataEntryStatus == 1).OrderBy(o => o.StateName)
                                .Select(s => new { id = s.Id, name = s.StateName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = stateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public ActionResult AjaxGetInterviewQAPopup(int id)
        {
            var appQuestion = db.ApplicantInterviewQuestion.Find(id);
            appQuestion.ApplicantQAnswerOptions = db.ApplicantQAnswerOption.Where(w => w.ApplicantInterviewQuestionId == id && w.DataEntryStatus == 1);


            return PartialView("ApplicantQAnswerPopup", appQuestion);
            //return Json(cityList);
        }
        public ActionResult GetApplicantQAOption(int id)
        {
            var appQuestion = db.ApplicantInterviewQuestion.Find(id);
            appQuestion.ApplicantQAnswerOptions = db.ApplicantQAnswerOption.Where(w => w.ApplicantInterviewQuestionId == id && w.DataEntryStatus == 1);


            return PartialView("_ApplicantQAOption", appQuestion);
            //return Json(cityList);
        }
        public ActionResult ApplicantEmployment(int id)
        {

            ViewBag.ApplicantExitTypeId = new SelectList(db.GetAll<ApplicantExitType>(JobPortalSession.JobPortalClientId), "Id", "ExitTypeName");
            ViewBag.RateFrequencyId = new SelectList(db.GetAll<RateFrequency>(JobPortalSession.JobPortalClientId), "Id", "RateFrequencyName");

            return PartialView("_ApplicantEmployment");
            //return Json(cityList);
        }
        public ActionResult ApplicantEducation()
        {
            ViewBag.DegreeId = new SelectList(db.GetAll<Degree>(JobPortalSession.JobPortalClientId), "Id", "DegreeName");
            return PartialView("_ApplicantEducation");
        }
        public JsonResult AjaxGetStateCity(int? id)
        {
            var cityList = db.City
                                .Where(w => w.StateId == id && w.DataEntryStatus == 1)
                                .Select(s => new { id = s.Id, name = s.CityName })
                                .OrderBy(e => e.name).ToList();

            JsonResult jsonResult = new JsonResult()
            {
                Data = cityList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
            //return Json(cityList);
        }
        public JsonResult AjaxGetAutoCompleteData(string term, string fieldName)
        {
            IList<string> autoCompleteDataList = null;
            switch (fieldName)
            {
                case "CompanyName":
                    autoCompleteDataList = db.ApplicantCompany
                                           .Where(w => w.CompanyName.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.CompanyName).Distinct()
                                           .ToList();
                    break;
                case "PositionName":
                    autoCompleteDataList = db.ApplicantPosition
                                           .Where(w => w.PositionName.ToLower().Contains(term.ToLower()))
                                           .Select(s => s.PositionName).Distinct()
                                           .ToList();
                    break;
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