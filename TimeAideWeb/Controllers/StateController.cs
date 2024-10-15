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
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class StateController : TimeAideWebControllers<State>
    {
        public virtual ActionResult IndexByCountry(int? countryId)
        {
            try
            {
                AllowView();
                StateListView stateListView = new StateListView();
                List<int> ids = new List<int>();
                stateListView.IndexCountryId = 1;
                var states = db.State.Where(c => c.CountryId == stateListView.IndexCountryId && c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId);
                stateListView.States = states.OrderBy(e => e.StateName).ToList();
                return PartialView("Index", stateListView);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(City).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public virtual ActionResult IndexByCountry([Bind(Include = "IndexCountryId")] StateListView cityListView)
        {
            try
            {
                AllowView();
                List<int> ids = new List<int>();
                if ((!cityListView.IndexCountryId.HasValue || cityListView.IndexCountryId == 0))
                {
                    cityListView.IndexCountryId = 1;
                }
                var states = db.State.Where(c => c.CountryId == cityListView.IndexCountryId && c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId);
                cityListView.States = states.OrderBy(e => e.StateName).ToList();
                return PartialView("Index", cityListView);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(City).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        //private TimeAideContext db = new TimeAideContext();
        // POST: State/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StateDescription,StateCode,StateName,CountryId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] State state)
        {
            if (ModelState.IsValid)
            {
                db.State.Add(state);
                db.SaveChanges();
                return Json(state);
            }
            return GetErrors();
        }



        // POST: State/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StateDescription,StateCode,StateName,CountryId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] State state)
        {
            if (ModelState.IsValid)
            {
                state.SetUpdated<State>();
                db.Entry(state).State = EntityState.Modified;
                db.SaveChanges();
                return Json(state);
            }
            return GetErrors();
        }

        public JsonResult AjaxGetCountryState(int? countryId)
        {
            var stateList = db.State
                                .Where(w => w.CountryId == countryId && w.DataEntryStatus == 1).OrderBy(o=>o.StateName)
                                .Select(s => new { id = s.Id, name = s.StateName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = stateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public override bool CheckBeforeDelete(int id)
        {
            var state = db.State.Include(u => u.Cities)
                                                .FirstOrDefault(c => c.Id == id);
            if (state.Cities.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
