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
    public class ApplicantApplicationController : BaseApplicantRoleRightsController<ApplicantApplication>
    {
        public ActionResult Details(int id)
        {
            try
            {
                AllowView();
                var model = db.ApplicantApplication.Where(w => w.ApplicantInformationId == id).FirstOrDefault();
                if (model == null)
                {
                    model = new ApplicantApplication();
                }
                else
                {
                    model.ApplicantEmploymentTypes = db.ApplicantEmploymentType.Where(w => w.ApplicantInformationId == model.ApplicantInformationId).ToList();
                    model.SelectedEmploymentTypeId = String.Join(",", model.ApplicantEmploymentTypes.Select(s => s.EmployeeType.EmployeeTypeName));
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
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantApplication", "Details");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EditApplication(int id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantApplication.Where(w => w.ApplicantInformationId == id).FirstOrDefault();
                if (model == null)
                {
                    model = new ApplicantApplication() { DateApplied = DateTime.Today };
                }
                var selectedEmploymentTypeIds = db.ApplicantEmploymentType.Where(w=>w.ApplicantInformationId==id).Select(s =>  s.EmployeeTypeId ).ToList();
                var selectedApplicantLocIds = db.ApplicantApplicationLocation.Where(w => w.ApplicantApplicationId == model.Id).Select(s => s.LocationId).ToList();

                ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName", model.PositionId);
                ViewBag.ApplicantReferenceTypeId = new SelectList(db.GetAll<ApplicantReferenceType>(SessionHelper.SelectedClientId), "Id", "ReferenceTypeName", model.ApplicantReferenceTypeId);
                ViewBag.ApplicantReferenceSourceId = new SelectList(db.GetAll<ApplicantReferenceSource>(SessionHelper.SelectedClientId), "Id", "ReferenceSourceName", model.ApplicantReferenceSourceId);
                ViewBag.RateFrequencyId = new SelectList(db.GetAll<RateFrequency>(SessionHelper.SelectedClientId), "Id", "RateFrequencyName", model.RateFrequencyId);
                ViewBag.EmployeeTypeId = new MultiSelectList(db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "EmployeeTypeName", selectedEmploymentTypeIds);
                ViewBag.JobLocationId = new MultiSelectList(db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "LocationName", selectedApplicantLocIds);

                //ViewBag.JobLocationId = new SelectList(db.GetAll<ApplicantReferenceSource>(SessionHelper.SelectedClientId), "Id", "ReferenceSourceName", model.ApplicantReferenceSourceId);
                //ViewBag.JobLocationId = new SelectList(db.GetAll<Location>(SessionHelper.SelectedClientId).OrderBy(o=>o.LocationName), "Id", "LocationName", model.JobLocationId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantApplication", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EditApplicantShift(int id)
        {
            try
            {
                AllowEdit();
                var model = db.ApplicantApplication.Where(w => w.ApplicantInformationId == id).FirstOrDefault();
                if (model == null)
                {
                    model = new ApplicantApplication() { DateApplied = DateTime.Today };
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
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantApplication", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw  ex;
            }
        }
        [HttpPost]
        public JsonResult EditApplicantShift(ApplicantApplication model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantApplication applicantApplication = null;
            try
            {
                 applicantApplication = db.ApplicantApplication.Find(model.Id);

                if (applicantApplication != null)
                {
                    applicantApplication.ModifiedBy = SessionHelper.LoginId;
                    applicantApplication.ModifiedDate = DateTime.Now;

                    applicantApplication.IsMondayShift = model.IsMondayShift;
                    applicantApplication.MondayStartShift = model.MondayStartShift;
                    applicantApplication.MondayEndShift = model.MondayEndShift;

                    applicantApplication.IsTuesdayShift = model.IsTuesdayShift;
                    applicantApplication.TuesdayStartShift = model.TuesdayStartShift;
                    applicantApplication.TuesdayEndShift = model.TuesdayEndShift;

                    applicantApplication.IsWednesdayShift = model.IsWednesdayShift;
                    applicantApplication.WednesdayStartShift = model.WednesdayStartShift;
                    applicantApplication.WednesdayEndShift = model.WednesdayEndShift;

                    applicantApplication.IsThursdayShift = model.IsThursdayShift;
                    applicantApplication.ThursdayStartShift = model.ThursdayStartShift;
                    applicantApplication.ThursdayEndShift = model.ThursdayEndShift;

                    applicantApplication.IsFridayShift = model.IsFridayShift;
                    applicantApplication.FridayStartShift = model.FridayStartShift;
                    applicantApplication.FridayEndShift = model.FridayEndShift;

                    applicantApplication.IsSaturdayShift = model.IsSaturdayShift;
                    applicantApplication.SaturdayStartShift = model.SaturdayStartShift;
                    applicantApplication.SaturdayEndShift = model.SaturdayEndShift;

                    applicantApplication.IsSundayShift = model.IsSundayShift;
                    applicantApplication.SundayStartShift = model.SundayStartShift;
                    applicantApplication.SundayEndShift = model.SundayEndShift;

                    db.SaveChanges();
                }
                else
                {
                    status = "Error";
                    message = "Invalid Application record";
                }
            }
             
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message  });
        }
        [HttpPost]
        public JsonResult EditApplication(ApplicantApplication model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            ApplicantApplication applicantApplication = null;
            using (var appDBTrans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id == 0)
                    {
                        applicantApplication = new ApplicantApplication();
                        applicantApplication.ApplicantInformationId = model.ApplicantInformationId;
                        db.ApplicantApplication.Add(applicantApplication);
                    }
                    else
                    {
                        applicantApplication = db.ApplicantApplication.Find(model.Id);
                        applicantApplication.ModifiedBy = SessionHelper.LoginId;
                        applicantApplication.ModifiedDate = DateTime.Now;
                    }
                    applicantApplication.DateApplied = model.DateApplied;
                    applicantApplication.PositionId = model.PositionId;
                    applicantApplication.ApplicantReferenceTypeId = model.ApplicantReferenceTypeId;
                    applicantApplication.ApplicantReferenceSourceId = model.ApplicantReferenceSourceId;
                    // applicantApplication.JobLocationId = model.JobLocationId;
                    applicantApplication.Rate = model.Rate;
                    applicantApplication.RateFrequencyId = model.RateFrequencyId;
                    applicantApplication.DateAvailable = model.DateAvailable;
                    applicantApplication.IsOvertime = model.IsOvertime;
                    applicantApplication.IsWorkedBefore = model.IsWorkedBefore;
                    applicantApplication.WorkedBeforeDate = model.WorkedBeforeDate;
                    applicantApplication.IsRelativeInCompany = model.IsRelativeInCompany;
                    applicantApplication.RelativeName = model.RelativeName;
                    db.SaveChanges();
                    //-- Save Employment type in seperate table                
                    List<string> selectedEmploymentTypeList = (model.SelectedEmploymentTypeId ?? "").Split(',').ToList();
                    List<ApplicantEmploymentType> appEmploymentTypeAddList = new List<ApplicantEmploymentType>();
                    List<ApplicantEmploymentType> appEmploymentTypeRemoveList = new List<ApplicantEmploymentType>();
                    applicantApplication.ApplicantEmploymentTypes = db.ApplicantEmploymentType.Where(w => w.ApplicantInformationId == model.ApplicantInformationId).ToList();

                    foreach (var appEmploymentItem in applicantApplication.ApplicantEmploymentTypes)
                    {
                        // var RecCnt = selectedVeteranList.Where(w => w == veteranItem.Id.ToString()).Count();
                        var RecCnt = selectedEmploymentTypeList.Where(w => w == appEmploymentItem.EmployeeTypeId.ToString()).Count();
                        if (RecCnt == 0)
                        {
                            appEmploymentTypeRemoveList.Add(appEmploymentItem);
                        }

                    }
                    foreach (var selectedEmploymentTypeId in selectedEmploymentTypeList)
                    {
                        if (selectedEmploymentTypeId == "") continue;
                        int employmentTypeId = int.Parse(selectedEmploymentTypeId);
                        var recExists = applicantApplication.ApplicantEmploymentTypes.Where(w => w.EmployeeTypeId == employmentTypeId).Count();
                        if (recExists == 0)
                        {
                            appEmploymentTypeAddList.Add(new ApplicantEmploymentType() { ApplicantInformationId = applicantApplication.ApplicantInformationId, EmployeeTypeId = employmentTypeId, CreatedBy = 1, DataEntryStatus = 1, CreatedDate = DateTime.Now });

                        }
                    }

                    db.ApplicantEmploymentType.RemoveRange(appEmploymentTypeRemoveList);
                    db.ApplicantEmploymentType.AddRange(appEmploymentTypeAddList);
                    db.SaveChanges();
                    //End Adding Employment types
                    //-- Save Job Location in seperate table                
                    List<string> selectedJobLocationList = (model.SelectedJobLocationId ?? "").Split(',').ToList();
                    List<ApplicantApplicationLocation> appJobLocationAddList = new List<ApplicantApplicationLocation>();
                    List<ApplicantApplicationLocation> appJobLocationRemoveList = new List<ApplicantApplicationLocation>();
                    applicantApplication.ApplicantApplicationLocations = db.ApplicantApplicationLocation.Where(w => w.ApplicantApplicationId == applicantApplication.Id).ToList();

                    foreach (var appJobLocItem in applicantApplication.ApplicantApplicationLocations)
                    {
                        // var RecCnt = selectedVeteranList.Where(w => w == veteranItem.Id.ToString()).Count();
                        var RecCnt = selectedJobLocationList.Where(w => w == appJobLocItem.LocationId.ToString()).Count();
                        if (RecCnt == 0)
                        {
                            appJobLocationRemoveList.Add(appJobLocItem);
                        }

                    }
                    foreach (var selectedJobLocationId in selectedJobLocationList)
                    {
                        if (selectedJobLocationId == "") continue;
                        int locationId = int.Parse(selectedJobLocationId);
                        var recExists = applicantApplication.ApplicantApplicationLocations.Where(w => w.LocationId == locationId).Count();
                        if (recExists == 0)
                        {
                            appJobLocationAddList.Add(new ApplicantApplicationLocation() { ApplicantApplicationId = applicantApplication.Id, LocationId = locationId });
                        }
                    }

                    db.ApplicantApplicationLocation.RemoveRange(appJobLocationRemoveList);
                    db.ApplicantApplicationLocation.AddRange(appJobLocationAddList);
                    db.SaveChanges();
                    //End Adding Applicant Location
                    appDBTrans.Commit();

                }
                catch (Exception ex)
                {
                    appDBTrans.Rollback();
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        

    }

}