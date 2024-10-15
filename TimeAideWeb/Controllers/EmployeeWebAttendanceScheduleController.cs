using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAide.Web.Extensions;
using TimeAide.Services;

namespace TimeAide.Web.Controllers
{
  
    public class EmployeeWebAttendanceScheduleController : TimeAideWebControllers<WebScheduledEditor>
    {
        public override ActionResult Index()
        {
            try
            {
                var loginUser = db.UserInformation.Include("SupervisorSubDepartment.SubDepartment")
                                                                     .Include("SupervisorDepartment.Department")
                                                                     .Include("SupervisorEmployeeType.EmployeeType").FirstOrDefault(u => u.Id == SessionHelper.LoginId);

                ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);

                if (loginUser.SupervisorCompany.Count == 0 || loginUser.SupervisorCompany.Where(c => c.CompanyId == SessionHelper.SelectedCompanyId).Count() == 0)
                {
                    ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName").OrderBy(o => o.Text);
                }
                else
                {
                    if (loginUser.SupervisorDepartment.Count == 0)
                        ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
                    else
                        ViewBag.DepartmentId = new SelectList(loginUser.SupervisorDepartment.Select(d => d.Department), "Id", "DepartmentName").OrderBy(o => o.Text);
                }
                    
               
                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "WebAttendanceSchedule", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult SelfServiceWebSchedule()
        {
            int? id = SessionHelper.LoginId;
            ViewBag.selfServiceSchBeforeDD = "-15d";
            ViewBag.selfServiceSchAfterDD = "+15d";
            ViewBag.userId = id ?? 0;
            var appConfigSchBeforeDD = ApplicationConfigurationService.GetApplicationConfiguration("SelfServiceSchBeforeDays").ApplicationConfigurationValue;
            //var appConfigSchBeforeDD = db.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceSchBeforeDays")
            //                                                                       .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            var appConfigSchAfterDD = ApplicationConfigurationService.GetApplicationConfiguration("SelfServiceSchAfterDays").ApplicationConfigurationValue;

            //var appConfigSchAfterDD = db.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceSchAfterDays")
            //                                                                       .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            int intSchBeforeDD;
            int intSchAfterDD;

            if (int.TryParse(appConfigSchBeforeDD, out intSchBeforeDD))
            {
                ViewBag.selfServiceSchBeforeDD = string.Format("-{0}d", appConfigSchBeforeDD);
            }
            if (int.TryParse(appConfigSchAfterDD, out intSchAfterDD))
            {
                ViewBag.selfServiceSchAfterDD = string.Format("+{0}d", appConfigSchAfterDD);
            }

            return PartialView("SelfServiceWebSchedule");
        }
        public ActionResult GetSelfServiceWebScheduleDetail(UserFilterViewModel schFilter)
        {
            IList<EmployeeAttendenceSchDetail> userAttendanceSchDetail = null;
            //  var userId = 248;//SessionHelper.LoginId;
            var userId = schFilter.EmployeeId == 0 ? SessionHelper.LoginId : schFilter.EmployeeId.Value;
            try
            {
                byte[] companyLogo = db.Company.Find(SessionHelper.SelectedCompanyId).CompanyLogo;
                ViewBag.CompanyLogo = companyLogo;
                var userInfo = db.UserInformation.Find(userId);
                schFilter.EmployeeId = userInfo.EmployeeId;
                var userAttendanceSch = getWebAttendanceScheduleList(schFilter).FirstOrDefault();

                if (userAttendanceSch != null)
                {

                    ViewBag.SelfServiceWebSchedule = userAttendanceSch;
                    userAttendanceSchDetail = getWebAttendanceScheduleDetailData(userAttendanceSch.Id);
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetSelfServiceScheduleDetail");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView("SelfServiceWebScheduleDetail", userAttendanceSchDetail);
        }
        public ActionResult GetWebAttendanceSchedule(UserFilterViewModel schFilter)
        {

            var attSchList = getWebAttendanceScheduleList(schFilter);
            ViewBag.SelectedScheduleIds = string.Join(",", attSchList.Select(s => s.Id.ToString()));
            return PartialView("WebScheduleList", attSchList);
        }

        public ActionResult GetWebAttendanceScheduleDetail(int schId)
        {

            var attSchDetailData = getWebAttendanceScheduleDetailData(schId);

            // return PartialView("");
            return PartialView("WebScheduleDetail", attSchDetailData);
        }
        private List<EmployeeAttendenceSchedule> getWebAttendanceScheduleList(UserFilterViewModel schFilter)
        {
            List<EmployeeAttendenceSchedule> retAttendenceScheduleList = null;

            try
            {
                var userList = getFilterEmployeeList(schFilter);

                var userSchedule = db.EmployeeWebScheduledPeriod
                    .Where(w => schFilter.ScheduleDate >= w.PeriodStartDate && schFilter.ScheduleDate <= w.PeriodEndDate).ToList()
                    .Join(userList,                                      //Inner Table t join
                     usch => usch.UserInformationId,     //Condition from outer table
                     urs => urs.Id,          //Condition from inner table
                     (usch, urs) => new EmployeeAttendenceSchedule
                     {                                //Result
                      
                          Id = usch.Id,
                          UserInformationId = urs.Id,
                          EmployeeId = urs.EmployeeId,                         
                          CompanyId =     urs.CompanyId,
                          sUserName =     urs.ShortFullName,
                          sWeekID   =     usch.PayWeekNumber.ToString(),
                          sNote     =     usch.Note,
                          dStartDate =    usch.PeriodStartDate,
                          dEndDate   =    usch.PeriodEndDate,
                          dblPeriodHours = usch.PeriodHours,
                          nPayPeriodType = usch.PayFrequencyId,
                          nPayWeekNum    = usch.PayWeekNumber,
                          nScheduleId    = 0
                      }).ToList();


                retAttendenceScheduleList = userSchedule;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retAttendenceScheduleList;

        }
        private List<EmployeeAttendenceSchDetail> getWebAttendanceScheduleDetailData(int schId)
        {
            List<EmployeeAttendenceSchDetail> retAttendenceScheduleDetail = null;

            try
            {
              

                var userSchedule = db.EmployeeWebScheduledPeriod
                    .Where(w => w.Id == schId)
                    .Select(s=> new EmployeeAttendenceSchedule
                      {
                          Id = s.Id,
                          UserInformationId = s.UserInformationId,
                          EmployeeId = s.UserInformation.EmployeeId,                       
                          CompanyId = s.UserInformation.CompanyId,                        
                          sUserName = s.UserInformation.ShortFullName,
                          sWeekID = s.PayWeekNumber.ToString(),
                          sNote = s.Note,
                          dStartDate = s.PeriodStartDate,
                          dEndDate = s.PeriodEndDate,
                          dblPeriodHours = s.PeriodHours,
                          nPayPeriodType = s.PayFrequencyId,
                          nPayWeekNum = s.PayWeekNumber,
                          nScheduleId = 0
                      }).FirstOrDefault();

                var userScheduleDetail = db.EmployeeWebScheduledPeriodDetail.Where(w => w.UserInformationId == userSchedule.UserInformationId &&
                                                               w.PunchDate >= userSchedule.dStartDate && w.PunchDate <= userSchedule.dEndDate)
                                          .OrderBy(o => o.PunchDate).AsEnumerable()
                                          .Select(s => new EmployeeAttendenceSchDetail
                                          {
                                              Id = s.Id,
                                              UserInformationId = s.UserInformationId,
                                              EmployeeId = s.UserInformation.EmployeeId,
                                              sUserName = s.UserInformation.ShortFullName,
                                              dPunchDate = s.PunchDate,
                                              dPunchIn1 = s.TimeIn1,
                                              dPunchOut1 = s.TimeOut1,
                                              dPunchIn2 = s.TimeIn2,
                                              dPunchOut2 = s.TimeOut2,
                                              nWorkDayType = s.WorkDayTypeId,
                                              sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (s.WorkDayTypeId ?? 0)).FirstOrDefault().Name,
                                              nPunchNum = s.NoOfPunch,
                                              dblDayHours = s.DayHours,
                                              nPayWeekNum = s.PayWeekNumber,
                                              nSchedModPeriodSummId = s.EmployeeWebScheduledPeriodId,
                                              nScheduleId = userSchedule.nScheduleId,
                                              sNote = s.Note

                                          }).ToList();

                retAttendenceScheduleDetail = userScheduleDetail;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retAttendenceScheduleDetail;

        }
        private EmployeeAttendenceSchDetail getWebAttendanceScheduleDetailEditData(int schDetailId)
        {
            EmployeeAttendenceSchDetail retAttendenceScheduleDetail = null;

            try
            {
              
                var userScheduleDetailEditRecord = db.EmployeeWebScheduledPeriodDetail.Where(w => w.Id == schDetailId)
                                          .AsEnumerable()
                                          .Select(s => new EmployeeAttendenceSchDetail
                                          {
                                              Id = s.Id,
                                              UserInformationId = s.UserInformationId,  
                                              EmployeeId = s.UserInformation.EmployeeId,
                                              sUserName = s.UserInformation.ShortFullName,
                                              dPunchDate = s.PunchDate,
                                              dPunchIn1 = s.TimeIn1,
                                              dPunchOut1 = s.TimeOut1,
                                              dPunchIn2 = s.TimeIn2,
                                              dPunchOut2 = s.TimeOut2,
                                              nWorkDayType = s.WorkDayTypeId,
                                              nPunchNum = s.NoOfPunch,
                                              dblDayHours = s.DayHours,
                                              nPayWeekNum = s.PayWeekNumber,
                                              sWeekID = s.PayWeekNumber.ToString(),
                                              nScheduleId = 0,                                             
                                              nSchedModPeriodSummId = s.EmployeeWebScheduledPeriod.Id
                                          }).FirstOrDefault();

                retAttendenceScheduleDetail = userScheduleDetailEditRecord;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retAttendenceScheduleDetail;

        }
        private IEnumerable<UserInformationViewModel> getFilterEmployeeList(UserFilterViewModel schFilter)
        {
            IEnumerable<UserInformationViewModel> retFilterEmployeeList;

            try
            {
                var employeeId = 0;
                var employeeName = "";
                var positionId = 0;
                var departmentId = 0;
                if (schFilter != null)
                {
                    employeeId = schFilter.EmployeeId ?? 0;
                    employeeName = schFilter.EmployeeName == null ? employeeName : schFilter.EmployeeName;
                    positionId = schFilter.PositionId ?? 0;
                    departmentId = schFilter.DepartmentId ?? 0;

                }
                //Calling Db Procedure
                var userInformationList = db.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                         0, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).
                                         Where(w => w.DepartmentId == (departmentId == 0 ? w.DepartmentId : departmentId));



                retFilterEmployeeList = userInformationList;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retFilterEmployeeList;

        }
        public ActionResult EditWebAttendenceSchedule(int schId)
        {
            if (schId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                EmployeeAttendenceSchedule model = null;
                AllowEdit();
                var schEntity = db.EmployeeWebScheduledPeriod.Find(schId);
                if (schEntity != null)
                {
                    model = new EmployeeAttendenceSchedule()
                    {
                        Id = schEntity.Id,
                        EmployeeId = schEntity.UserInformation.EmployeeId,
                        UserInformationId = schEntity.UserInformationId,                      
                        nPayWeekNum = schEntity.PayWeekNumber,
                        dStartDate = schEntity.PeriodStartDate,
                        dEndDate = schEntity.PeriodEndDate,
                        sNote = schEntity.Note
                    };
                }

                return PartialView("EditWebSchedule", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AttendanceWebSchedule", "EditAttendenceSchedule");
                return PartialView("Error", handleErrorInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EditWebAttendenceSchedule(EmployeeAttendenceSchedule model)
        {
            string status = "Success";
            string message = "Successfully Updated!";

            try
            {
                if (model.Id == 0)
                {
                    throw new Exception("Invalid Schedule record.");
                }


                var schedModPeriodSummEntity = db.EmployeeWebScheduledPeriod.Find(model.Id);
                if (schedModPeriodSummEntity != null)
                {
                    schedModPeriodSummEntity.Note = model.sNote;
                    schedModPeriodSummEntity.ModifiedBy = SessionHelper.LoginId;
                    schedModPeriodSummEntity.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                }


            }

            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, retData = model });
        }
        public ActionResult EditWebAttendenceScheduleDetail(int schDetailId)
        {

            if (schDetailId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                AllowEdit();
                var scheduleEditData = getWebAttendanceScheduleDetailEditData(schDetailId);
                ViewBag.nWorkDayType = new SelectList(AttendenceScheduleMasterDDV.WorkDayTypes, "Id", "Name", scheduleEditData.nWorkDayType);
                ViewBag.nPunchNum = new SelectList(AttendenceScheduleMasterDDV.PunchNumTypes, "Id", "Name", scheduleEditData.nPunchNum);
                return PartialView("EditWebScheduleDetail", scheduleEditData);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AttendanceSchedule", "EditAttendenceScheduleDetail");
                return PartialView("Error", handleErrorInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult EditWebAttendenceScheduleDetail(EmployeeAttendenceSchDetail model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            var dayHours = 0.0;
            try
            {
                if (model.dPunchDate == null)
                {
                    throw new Exception("Punch date is missing.");
                }
                var punchDate = model.dPunchDate.Value;
                // var minAllowPunchDate = punchDate.AddDays(-1);
                var minAllowPunchDate = model.dPunchDate.Value;
                var maxAllowPunchDate = punchDate.AddDays(2).AddMinutes(-1);

                var dPunchIn1 = model.dPunchIn1 ?? punchDate;
                var dPunchOut1 = model.dPunchOut1 ?? punchDate;
                var dPunchIn2 = model.dPunchIn2 ?? punchDate;
                var dPunchOut2 = model.dPunchOut2 ?? punchDate;
                //var LoginUser_OldId = timeAideWebContext.UserInformation.Find(SessionHelper.LoginId).Old_Id;
                switch (model.nPunchNum)
                {
                    case 1:
                        if (dPunchIn1 < minAllowPunchDate && dPunchIn1 > maxAllowPunchDate)
                        {
                            throw new Exception("PunchIn1 should be between one day before or after to punch Date.");
                        }

                        break;
                    case 2:
                        if ((dPunchIn1 < minAllowPunchDate || dPunchIn1 > maxAllowPunchDate)
                            || (dPunchOut1 < minAllowPunchDate || dPunchOut1 > maxAllowPunchDate))
                        {
                            throw new Exception("Punch In1/Out1 should be between one day before or after to punch Date.");
                        }
                        var diffInOut1Mins = (dPunchOut1.Subtract(dPunchIn1).TotalMinutes);
                        if (diffInOut1Mins < 0)
                        {
                            throw new Exception("Punch Out1 should not be before Punch In1");
                        }
                        dayHours = Math.Round((diffInOut1Mins / 60), 2);

                        break;
                    case 4:
                        if ((dPunchIn1 < minAllowPunchDate || dPunchIn1 > maxAllowPunchDate)
                            || (dPunchOut1 < minAllowPunchDate || dPunchOut1 > maxAllowPunchDate))
                        {
                            throw new Exception("Punch In1/Out1 should be between one day before or after to punch Date.");
                        }
                        var diffInOut11Mins = (dPunchOut1.Subtract(dPunchIn1).TotalMinutes);
                        if (diffInOut11Mins < 0)
                        {
                            throw new Exception("Punch Out1 should not be before Punch In1");
                        }
                        if ((dPunchIn2 < minAllowPunchDate || dPunchIn2 > maxAllowPunchDate)
                            || (dPunchOut2 < minAllowPunchDate || dPunchOut2 > maxAllowPunchDate))
                        {
                            throw new Exception("Punch In2/Out2 should be between one day before or after to punch Date.");
                        }
                        var diffInOut22Mins = (dPunchOut2.Subtract(dPunchIn2).TotalMinutes);
                        if (diffInOut22Mins < 0)
                        {
                            throw new Exception("Punch Out2 should not be before Punch In2");
                        }
                        if ((dPunchIn2 >= dPunchIn1 && dPunchIn2 <= dPunchOut1)
                            || (dPunchOut2 >= dPunchIn1 && dPunchOut2 <= dPunchOut1))
                        {
                            throw new Exception("Punch In2/Out2 should not be overlapped with Punch In1/Out1.");
                        }
                        dayHours = Math.Round(((diffInOut11Mins + diffInOut22Mins) / 60), 2);
                        break;
                }
                model.dblDayHours = dayHours;

                var schedModDailyDetailEntity = db.EmployeeWebScheduledPeriodDetail.Find(model.Id);
                if (schedModDailyDetailEntity != null)
                {
                    schedModDailyDetailEntity.WorkDayTypeId = model.nWorkDayType;
                    schedModDailyDetailEntity.NoOfPunch = model.nPunchNum??0;
                    schedModDailyDetailEntity.TimeIn1 = model.dPunchIn1;
                    schedModDailyDetailEntity.TimeIn2 = model.dPunchIn2;
                    schedModDailyDetailEntity.TimeOut1 = model.dPunchOut1;
                    schedModDailyDetailEntity.TimeOut2 = model.dPunchOut2;
                    schedModDailyDetailEntity.DayHours = model.dblDayHours;
                    schedModDailyDetailEntity.Note = model.sNote;
                    schedModDailyDetailEntity.ModifiedDate = DateTime.Now;
                    schedModDailyDetailEntity.ModifiedBy = SessionHelper.LoginId;
                    db.SaveChanges();

                    var totalPeriodHours = db.EmployeeWebScheduledPeriodDetail.Where(w => w.UserInformationId == model.UserInformationId &&
                                                              w.PayWeekNumber == model.nPayWeekNum && w.WorkDayTypeId == 1)
                                                              .Select(s => s.DayHours ?? 0).Sum();

                    var webScheduledPeriodEntity = db.EmployeeWebScheduledPeriod.Find(model.nSchedModPeriodSummId);
                    if (webScheduledPeriodEntity != null)
                    {
                        webScheduledPeriodEntity.PeriodHours = totalPeriodHours;
                        webScheduledPeriodEntity.ModifiedDate = DateTime.Now;
                        webScheduledPeriodEntity.ModifiedBy = SessionHelper.LoginId;
                        db.SaveChanges();
                    }

                    model.dblTotalPeriodHours = totalPeriodHours;
                    model.sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w => w.Id == (model.nWorkDayType ?? 0)).FirstOrDefault().Name;
                }

            }

            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, retData = model });
        }
        public JsonResult AjaxGetPunchScheduleDates(EmployeeAttendenceSchDetail model)
        {
            string status = "Success";
            string message = "Successfully fetch!";
            EmployeeAttendenceSchDetail punchData = null;
            // var applicantEntity = timeAideWebContext.ApplicantInformation.Find(id);
            try
            {
                var punchDate = model.dPunchDate ?? DateTime.Today;
                var dayHours = 0.0;
                var punchDayOfWk = (int)punchDate.DayOfWeek;
                model.dPunchIn1 = punchDate;
                model.dPunchOut1 = punchDate;
                model.dPunchIn2 = punchDate;
                model.dPunchOut2 = punchDate;
                var schInfo = db.BaseSchedule
                                .Where(w => w.Id == model.nScheduleId).FirstOrDefault();
                var dayInfo = db.BaseScheduleDayInfo
                              .Where(w => w.BaseScheduleId == model.nScheduleId && w.DayOfWeek == punchDayOfWk).FirstOrDefault();

                if (dayInfo != null)
                {
                    var schPunchCount = schInfo.NoOfPunch;
                    //Punch in/out 1
                    var dIn1 = dayInfo.TimeIn1 ?? punchDate;
                    var dOut1 = dayInfo.TimeOut1 ?? punchDate;
                    var in1DayOffSet = 0;
                    var out1DayOffSet = 0;
                    if (dIn1.ToString("tt") == "PM" && dOut1.ToString("tt") == "AM")
                    {
                        out1DayOffSet = 1;
                    }
                    TimeSpan in1Time = new TimeSpan(in1DayOffSet, dIn1.Hour, dIn1.Minute, dIn1.Second);
                    TimeSpan out1Time = new TimeSpan(out1DayOffSet, dOut1.Hour, dOut1.Minute, dOut1.Second);

                    var diffInOut1Mins = out1Time.Subtract(in1Time).TotalMinutes;
                    var in1DateTime = punchDate.Add(in1Time);
                    var out1DateTime = in1DateTime.AddMinutes(Math.Abs(diffInOut1Mins));
                    //Punch in/out 2
                    var dIn2 = dayInfo.TimeIn2 ?? punchDate;
                    var dOut2 = dayInfo.TimeOut2 ?? punchDate;
                    var in2DayOffSet = 0;
                    var out2DayOffSet = 0;
                    if (dIn2.ToString("tt") == "PM" && dOut2.ToString("tt") == "AM")
                    {
                        out2DayOffSet = 1;
                    }
                    TimeSpan in2Time = new TimeSpan(in2DayOffSet, dIn2.Hour, dIn2.Minute, dIn2.Second);
                    TimeSpan out2Time = new TimeSpan(out2DayOffSet, dOut2.Hour, dOut2.Minute, dOut2.Second);
                    var diffInOut2Mins = out2Time.Subtract(in2Time).TotalMinutes;
                    var in2DateTime = punchDate.Add(in2Time);
                    var out2DateTime = in2DateTime.AddMinutes(Math.Abs(diffInOut2Mins));

                    switch (model.nPunchNum)
                    {
                        case 1:
                            if (schPunchCount >= 1)
                            {
                                model.dPunchIn1 = in1DateTime;
                            }
                            break;
                        case 2:

                            model.dPunchIn1 = in1DateTime;
                            if (schPunchCount >= 2)
                            {
                                model.dPunchOut1 = out1DateTime;
                                dayHours = Math.Round((diffInOut1Mins / 60), 2);
                            }
                            break;
                        case 4:

                            model.dPunchIn1 = in1DateTime;
                            model.dPunchOut1 = out1DateTime;
                            dayHours = Math.Round((diffInOut1Mins / 60), 2);
                            if (schPunchCount >= 4)
                            {
                                model.dPunchIn2 = in2DateTime;
                                model.dPunchOut2 = out2DateTime;
                                dayHours = Math.Round((diffInOut1Mins / 60) + (diffInOut2Mins / 60), 2);
                            }
                            break;
                    }
                    model.dblDayHours = dayHours;


                }
                //punchData = model;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, punchData = model });
        }

        public JsonResult GetSelfServiceCurrentWebScheduleWidget(int? id)
        {
            // var userId = 248;//SessionHelper.LoginId;
            var userId = id ?? SessionHelper.LoginId;
            var userInfo = db.UserInformation.Where(w => w.Id == userId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();
            string status = "Success";
            string message = "Successfully fetch!";
            EmployeeAttendenceSchedule widgetData = new EmployeeAttendenceSchedule();
            UserFilterViewModel schFilter = null;
            try
            {
                schFilter = new UserFilterViewModel() { ScheduleDate = DateTime.Today, EmployeeId = userInfo.EmployeeId };
                var userAttendanceSch = getWebAttendanceScheduleList(schFilter).FirstOrDefault();

                if (userAttendanceSch != null)
                {
                    var totalDays = getWebAttendanceScheduleDetailData(userAttendanceSch.Id).
                                                     Where(w => w.nWorkDayType == 1).Count();

                    string schStartDate = userAttendanceSch.dStartDate == null ? ""
                                                                        : (userAttendanceSch.dStartDate ?? DateTime.Today).ToString("MM/dd/yyyy");
                    string schEndDate = userAttendanceSch.dEndDate == null ? ""
                                                                        : (userAttendanceSch.dEndDate ?? DateTime.Today).ToString("MM/dd/yyyy");
                    widgetData.Id = userAttendanceSch.Id;
                    widgetData.sNote = String.Format("{0}-{1}", schStartDate, schEndDate);
                    widgetData.nPayWeekNum = totalDays;
                    widgetData.dblPeriodHours = userAttendanceSch.dblPeriodHours;
                }
            }

            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, widgetData = widgetData });
        }
       
        // GET: UserContactInformation/Edit/5

        
    }
}
