using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeWebScheduleController : TimeAideWebSecurotyBaseControllers
    {
        public EmployeeWebScheduleController() : base(typeof(WebSchedule).Name)
        {
        }
        // GET: EmployeeLicenseAndBalance
        public ActionResult IndexByUser(int id)
        {
            try
            {
                AllowView();
                return PartialView("Index");
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "IndexByUser");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public ActionResult Details(int id)
        {
            try
            {
                AllowView();
                var model = db.UserInformation.Find(id);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "AccrualRuleDetail");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public JsonResult EditWebScheduleType(WebScheduleViewModel model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    var userInfoEntity = db.UserInformation.Find(model.UserInformationId);
                    if (userInfoEntity != null)
                    {
                        userInfoEntity.IsRotatingSchedule = model.IsRotatingSchedule;
                        userInfoEntity.ModifiedBy = SessionHelper.LoginId;
                        userInfoEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                    }

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

            return Json(new { status = status, message = message });
        }

        public ActionResult GetWebScheduleTypeDetail(int id)
        {
            var model = new WebScheduleViewModel();
            try
            {
                var userInfoEntity = db.UserInformation.Find(id);
                if (userInfoEntity != null)
                {
                    model.UserInformationId = id;
                    model.IsRotatingSchedule = userInfoEntity.IsRotatingSchedule;
                    if ((model.IsRotatingSchedule ?? false) == false)
                    {
                        model.EmployeeFutureSchedule = db.EmployeeFutureSchedule
                                                            .Where(w => w.DataEntryStatus == 1 &&
                                                                        w.UserInformationId == model.UserInformationId)
                                                            .OrderByDescending(o=>o.StartDate);
                    }
                    else
                    {
                        model.EmployeeRotatingSchedule = db.EmployeeRotatingSchedule
                                                            .Where(w => w.DataEntryStatus == 1 &&
                                                                        w.UserInformationId == model.UserInformationId);
                    }

                }

                return PartialView("WebScheduleTypeView", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "CreateAccrualRule");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public ActionResult AddEditFutureSchedule(int id)
        {
            AllowAdd();
            EmployeeFutureSchedule model = null;
            if (id == 0)
            {
                model = new EmployeeFutureSchedule();
            }
            else
            {
                model = db.EmployeeFutureSchedule.Find(id);
            }
            ViewBag.BaseScheduleId = new SelectList(db.GetAllByCompany<BaseSchedule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "Name", model.BaseScheduleId);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult AddEditFutureSchedule(EmployeeFutureSchedule model)
        {
            string status = "Success";
            string message = "Successfully Save!";

            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeFutureSchedule empFutureScheduleEntity = null;
                    var isExistCount = db.EmployeeFutureSchedule.Where(w => w.UserInformationId == model.UserInformationId
                                                                         && w.StartDate == model.StartDate && w.Id != model.Id
                                                                         && w.DataEntryStatus == 1).Count();
                    if (isExistCount > 0)
                    {
                        throw new Exception("Future schedule is already exist on start date.");
                    }
                    if(model.StartDate<= DateTime.Today)
                    {
                        //throw new Exception("Start date of schedule should be in future.");
                        message = "Schedule start date is in past, save successfully!";
                    }
                    if (model.Id == 0)
                    {
                        empFutureScheduleEntity = new EmployeeFutureSchedule();
                        empFutureScheduleEntity.UserInformationId = model.UserInformationId;
                        db.EmployeeFutureSchedule.Add(empFutureScheduleEntity);
                    }
                    else
                    {
                        empFutureScheduleEntity = db.EmployeeFutureSchedule.Find(model.Id);
                        empFutureScheduleEntity.ModifiedBy = SessionHelper.LoginId;
                        empFutureScheduleEntity.ModifiedDate = DateTime.Now;
                    }
                    empFutureScheduleEntity.StartDate = model.StartDate;
                    empFutureScheduleEntity.BaseScheduleId = model.BaseScheduleId;
                    empFutureScheduleEntity.Note = model.Note;
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

            return Json(new { status = status, message = message });
        }
        public ActionResult DeleteFutureSchedule(int id)
        {          
            var  model = db.EmployeeFutureSchedule.Find(id);         
            
            return PartialView(model);
        }
        public JsonResult ConfirmDeleteFutureSchedule(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var futureSchEntity = db.EmployeeFutureSchedule.Find(id);
            try
            {
                futureSchEntity.ModifiedBy = SessionHelper.LoginId;
                futureSchEntity.ModifiedDate = DateTime.Now;
                futureSchEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult AddEditRotatingSchedule(int id)
        {
            AllowAdd();
            EmployeeRotatingSchedule model = null;
            if (id == 0)
            {
                model = new EmployeeRotatingSchedule();
            }
            else
            {
                model = db.EmployeeRotatingSchedule.Find(id);
            }
            ViewBag.BaseScheduleId = new SelectList(db.GetAllByCompany<BaseSchedule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "Name", model.BaseScheduleId);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult AddEditRotatingSchedule(EmployeeRotatingSchedule model)
        {
            string status = "Success";
            string message = "Successfully Save!";

            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeRotatingSchedule empRotatingScheduleEntity = null;
                    var isExistCount = db.EmployeeRotatingSchedule.Where(w => w.UserInformationId == model.UserInformationId
                                                                         && w.BaseScheduleId == model.BaseScheduleId && w.Id != model.Id
                                                                         && w.DataEntryStatus == 1).Count();
                    if (isExistCount > 0)
                    {
                        throw new Exception("Rotating schedule is already exist.");
                    }
                    
                    if (model.Id == 0)
                    {
                        empRotatingScheduleEntity = new EmployeeRotatingSchedule();
                        empRotatingScheduleEntity.UserInformationId = model.UserInformationId;
                        db.EmployeeRotatingSchedule.Add(empRotatingScheduleEntity);
                    }
                    else
                    {
                        empRotatingScheduleEntity = db.EmployeeRotatingSchedule.Find(model.Id);
                        empRotatingScheduleEntity.ModifiedBy = SessionHelper.LoginId;
                        empRotatingScheduleEntity.ModifiedDate = DateTime.Now;
                    }

                    empRotatingScheduleEntity.BaseScheduleId = model.BaseScheduleId;
                    empRotatingScheduleEntity.Note = model.Note;
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

            return Json(new { status = status, message = message });
        }
        public ActionResult DeleteRotatingSchedule(int id)
        {
            var model = db.EmployeeRotatingSchedule.Find(id);

            return PartialView(model);
        }
        public JsonResult ConfirmDeleteRotatingSchedule(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var rotatingSchEntity = db.EmployeeRotatingSchedule.Find(id);
            try
            {
                rotatingSchEntity.ModifiedBy = SessionHelper.LoginId;
                rotatingSchEntity.ModifiedDate = DateTime.Now;
                rotatingSchEntity.DataEntryStatus = 0;
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
        
