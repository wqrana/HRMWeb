using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
    {
        public class BaseScheduleController : TimeAideWebControllers<BaseSchedule>
        {
            
            public override ActionResult Index()
            {
                try
                {
                    AllowView();
                    return PartialView();
                }
                catch (AuthorizationException ex)
                {
                    Exception exception = new Exception(ex.ErrorMessage);
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(BaseSchedule).Name, "Index");
                    return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }
            }
            public ActionResult IndexByBaseSchedule()
            {
                try
                {
                    AllowView();

                    var entitySet = db.GetAllByCompany<BaseSchedule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                    var model = entitySet.OrderByDescending(e => e.CreatedDate).ToList();
                    return PartialView(model);
                }
                catch (AuthorizationException ex)
                {
                    Exception exception = new Exception(ex.ErrorMessage);
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(BaseSchedule).Name, "IndexByBaseSchedule");
                    return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }

            }                  
                        
            public override ActionResult Details(int? id)
            {
                try
                {
                    BaseSchedule model = null;
                    AllowView();
                    if (id == 0)
                        model = db.GetAllByCompany<BaseSchedule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderByDescending(o => o.Id).FirstOrDefault();
                    else
                        model = db.Find<BaseSchedule>(id.Value, SessionHelper.SelectedClientId);

                    if (model == null)
                        model = new BaseSchedule();

                    //return View(city);
                    ViewBag.Label = ViewBag.Label + " - Detail";
                    return PartialView(model);
                }
                catch (AuthorizationException ex)
                {
                    Exception exception = new Exception(ex.ErrorMessage);
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "BaseSchedule", "Details");
                    return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }
            }
        public override ActionResult Create()
        {
            try
            {
                AllowAdd();               
                
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "BaseSchedule", "Create");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult Edit(int? id)
            {

                try
                {
                    AllowEdit();
                    var model = db.BaseSchedule.Find(id ?? 0);                    

                    ViewBag.CanBeAssignedToCurrentCompany = false;
                   
                    var companies = (model as BaseCompanyObjects).GetRefferredCompanies();
                    if (companies.Count == 0 || (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
                    {
                        ViewBag.CanBeAssignedToCurrentCompany = true;
                    }
                    return PartialView(model);
                }
                catch (AuthorizationException ex)
                {
                    Exception exception = new Exception(ex.ErrorMessage);
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "Create");
                    return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }


            }
            [HttpPost]
            public JsonResult CreateEdit(BaseScheduleViewModel model)
            {
                string status = "Success";
                string message = "Successfully Added/Updated!";
                int id = 0;
               
                BaseSchedule baseSchEntity = null;
                try
                {
                    var isAlreadyExist = db.GetAllByCompany<BaseSchedule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                            .Where(w => (w.Id != model.Id) && (w.Name.ToLower() == model.Name.ToLower()))
                                            .Count();
                    if (isAlreadyExist > 0)
                    {
                        status = "Error";
                        message = "Base schedule name is already Exists";
                    }
                    else
                    {
                        if (model.Id == 0)
                        {
                            baseSchEntity = new BaseSchedule();
                            db.BaseSchedule.Add(baseSchEntity);
                        }
                        else
                        {
                            baseSchEntity = db.BaseSchedule.Find(model.Id);
                            baseSchEntity.ModifiedBy = SessionHelper.LoginId;
                            baseSchEntity.ModifiedDate = DateTime.Now;
                        }
                        baseSchEntity.Name = model.Name;
                        baseSchEntity.Description = model.Description;
                        baseSchEntity.NoOfPunch = model.NoOfPunch;
                        baseSchEntity.IsSunday = model.IsSunday;
                        baseSchEntity.IsMonday = model.IsMonday;
                        baseSchEntity.IsTuesday = model.IsTuesday;
                        baseSchEntity.IsWednesday = model.IsWednesday;
                        baseSchEntity.IsThursday = model.IsThursday;
                        baseSchEntity.IsFriday = model.IsFriday;
                        baseSchEntity.IsSaturday = model.IsSaturday;
                        baseSchEntity.CompanyId = model.IsAllCompanies ? null : (int?)SessionHelper.SelectedCompanyId;
                        using (var baseSchDBTrans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.SaveChanges();
                                id = baseSchEntity.Id;
                                if (id > 0){
                                    if (model.Id == 0){
                                     foreach(var dayInfo in model.BaseScheduleDaysInfo){
                                        dayInfo.BaseScheduleId = id;
                                        SetNoOfPunchInOut(dayInfo,dayInfo.TimeIn1.Value);
                                        db.BaseScheduleDayInfo.Add(dayInfo);
                                        }                                  
                                    }
                                    else {
                                    foreach (var dayInfo in model.BaseScheduleDaysInfo)
                                    {
                                        var dayInfoEntity = baseSchEntity.BaseScheduleDayInfos.Where(w => w.DayOfWeek == dayInfo.DayOfWeek).FirstOrDefault();
                                        if (dayInfoEntity != null)
                                        {
                                            dayInfoEntity.NoOfPunch = dayInfo.NoOfPunch;
                                            dayInfoEntity.TimeIn1 = dayInfo.TimeIn1;
                                            dayInfoEntity.TimeOut1 = dayInfo.TimeOut1;
                                            dayInfoEntity.TimeIn2 = dayInfo.TimeIn2;
                                            dayInfoEntity.TimeOut2 = dayInfo.TimeOut2;
                                            SetNoOfPunchInOut(dayInfo, dayInfo.TimeIn1.Value);
                                        }
                                    }

                                }
                                    db.SaveChanges();
                                }
                                else
                                {
                                    throw new Exception("Error Insterting record");
                                }

                            baseSchDBTrans.Commit();
                            }
                            catch (Exception ex)
                            {
                            baseSchDBTrans.Rollback();
                                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                                status = "Error";
                                message = ex.Message;
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

                return Json(new {  status = status, message = message });
            }
            private void SetNoOfPunchInOut(BaseScheduleDayInfo dayInfo,DateTime baseDate)
        {
            DateTime defaultDatetime = DateTime.Parse(baseDate.ToShortDateString());
            switch (dayInfo.NoOfPunch)
            {
                case 1:
                    dayInfo.TimeOut1 = defaultDatetime;
                    dayInfo.TimeIn2 = defaultDatetime;
                    dayInfo.TimeOut2 = defaultDatetime;
                    break;
                case 2:
                    dayInfo.TimeIn2 = defaultDatetime;
                    dayInfo.TimeOut2 = defaultDatetime;
                    break;
            }
            
        }
        [HttpPost]
        public JsonResult AjaxDayInfoHours(BaseScheduleDayInfo model)
        {

            return Json(new { ShiftHours = model.ShiftHours});
        }

            [HttpPost]
            public JsonResult ConfirmDelete(int id)
            {
                string status = "Success";
                string message = "Successfully Deleted!";
                var baseScheduleEntity = db.BaseSchedule.Find(id);
                using (var baseScheduleDBTrans = db.Database.BeginTransaction())
                {
                    try
                    {

                    baseScheduleEntity.ModifiedBy = SessionHelper.LoginId;
                    baseScheduleEntity.ModifiedDate = DateTime.Now;
                    baseScheduleEntity.DataEntryStatus = 0;
                        //var baseScheduleDaysInfo = db.AccrualRuleTier.Where(w => w.AccrualRuleId == id && w.DataEntryStatus == 1);
                        //if (accrualRuleTiers.Count() > 0)
                        //{
                            foreach (var baseScheduleDayInfo in baseScheduleEntity.BaseScheduleDayInfos)
                            {
                            baseScheduleDayInfo.DataEntryStatus = 0;
                            baseScheduleDayInfo.ModifiedBy = SessionHelper.LoginId;
                            baseScheduleDayInfo.ModifiedDate = DateTime.Now;

                               
                            }

                        //}

                        db.SaveChanges();
                        baseScheduleDBTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        baseScheduleDBTrans.Rollback();
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }

                    return Json(new { status = status, message = message });
                }
            }

        }
    }

