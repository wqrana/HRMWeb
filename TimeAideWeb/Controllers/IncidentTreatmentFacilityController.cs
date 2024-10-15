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
    public class IncidentTreatmentFacilityController : TimeAideWebControllers<IncidentTreatmentFacility>
    {
        [HttpGet]
        public override ActionResult Create()
        {
            try
            {
                int? defaultCountryId=0;
                int? defaultStateId=0;
                int? defaultCityId=0;
                AllowAdd();
                ViewBag.Label = ViewBag.Label + " - Add";
                defaultCountryId = db.GetAll<Country>(SessionHelper.SelectedClientId).Where(w => w.CountryCode == "PR").
                                    Select(s => s.Id).FirstOrDefault();
                var client = db.Client.Find(SessionHelper.SelectedClientId);
                if (client != null)
                {
                    defaultCountryId = client.CountryId == null ? defaultCountryId : client.CountryId;
                    defaultStateId = client.StateId == null ? 0 : client.StateId;
                    defaultCityId = client.CityId == null ? 0 : client.CityId;
                }
                ViewBag.StateId = new SelectList(db.GetAll<State>(SessionHelper.SelectedClientId).Where(w=>w.CountryId== defaultCountryId), "Id", "StateName", defaultStateId);
                ViewBag.CityId = new SelectList(db.GetAll<City>(SessionHelper.SelectedClientId).Where(w => w.StateId == defaultStateId), "Id", "CityName", defaultCityId);

                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpGet]
        public override ActionResult CreateAdhocMasterData()
        {
            try
            {
                AllowAdd();
                int? defaultCountryId = 0;
                int? defaultStateId = 0;
                int? defaultCityId = 0;
                AllowAdd();
                ViewBag.Label = ViewBag.Label + " - Add";
                defaultCountryId = db.GetAll<Country>(SessionHelper.SelectedClientId).Where(w => w.CountryCode == "PR").
                                    Select(s => s.Id).FirstOrDefault();
                var client = db.Client.Find(SessionHelper.SelectedClientId);
                if (client != null)
                {
                    defaultCountryId = client.CountryId == null ? defaultCountryId : client.CountryId;
                    defaultStateId = client.StateId == null ? 0 : client.StateId;
                    defaultCityId = client.CityId == null ? 0 : client.CityId;
                }
                ViewBag.StateId = new SelectList(db.GetAll<State>(SessionHelper.SelectedClientId).Where(w => w.CountryId == defaultCountryId), "Id", "StateName", defaultStateId);
                ViewBag.CityId = new SelectList(db.GetAll<City>(SessionHelper.SelectedClientId).Where(w => w.StateId == defaultStateId), "Id", "CityName", defaultCityId);

                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpGet]
        public override ActionResult Edit(int? id)
        {
            try
            {
                int? selectedCountryId = 0;
                int? selectedStateId = 0;
                int? selectedCityId = 0;
                AllowEdit();
                
                if (id == null)
                {
                    return PartialView();
                }
                IncidentTreatmentFacility entity = db.Find<IncidentTreatmentFacility>(id.Value, SessionHelper.SelectedClientId);
                if (entity == null)
                {
                    return PartialView();
                }                
                ViewBag.Label = ViewBag.Label + " - Edit";

                selectedCountryId = db.GetAll<Country>(SessionHelper.SelectedClientId).Where(w => w.CountryCode == "PR").
                                   Select(s => s.Id).FirstOrDefault();
                var client = db.Client.Find(SessionHelper.SelectedClientId);
                if (client != null)
                {
                    selectedCountryId = client.CountryId == null ? selectedCountryId : client.CountryId;
                    selectedStateId = entity.StateId == null ? client.StateId : entity.StateId;
                    selectedCityId = entity.CityId == null ? client.CityId : entity.CityId;
                }
                ViewBag.StateId = new SelectList(db.GetAll<State>(SessionHelper.SelectedClientId).Where(w => w.CountryId == selectedCountryId), "Id", "StateName", selectedStateId);
                ViewBag.CityId = new SelectList(db.GetAll<City>(SessionHelper.SelectedClientId).Where(w => w.StateId == selectedStateId), "Id", "CityName", selectedCityId);


                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
               
        // GET: 
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(IncidentTreatmentFacility model)
        {
            if (ModelState.IsValid)
            {
                db.IncidentTreatmentFacility.Add(model);
                db.SaveChanges();
                // return RedirectToAction("Index");
                return Json(model);
            }

            //return PartialView(model);
            return GetErrors();
        }
        //Use for Adhoc addition of Master data from application   
        [HttpPost]
        public ActionResult CreateAdhocMasterData(IncidentTreatmentFacility model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.IncidentTreatmentFacility.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required* field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.TreatmentFacilityName });
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(IncidentTreatmentFacility model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return PartialView(model);
            return GetErrors();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
