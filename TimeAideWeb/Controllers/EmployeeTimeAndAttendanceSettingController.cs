using Newtonsoft.Json;
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
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeTimeAndAttendanceSettingController : TimeAideWebControllers<EmployeeTimeAndAttendanceSetting>
    {
        [HttpGet]
        public ActionResult CreateEditTimeAndAttendanceSetting(int id)
        {
            var model = db.GetAllByUser<EmployeeTimeAndAttendanceSetting>(id, SessionHelper.SelectedClientId).FirstOrDefault();
            if(model==null)
            {
                model = new EmployeeTimeAndAttendanceSetting();
                model.UserInformationId = id;
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult CreateEditTimeAndAttendanceSetting(EmployeeTimeAndAttendanceSetting model)
        {
            var modelDb = db.GetAllByUser<EmployeeTimeAndAttendanceSetting>(model.UserInformationId, SessionHelper.SelectedClientId).FirstOrDefault();
            if (modelDb == null)
            {
                modelDb = new EmployeeTimeAndAttendanceSetting();
                modelDb.UserInformationId = model.UserInformationId;
                db.EmployeeTimeAndAttendanceSetting.Add(modelDb);
            }
            else {
                modelDb.SetUpdated<EmployeeTimeAndAttendanceSetting>();
            }
            modelDb.EnableWebPunch = model.EnableWebPunch;
           
            db.SaveChanges();
            return Json(new { status = "Success", message = "Success" });
        }

        [HttpGet]
        public ActionResult CreateEdit(int id)
        {
            var model = db.GetAllByUser<EmployeeTimeAndAttendanceSetting>(id, SessionHelper.SelectedClientId).FirstOrDefault();
            if (model == null)
            {
                model = new EmployeeTimeAndAttendanceSetting();
                model.UserInformationId = id;
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult CreateEdit(EmployeeTimeAndAttendanceSetting model)
        {
            var modelDb = db.GetAllByUser<EmployeeTimeAndAttendanceSetting>(model.UserInformationId, SessionHelper.SelectedClientId).FirstOrDefault();
            if (modelDb == null)
            {
                modelDb = new EmployeeTimeAndAttendanceSetting();
                modelDb.UserInformationId = model.UserInformationId;
                db.EmployeeTimeAndAttendanceSetting.Add(modelDb);
            }
            else
            {
                modelDb.SetUpdated<EmployeeTimeAndAttendanceSetting>();
            }
            modelDb.EnableWebPunch = model.EnableWebPunch;

            db.SaveChanges();
            return Json(new { status = "Success", message = "Success" });
        }
        [HttpGet]
        public ActionResult IndexByUser(int id)
        {
            var model = db.EmployeeTimeAndAttendanceSetting.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).OrderByDescending(o => o.Id);
            return PartialView("Index", model);
        }

        [HttpPost]
        public ActionResult Create(EmployeeTimeAndAttendanceSetting model)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeTimeAndAttendanceSetting.Add(model);
                db.SaveChanges();
                return Json(model);
            }

            return GetErrors();
        }

        [HttpPost]
        public ActionResult Edit(EmployeeTimeAndAttendanceSetting model)
        {
            if (ModelState.IsValid)
            {
                model.SetUpdated<EmployeeTimeAndAttendanceSetting>();
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexByUser");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.Country.Include(u => u.States)
            //                          .Include(u => u.MailingCountryUserContactInformation)
            //                          .Include(u => u.HomeCountryUserContactInformation)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.States.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.MailingCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.HomeCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
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