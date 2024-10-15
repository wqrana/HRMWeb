using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class CityController : TimeAideWebControllers<City>
    {
        public virtual ActionResult IndexByState()
        {
            try
            {
                AllowView();
                CityListView cityListView = new CityListView();
                List<int> ids = new List<int>();
                cityListView.IndexCountryId = 1;

                ids = (from state in db.State where state.CountryId == 1 select state.Id).ToList();
                var entitySet = db.City.Where(c => ids.Contains(c.StateId) && c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId);
                cityListView.Cities = entitySet.OrderBy(e => e.CityName).ToList();
                return PartialView("Index", cityListView);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(City).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public virtual ActionResult IndexByState([Bind(Include = "IndexCountryId,IndexStateId")] CityListView cityListView)
        {
            try
            {
                AllowView();
                List<int> ids = new List<int>();
                if ((!cityListView.IndexCountryId.HasValue || cityListView.IndexCountryId == 0) && (!cityListView.IndexStateId.HasValue || cityListView.IndexStateId == 0))
                {
                    cityListView.IndexCountryId = 1;
                }

                if ((!cityListView.IndexStateId.HasValue || cityListView.IndexStateId == 0))
                {
                    ids = (from state in db.State where state.CountryId == 1 select state.Id).ToList();
                }
                else
                {
                    ids.Add(cityListView.IndexStateId.Value);
                }

                var entitySet = db.City.Where(c => ids.Contains(c.StateId) && c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId);
                cityListView.Cities = entitySet.OrderBy(e => e.CityName).ToList();
                return PartialView("Index", cityListView);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(City).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CityName,CityCode,CityName,CountryId,StateId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] City city)
        {
            if (ModelState.IsValid)
            {
                db.City.Add(city);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                }
                return Json(city);
            }
            return GetErrors();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CityName,CityCode,CityName,CountryId,StateId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] City city)
        {
            if (ModelState.IsValid)
            {
                city.SetUpdated<City>();
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return Json(city);
            }
            return GetErrors();
        }
        public JsonResult AjaxGetStateCity(int? StateId)
        {
            var cityList = db.City
                                .Where(w => w.StateId == StateId && w.DataEntryStatus == 1)
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

        public override bool CheckBeforeDelete(int id)
        {
            var city = db.City.Include(u => u.MailingCityUserContactInformation)
                         .Include(u => u.HomeCityUserContactInformation)
                         .FirstOrDefault(c => c.Id == id);
            if (city.MailingCityUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (city.HomeCityUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
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
    }
}
