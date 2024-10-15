using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeIncidentController : TimeAideWebControllers<EmployeeIncident>
    {
               
        public override ActionResult MostRecentRecord(int? id)
        {
            try
            {
                EmployeeIncident model = null;
                AllowView();
                model = db.EmployeeIncident.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id).FirstOrDefault();
                if (model == null) model = new EmployeeIncident();

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "UserInformation", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public ActionResult CreateByUser(int id)
        {
            try
            {
                
                AllowAdd();
                ViewBag.IncidentTypeId = new SelectList(db.GetAll<IncidentType>(), "Id", "IncidentTypeName");
                ViewBag.OSHACaseClassificationId = new SelectList(db.GetAll<OSHACaseClassification>(), "Id", "OSHACaseClassificationName");
                ViewBag.CompletedById = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "ShortFullName");
                ViewBag.LocationId = new SelectList(db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "LocationName");
                ViewBag.IncidentAreaId = new SelectList(db.GetAllByCompany<IncidentArea>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "IncidentAreaName");
                var model = new EmployeeIncident();
                return PartialView("Create", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.EmployeeIncident.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.IncidentTypeId = new SelectList(db.GetAll<IncidentType>(), "Id", "IncidentTypeName",model.IncidentTypeId);
                ViewBag.OSHACaseClassificationId = new SelectList(db.GetAll<OSHACaseClassification>(), "Id", "OSHACaseClassificationName",model.OSHACaseClassificationId);
                ViewBag.CompletedById = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "ShortFullName",model.CompletedById);
                ViewBag.LocationId = new SelectList(db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "LocationName", model.LocationId);
                ViewBag.IncidentAreaId = new SelectList(db.GetAllByCompany<IncidentArea>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "IncidentAreaName", model.IncidentAreaId);
                
                return PartialView( model);
                
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult CreateEdit(EmployeeIncident model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeIncident employeeIncidentEntity = null;
            try
            {

                if (model.Id == 0)
                {

                    employeeIncidentEntity = new EmployeeIncident();
                    employeeIncidentEntity.UserInformationId = model.UserInformationId;
                    db.EmployeeIncident.Add(employeeIncidentEntity);
                }
                else
                {
                    employeeIncidentEntity = db.EmployeeIncident.Find(model.Id);
                    employeeIncidentEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeIncidentEntity.ModifiedDate = DateTime.Now;
                }
                employeeIncidentEntity.IncidentTypeId = model.IncidentTypeId;
                employeeIncidentEntity.IsOSHARecordable = model.IsOSHARecordable;
                employeeIncidentEntity.OSHACaseClassificationId = model.OSHACaseClassificationId;
                employeeIncidentEntity.CompletedDate = model.CompletedDate;
                employeeIncidentEntity.CompletedById = model.CompletedById;
                employeeIncidentEntity.LocationId = model.LocationId;
                employeeIncidentEntity.IncidentAreaId = model.IncidentAreaId;
                employeeIncidentEntity.IncidentDate = model.IncidentDate;
                employeeIncidentEntity.IncidentTime = model.IncidentTime;
                employeeIncidentEntity.EmployeeBeganWorkTime = model.EmployeeBeganWorkTime;
                employeeIncidentEntity.EmployeeDoingBeforeIncident = model.EmployeeDoingBeforeIncident;
                employeeIncidentEntity.HowIncidentOccured = model.HowIncidentOccured;

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
            var employeeIncidentEntity = db.EmployeeIncident.Find(id);
            try
            {
                employeeIncidentEntity.ModifiedBy = SessionHelper.LoginId;
                employeeIncidentEntity.ModifiedDate = DateTime.Now;
                employeeIncidentEntity.DataEntryStatus = 0;
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
        
        
        public ActionResult EditInjury(int? id)
        {
            try
            {
                AllowEdit();

                var model = db.EmployeeIncident.Where(w => w.Id == id).FirstOrDefault();             
                ViewBag.OSHAInjuryClassificationId = new SelectList(db.GetAll<OSHAInjuryClassification>(), "Id", "OSHAInjuryClassificationName", model.OSHAInjuryClassificationId);
                ViewBag.IncidentBodyPartId = new SelectList(db.GetAll<IncidentBodyPart>(SessionHelper.SelectedClientId), "Id", "IncidentBodyPartName", model.IncidentBodyPartId);
                ViewBag.IncidentInjuryDescriptionId = new SelectList(db.GetAll<IncidentInjuryDescription>(SessionHelper.SelectedClientId), "Id", "IncidentInjuryDescriptionName", model.IncidentInjuryDescriptionId);
                ViewBag.IncidentInjurySourceId = new SelectList(db.GetAll<IncidentInjurySource>(SessionHelper.SelectedClientId), "Id", "IncidentInjurySourceName", model.IncidentInjurySourceId);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
      
        [HttpPost]
        public JsonResult EditInjury(EmployeeIncident model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeIncident employeeIncidentEntity = null;
            try
            {

                if (model.Id != 0)
                {
                    employeeIncidentEntity = db.EmployeeIncident.Find(model.Id);
                    employeeIncidentEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeIncidentEntity.ModifiedDate = DateTime.Now;
                    employeeIncidentEntity.OSHAInjuryClassificationId = model.OSHAInjuryClassificationId;
                    employeeIncidentEntity.IncidentBodyPartId = model.IncidentBodyPartId;
                    employeeIncidentEntity.IncidentInjuryDescriptionId = model.IncidentInjuryDescriptionId;
                    employeeIncidentEntity.IncidentInjurySourceId = model.IncidentInjurySourceId;
                    employeeIncidentEntity.DateOfDeath = model.DateOfDeath;
                    employeeIncidentEntity.AwayFromWorkDays = model.AwayFromWorkDays;
                    employeeIncidentEntity.RestrictedFromWorkDays = model.RestrictedFromWorkDays;
                    db.SaveChanges();
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

        public ActionResult EditTreatment(int? id)
        {
            try
            {
                AllowEdit();
                var model = db.EmployeeIncident.Where(w => w.Id == id).FirstOrDefault();
                ViewBag.IncidentTreatmentFacilityId = new SelectList(db.GetAll<IncidentTreatmentFacility>(SessionHelper.SelectedClientId), "Id", "TreatmentFacilityName", model.IncidentTreatmentFacilityId);                
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public JsonResult EditTreatment(EmployeeIncident model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeIncident employeeIncidentEntity = null;
            try
            {

                if (model.Id != 0)
                {
                    employeeIncidentEntity = db.EmployeeIncident.Find(model.Id);
                    employeeIncidentEntity.ModifiedBy = SessionHelper.LoginId;
                    employeeIncidentEntity.ModifiedDate = DateTime.Now;
                    employeeIncidentEntity.PhysicianName = model.PhysicianName;
                    employeeIncidentEntity.IncidentTreatmentFacilityId = model.IncidentTreatmentFacilityId;
                    employeeIncidentEntity.IsTreatedInEmergencyRoom = model.IsTreatedInEmergencyRoom;
                    employeeIncidentEntity.IsHospitalizedOvernight = model.IsHospitalizedOvernight;
                    employeeIncidentEntity.HospitalizedDays = model.HospitalizedDays;

                    db.SaveChanges();
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
    }
}


