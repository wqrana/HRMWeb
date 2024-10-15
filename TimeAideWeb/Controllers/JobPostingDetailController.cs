using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class JobPostingDetailController : TimeAideWebControllers<JobPostingDetail>
    {
        public override ActionResult Index()
        {
            try
            {
                AllowView();
                ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName");
                ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName");
                ViewBag.EmploymentTypeId = new SelectList(db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmploymentTypeName");
                ViewBag.JobPostingStatusId = new SelectList(db.GetAll<JobPostingStatus>(SessionHelper.SelectedClientId), "Id", "Name");

                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(JobPostingDetail).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public ActionResult IndexByFilter(UserFilterViewModel recFilter)
        {
            List<JobPostingDetail> jobPostingList = GetFilteredJobPosting(recFilter);          

            return PartialView(jobPostingList);
        }

        private List<JobPostingDetail> GetFilteredJobPosting(UserFilterViewModel recFilter)
        {
            var filteredList = db.GetAllByCompany<JobPostingDetail>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                    .Where(w => (recFilter.PositionId == null ? true : w.PositionId == recFilter.PositionId))
                                    .Where(w => (recFilter.DepartmentId == null ? true : w.DepartmentId == recFilter.DepartmentId))
                                    .Where(w => (recFilter.ViewTypeId == 0 ? true : w.EmploymentTypeId == recFilter.ViewTypeId))
                                    .Where(w => (recFilter.EmployeeStatusId == null ? true : w.JobPostingStatusId == recFilter.EmployeeStatusId)).ToList();
            return filteredList;
        }
        public override ActionResult Details(int? id)
        {
            try
            {
                JobPostingDetail model = null;
                AllowView();
                model = db.JobPostingDetail.Find(id);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "JobPosting", "Details");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult EditJobDescription(int? id)
        {
            try
            {
                JobPostingDetail model = null;
                AllowEdit();
                model = db.JobPostingDetail.Find(id);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "JobPosting", "EditJobDesc");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult EditJobDescription(JobPostingDetail model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            JobPostingDetail jobPostingEntity = null;
            try
            {
               
                jobPostingEntity = db.JobPostingDetail.Find(model.Id);
                jobPostingEntity.ModifiedBy = SessionHelper.LoginId;
                jobPostingEntity.ModifiedDate = DateTime.Now;
                jobPostingEntity.JobDescription = model.JobDescription;
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
        public override ActionResult Create()
        {
            AllowAdd();
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName");
            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName");
            ViewBag.EmploymentTypeId = new SelectList(db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmploymentTypeName");
            ViewBag.JobPostingStatusId = new SelectList(db.GetAll<JobPostingStatus>(SessionHelper.SelectedClientId), "Id", "Name");
            ViewBag.LocationId = new SelectList(db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderBy(o=>o.LocationName), "Id", "LocationName");
           
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {

            var model = db.JobPostingDetail.Where(w => w.Id == id).FirstOrDefault();
            AllowEdit();
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName",model.PositionId);
            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName",model.DepartmentId);
            ViewBag.EmploymentTypeId = new SelectList(db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmploymentTypeName",model.EmploymentTypeId);
            ViewBag.JobPostingStatusId = new SelectList(db.GetAll<JobPostingStatus>(SessionHelper.SelectedClientId), "Id", "Name",model.JobPostingStatusId);
            //ViewBag.LocationId = new SelectList(db.GetAll<Location>(SessionHelper.SelectedClientId), "Id", "LocationName",model.LocationId);
            ViewBag.LocationId = new SelectList(db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderBy(o => o.LocationName), "Id", "LocationName");
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateEdit(JobPostingDetail model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            JobPostingDetail jobPostingEntity = null;
            var id = 0;
            try
            {
                //Comment the existing job checking
                //var isAlreadyExist = db.GetAllByCompany<JobPostingDetail>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                //                        .Where(w => (w.Id != model.Id) && (w.PositionId == model.PositionId)&& w.JobPostingStatusId==1)
                //                        .Count();
                var isAlreadyExist = 0;
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Similar type of job is already posted.";
                }
                else
                {
                    if (model.Id == 0)
                    {
                       jobPostingEntity = new JobPostingDetail();
                        db.JobPostingDetail.Add(jobPostingEntity);
                    }
                    else
                    {
                        jobPostingEntity = db.JobPostingDetail.Find(model.Id);
                        jobPostingEntity.ModifiedBy = SessionHelper.LoginId;
                        jobPostingEntity.ModifiedDate = DateTime.Now;
                    }
                    jobPostingEntity.PositionId = model.PositionId;
                    jobPostingEntity.DepartmentId = model.DepartmentId;
                    jobPostingEntity.EmploymentTypeId = model.EmploymentTypeId;
                    jobPostingEntity.LocationId = model.LocationId;
                    jobPostingEntity.NoOfVacancies = model.NoOfVacancies;
                    jobPostingEntity.Experience = model.Experience;
                    jobPostingEntity.SalaryFrom = model.SalaryFrom;
                    jobPostingEntity.SalaryTo = model.SalaryTo;
                    jobPostingEntity.JobPostingStartDate = model.JobPostingStartDate;
                    jobPostingEntity.JobPostingExpiringDate = model.JobPostingExpiringDate;
                    jobPostingEntity.JobPostingStatusId = model.JobPostingStatusId;
                    jobPostingEntity.IsHomeAddress = model.IsHomeAddress;
                    jobPostingEntity.IsMailingAddress = model.IsMailingAddress;
                    jobPostingEntity.IsAvailablity = model.IsAvailablity;
                    jobPostingEntity.IsPreviousEmployment = model.IsPreviousEmployment;
                    jobPostingEntity.IsEducation = model.IsEducation;
                    using (var jobPostingDBTrans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.SaveChanges();
                            id = jobPostingEntity.Id;
                            if(id>0)
                            {
                                IList<JobPostingLocation> emptyList = new List<JobPostingLocation>();
                                //remove existing loc
                                IList<JobPostingLocation>  removeRecordList = jobPostingEntity.JobPostingLocations!=null? jobPostingEntity.JobPostingLocations.ToList(): emptyList;
                                IList<JobPostingLocation>  addRecordList = emptyList;
                                if (removeRecordList.Count > 0)
                                {
                                    db.JobPostingLocation.RemoveRange(removeRecordList);
                                    db.SaveChanges();
                                }                               
                                //add new fresh list
                                var locationIds = model.LocationIds==null? "" : model.LocationIds;
                                foreach (var locationId in locationIds.Split(','))
                                {
                                    addRecordList.Add( new JobPostingLocation() { LocationId = int.Parse(locationId), JobPostingDetailId = id });
                                    
                                }
                                if (addRecordList.Count > 0)
                                {
                                    db.JobPostingLocation.AddRange(addRecordList);
                                    db.SaveChanges();
                                }
                                
                            }
                            jobPostingDBTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            jobPostingDBTrans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new {id =id, status = status, message = message });
        }
        
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var jpEntity = db.JobPostingDetail.Find(id);
            try
            {
                jpEntity.ModifiedBy = SessionHelper.LoginId;
                jpEntity.ModifiedDate = DateTime.Now;
                jpEntity.DataEntryStatus = 0;
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