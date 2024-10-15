
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

namespace TimeAide.Web.Controllers
{
    //public class WorkDayType
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
    //public class PunchNumType
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
    public class EmployeeAttendanceScheduleController : BaseTAWindowRoleRightsController<EmployeeAttendenceSchedule>
    {
        //private List<WorkDayType> WorkDayTypes = new List<WorkDayType>() { new WorkDayType {Id=1,Name="Work Day"},
        //                                                          new WorkDayType {Id=0,Name="UnScheduled"} };
        //private List<PunchNumType> PunchNumTypes = new List<PunchNumType>() {
        //                                                            new PunchNumType {Id=0,Name="0"},
        //                                                            new PunchNumType {Id=1,Name="1"},
        //                                                            new PunchNumType {Id=2,Name="2"},
        //                                                            new PunchNumType {Id=4,Name="4"}};
        [ChildActionOnly]
        public ActionResult GetScheduleFilters()
        {
            try
            {
                SetFilterDropdownList();

                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetScheduleFilters");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult Index()
        {
            try
            {

                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }        
        public ActionResult IndexByCalendar()
        {
            try
            {
                ViewBag.Title = "Schedule Calendar";
                ViewBag.DefaultViewTypeId = 0;
                ViewBag.PositionId = new SelectList(timeAideWebContext.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);
                ViewBag.DepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
               
                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "IndexByCalendar");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult IndexByOverview()
        {
            try
            {
                ViewBag.Title = "Schedule Overview";
                ViewBag.DefaultViewTypeId = 1;
                ViewBag.PositionId = new SelectList(timeAideWebContext.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);
                ViewBag.DepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);

                return PartialView("IndexByCalendar");
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "IndexByOverview");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult GetScheduleOverview(UserFilterViewModel schFilter)
        {
            try
            {
                var model = getAttendanceScheduleCalendarData(schFilter);
                return PartialView("ScheduleOverview", model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetScheduleOverview");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult GetScheduleCalendar(UserFilterViewModel schFilter)
        {
            try
            {
                var model = getAttendanceScheduleCalendarData(schFilter);
                return PartialView("ScheduleCalendarView", model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetScheduleCalendar");
                return PartialView("Error", handleErrorInfo);
            }
        }

        public ActionResult SelfServiceSchedule(string id)
        {
            //int? id = SessionHelper.LoginId;
            ViewBag.selfServiceSchBeforeDD = "-15d";
            ViewBag.selfServiceSchAfterDD = "+15d";
            var userId = id == null ? SessionHelper.LoginId : int.Parse(Encryption.DecryptURLParm(id));
            ViewBag.userId = userId;
            var appConfigSchBeforeDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceSchBeforeDays")
                                                                                   .Select(w =>(w.ApplicationConfigurationValue)).FirstOrDefault();

            var appConfigSchAfterDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceSchAfterDays")
                                                                                   .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            int intSchBeforeDD ;
            int intSchAfterDD ;

           if( int.TryParse(appConfigSchBeforeDD,out intSchBeforeDD))
            {
                ViewBag.selfServiceSchBeforeDD = string.Format("-{0}d", appConfigSchBeforeDD);
            }
            if (int.TryParse(appConfigSchAfterDD, out intSchAfterDD))
            {
                ViewBag.selfServiceSchAfterDD = string.Format("+{0}d", appConfigSchAfterDD);
            }
        
            return PartialView("SelfServiceSchedule");
        }
        public ActionResult GetSelfServiceScheduleDetail(UserFilterViewModel schFilter)
        {
            IList<EmployeeAttendenceSchDetail> userAttendanceSchDetail = null;
            //  var userId = 248;//SessionHelper.LoginId;
            var userId = schFilter.EmployeeId == 0 ? SessionHelper.LoginId : schFilter.EmployeeId.Value;
            try
            {
                byte[] companyLogo = timeAideWebContext.Company.Find(SessionHelper.SelectedCompanyId).CompanyLogo;
                ViewBag.CompanyLogo = companyLogo;
                var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();
                schFilter.EmployeeId = userInfo.EmployeeId;
                var userAttendanceSch = getAttendanceScheduleList(schFilter).Where(w => w.OldCompanyId == userInfo.OldCompanyId).
                                                                                   FirstOrDefault();

                if (userAttendanceSch != null)
                {

                    ViewBag.SelfServiceSchedule = userAttendanceSch;
                    userAttendanceSchDetail = getAttendanceScheduleDetailData(userAttendanceSch.Id, userAttendanceSch.OldCompanyId ?? 0);
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetSelfServiceScheduleDetail");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView("SelfServiceScheduleDetail", userAttendanceSchDetail);
        }
        public ActionResult GetAttendanceSchedule(UserFilterViewModel schFilter)
        {
          
            var attSchList = getAttendanceScheduleList(schFilter);
            ViewBag.SelectedScheduleIds = string.Join(",", attSchList.Select(s => s.Id.ToString()));
            return PartialView("ScheduleList", attSchList);
        }

        public ActionResult GetAttendanceScheduleDetail(int schId, int companyId)
        {

             var attSchDetailData = getAttendanceScheduleDetailData(schId, companyId);

           // return PartialView("");
            return PartialView("ScheduleDetail", attSchDetailData);
        }
        public ActionResult GetUserScheduleDetail(int schId, int companyId)
        {
            var periodHours = timeAideWindowContext.tSchedModPeriodSumms
                   .Where(w => w.ID == schId)
                    .FirstOrDefault().dblPeriodHours;
            ViewBag.PeriodHours = periodHours;
            var attSchDetailData = getAttendanceScheduleDetailData(schId, companyId);

            // return PartialView("");
            return PartialView("UserScheduleDetailPopup", attSchDetailData);
        }
        private void SetFilterDropdownList()
        {
            ViewBag.PositionId = new SelectList(timeAideWebContext.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);

            var loginUser = timeAideWebContext.UserInformation.Include("SupervisorSubDepartment.SubDepartment")
                                                               .Include("SupervisorDepartment.Department")
                                                               .Include("SupervisorEmployeeType.EmployeeType").FirstOrDefault(u => u.Id == SessionHelper.LoginId);

            if (loginUser.SupervisorCompany.Count == 0 || loginUser.SupervisorCompany.Where(c => c.CompanyId == SessionHelper.SelectedCompanyId).Count() == 0)
            {
                ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName").OrderBy(o => o.Text);
                ViewBag.SubDepartmentId = new SelectList(new List<SubDepartment>(), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                ViewBag.EmployeeTypeId = new SelectList(new List<EmployeeType>(), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
            }
            else
            {
                if (loginUser.SupervisorDepartment.Count == 0)
                {
                    ViewBag.DepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
                }
                else
                {
                    ViewBag.DepartmentId = new SelectList(loginUser.SupervisorDepartment.Select(d => d.Department), "Id", "DepartmentName").OrderBy(o => o.Text);
                }
                if (loginUser.SupervisorSubDepartment.Count == 0)
                {
                    ViewBag.SubDepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                }
                else
                {
                    ViewBag.SubDepartmentId = new SelectList(loginUser.SupervisorSubDepartment.Select(s => s.SubDepartment), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                }
                if (loginUser.SupervisorEmployeeType.Count == 0)
                {
                    ViewBag.EmployeeTypeId = new SelectList(timeAideWebContext.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                }
                else
                {
                    ViewBag.EmployeeTypeId = new SelectList(loginUser.SupervisorEmployeeType.Select(d => d.EmployeeType), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                }
            }
        }
        private EmployeeAttSchCalendarViewModel getAttendanceScheduleCalendarData(UserFilterViewModel schFilter)
        {
            EmployeeAttSchCalendarViewModel calenderViewModel = null;

            try
            {
                var scheduleList = getAttendanceScheduleList(schFilter);
                if (scheduleList != null)
                {
                    calenderViewModel = new EmployeeAttSchCalendarViewModel();
                    //calenderViewModel.CalendarStartDate
                    calenderViewModel.CalendarStartDate = scheduleList.Min(m => m.dStartDate)??DateTime.Today;
                    calenderViewModel.CalendarEndDate = scheduleList.Max(m => m.dEndDate) ?? DateTime.Today;
                    var calendarDateRange = new List<DateTime>();
                    var tempDate = calenderViewModel.CalendarStartDate;
                    while(tempDate<= calenderViewModel.CalendarEndDate)
                    {
                        calendarDateRange.Add(tempDate);
                        tempDate = tempDate.AddDays(1);
                    }
                    calenderViewModel.CalendarDateRange = calendarDateRange;
                    var calenderDetail = new List<EmployeeAttSchCalendarDetailViewModel>();
                    foreach(var userSch in scheduleList)
                    {
                        var userCalData = new EmployeeAttSchCalendarDetailViewModel();
                        userCalData.EmployeeSchedule = userSch;
                        userCalData.EmployeeScheduleDetail = getAttendanceScheduleDetailData(userSch.Id, userSch.OldCompanyId??0)
                                                            .OrderBy(o => o.dPunchDate).ToList();
                        calenderDetail.Add(userCalData);
                    }
                    calenderViewModel.CalendarDetail = calenderDetail;

                }

                

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return calenderViewModel;

        }
        private List<EmployeeAttendenceSchedule> getAttendanceScheduleList(UserFilterViewModel schFilter)
        {
            List<EmployeeAttendenceSchedule> retAttendenceScheduleList = null ;
           
            try
            {
                var userList=  getFilterEmployeeList(schFilter);

                var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                    .Where(w => schFilter.ScheduleDate >= w.dStartDate && schFilter.ScheduleDate <= w.dEndDate)
                    .Join(timeAideWindowContext.tusers,
                     usch => usch.nUserID,
                      ur => ur.id,
                      (usch, ur) => new EmployeeAttendenceSchedule
                      {
                          Id = usch.ID,
                          nUserID = usch.nUserID,
                          CompanyId = ur.nCompanyID,
                          sUserName = usch.sUserName,
                          sWeekID = usch.sWeekID,
                          sNote = usch.sNote,
                          dStartDate = usch.dStartDate,
                          dEndDate = usch.dEndDate,
                          dblPeriodHours = usch.dblPeriodHours,
                          nPayPeriodType = usch.nPayPeriodType,
                          nPayWeekNum = usch.nPayWeekNum,
                          nScheduleId = ur.nScheduleID
                      }).ToList();



                var usrSchFinal = userSchedule                                       //Outer Table
                .Join(userList,                                      //Inner Table t join
                     usch => new { userId = usch.nUserID, cmpId = usch.CompanyId },     //Condition from outer table
                     urs => new { userId = urs.EmployeeId, cmpId = urs.OldCompanyId },          //Condition from inner table
                     (usch, urs) => new EmployeeAttendenceSchedule
                     {                                 //Result
                         Id = usch.Id,
                         nUserID = usch.nUserID,
                         OldCompanyId = usch.CompanyId,
                         EmployeeId = urs.EmployeeId,
                         UserInformationId = urs.Id,
                         CompanyId = urs.CompanyId,
                         sUserName = usch.sUserName,
                         sWeekID = usch.sWeekID,
                         sNote = usch.sNote,
                         dStartDate = usch.dStartDate,
                         dEndDate = usch.dEndDate,
                         dblPeriodHours = usch.dblPeriodHours,
                         nPayPeriodType = usch.nPayPeriodType,
                         nPayWeekNum = usch.nPayWeekNum
                     }
                     ).ToList();
                retAttendenceScheduleList = usrSchFinal;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retAttendenceScheduleList;

        }
        private List<EmployeeAttendenceSchDetail> getAttendanceScheduleDetailData(int schId,int olCompanyId)
        {
            List<EmployeeAttendenceSchDetail> retAttendenceScheduleDetail = null;

            try
            {
               // var userList = getFilterEmployeeList(schFilter);

                var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                    .Where(w => w.ID == schId)
                    .Join(timeAideWindowContext.tusers,
                     usch => usch.nUserID,
                      ur => ur.id,
                      (usch, ur) => new EmployeeAttendenceSchedule
                      {
                          Id = usch.ID,
                          nUserID = usch.nUserID,
                          CompanyId = ur.nCompanyID,
                          OldCompanyId = olCompanyId,
                          sUserName = usch.sUserName,
                          sWeekID = usch.sWeekID,
                          sNote = usch.sNote,
                          dStartDate = usch.dStartDate,
                          dEndDate = usch.dEndDate,
                          dblPeriodHours = usch.dblPeriodHours,
                          nPayPeriodType = usch.nPayPeriodType,
                          nPayWeekNum = usch.nPayWeekNum,
                          nScheduleId = ur.nScheduleID
                      }).FirstOrDefault();

                var userScheduleDetail = timeAideWindowContext.tSchedModDailyDetails.Where(w => w.nUserID == userSchedule.nUserID &&
                                                               w.dPunchDate >= userSchedule.dStartDate && w.dPunchDate <= userSchedule.dEndDate)
                                          .OrderBy(o=>o.dPunchDate).AsEnumerable()
                                          .Select(s => new EmployeeAttendenceSchDetail
                                          {
                                              Id = s.ID,
                                              nUserID = s.nUserID,
                                              sUserName = s.sUserName,
                                              dPunchDate =  s.dPunchDate,
                                              dPunchIn1 = s.dPunchIn1,
                                              dPunchOut1 = s.dPunchOut1,
                                              dPunchIn2 = s.dPunchIn2,
                                              dPunchOut2 = s.dPunchOut2,
                                              nWorkDayType= s.nWorkDayType,
                                              sWorkDayTypeName = AttendenceScheduleMasterDDV.WorkDayTypes.Where(w=>w.Id== (s.nWorkDayType??0)).FirstOrDefault().Name,
                                              nPunchNum= s.nPunchNum,
                                              dblDayHours = s.dblDayHours,
                                              nPayWeekNum = s.nPayWeekNum,
                                              nSchedModPeriodSummId = userSchedule.Id,
                                              nScheduleId = userSchedule.nScheduleId,
                                              sNote = s.sNote
                                              
                                          }).ToList();

                retAttendenceScheduleDetail = userScheduleDetail;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retAttendenceScheduleDetail;

        }
        private EmployeeAttendenceSchDetail getAttendanceScheduleDetailEditData(int schId, int schDetailId, int olCompanyId)
        {
            EmployeeAttendenceSchDetail retAttendenceScheduleDetail = null;

            try
            {
                // var userList = getFilterEmployeeList(schFilter);

                var userDetail = timeAideWindowContext.tSchedModDailyDetails
                    .Where(w => w.ID == schDetailId)
                    .Join(timeAideWindowContext.tusers,
                     usch => usch.nUserID,
                      ur => ur.id,
                      (usch, ur) => new EmployeeAttendenceSchedule
                      {
                          Id = usch.ID,
                          nUserID = usch.nUserID,
                          CompanyId = ur.nCompanyID,
                          OldCompanyId = olCompanyId,
                          sUserName = usch.sUserName,
                          sWeekID = usch.sWeekID,
                          sNote = usch.sNote??string.Empty,                          
                          nPayWeekNum = usch.nPayWeekNum,
                          nScheduleId = ur.nScheduleID
                      }).FirstOrDefault();

                var userScheduleDetailEditRecord = timeAideWindowContext.tSchedModDailyDetails.Where(w => w.ID == schDetailId)
                                          .AsEnumerable()
                                          .Select(s => new EmployeeAttendenceSchDetail
                                          {
                                              Id = s.ID,
                                              nUserID = s.nUserID,
                                              sUserName = s.sUserName,
                                              dPunchDate = s.dPunchDate,
                                              dPunchIn1 = s.dPunchIn1,
                                              dPunchOut1 = s.dPunchOut1,
                                              dPunchIn2 = s.dPunchIn2,
                                              dPunchOut2 = s.dPunchOut2,
                                              nWorkDayType = s.nWorkDayType,                                              
                                              nPunchNum = s.nPunchNum,
                                              dblDayHours = s.dblDayHours,
                                              nPayWeekNum = s.nPayWeekNum,
                                              sWeekID     = s.sWeekID,
                                              nScheduleId = userDetail.nScheduleId,
                                              OldCompanyId = olCompanyId,
                                               nSchedModPeriodSummId = schId
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
                var subDepartmentId = 0;
                var employeeTypeId = 0;

                if (schFilter != null)
                {
                    employeeId = schFilter.EmployeeId ?? 0;
                    employeeName = schFilter.EmployeeName == null ? employeeName : schFilter.EmployeeName;
                    positionId = schFilter.PositionId ?? 0;
                    departmentId = schFilter.DepartmentId ?? 0;
                    subDepartmentId= schFilter.SubDepartmentId ?? 0;
                    employeeTypeId = schFilter.EmployeeTypeId ?? 0;

                }
                //Calling Db Procedure
                var userInformationList = timeAideWebContext.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                         0, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).
                                         Where(w => w.DepartmentId == (departmentId == 0 ? w.DepartmentId : departmentId))
                                         .Where(w => w.SubDepartmentId == (subDepartmentId == 0 ? w.SubDepartmentId : subDepartmentId))
                                         .Where(w => w.EmployeeTypeID == (employeeTypeId == 0 ? w.EmployeeTypeID : employeeTypeId));



                retFilterEmployeeList = userInformationList;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retFilterEmployeeList;

        }
        public ActionResult EditAttendenceSchedule(int schId)
        {
            if (schId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                EmployeeAttendenceSchedule model=null;
                AllowEdit();
                var schEntity = timeAideWindowContext.tSchedModPeriodSumms.Find(schId);
                if (schEntity != null)
                {
                    model = new EmployeeAttendenceSchedule()
                    {
                        Id = schEntity.ID,
                        nUserID = schEntity.nUserID,
                        nPayWeekNum = schEntity.nPayWeekNum,
                        dStartDate = schEntity.dStartDate,
                        dEndDate = schEntity.dEndDate,
                        sNote = schEntity.sNote??string.Empty
                    };
                }
               
                return PartialView("EditSchedule", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AttendanceSchedule", "EditAttendenceSchedule");
                return PartialView("Error", handleErrorInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EditAttendenceSchedule(EmployeeAttendenceSchedule model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
                        
            try
            {
                if (model.Id == 0)
                {
                    throw new Exception("Invalid Schedule record.");
                }
               

                    var schedModPeriodSummEntity = timeAideWindowContext.tSchedModPeriodSumms.Find(model.Id);
                    if (schedModPeriodSummEntity != null)
                    {
                        schedModPeriodSummEntity.sNote = model.sNote;
                        schedModPeriodSummEntity.dModifiedDate = DateTime.Now;
                        timeAideWindowContext.SaveChanges();
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
        public ActionResult EditAttendenceScheduleDetail(int schId, int? schDetailId, int? companyId,string openType)
        {

            if (schDetailId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }         

            try
            {
                AllowEdit();
                var scheduleEditData = getAttendanceScheduleDetailEditData(schId, schDetailId ?? 0, companyId ?? 0);
                ViewBag.nWorkDayType = new SelectList(AttendenceScheduleMasterDDV.WorkDayTypes, "Id", "Name", scheduleEditData.nWorkDayType);
                ViewBag.nPunchNum = new SelectList(AttendenceScheduleMasterDDV.PunchNumTypes, "Id", "Name", scheduleEditData.nPunchNum);
                ViewBag.openType = openType;
                return PartialView("EditScheduleDetail", scheduleEditData);
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
        public JsonResult EditAttendenceScheduleDetail(EmployeeAttendenceSchDetail model)
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
                var minAllowPunchDate = punchDate.AddDays(-1);
                var maxAllowPunchDate = punchDate.AddDays(2).AddMinutes(-1);

                var dPunchIn1 = model.dPunchIn1?? punchDate;
                var dPunchOut1 = model.dPunchOut1??punchDate;
                var dPunchIn2 = model.dPunchIn2??punchDate;
                var dPunchOut2 = model.dPunchOut2??punchDate;
                var LoginUser_OldId = timeAideWebContext.UserInformation.Find(SessionHelper.LoginId).Old_Id;
                switch (model.nPunchNum)
                {
                    case 1:
                       if(dPunchIn1< minAllowPunchDate && dPunchIn1> maxAllowPunchDate)
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
                        dayHours = Math.Round(((diffInOut11Mins+ diffInOut22Mins) / 60), 2);
                        break;
                }
                model.dblDayHours = dayHours;
               
                var schedModDailyDetailEntity = timeAideWindowContext.tSchedModDailyDetails.Find(model.Id);
                if (schedModDailyDetailEntity != null)
                {
                    schedModDailyDetailEntity.nWorkDayType = model.nWorkDayType;
                    schedModDailyDetailEntity.nPunchNum = model.nPunchNum;
                    schedModDailyDetailEntity.dPunchIn1 = model.dPunchIn1;
                    schedModDailyDetailEntity.dPunchIn2 = model.dPunchIn2;
                    schedModDailyDetailEntity.dPunchOut1 = model.dPunchOut1;
                    schedModDailyDetailEntity.dPunchOut2 = model.dPunchOut2;
                    schedModDailyDetailEntity.dblDayHours = model.dblDayHours;
                    schedModDailyDetailEntity.sNote = model.sNote ?? string.Empty;
                    schedModDailyDetailEntity.dModifiedDate = DateTime.Now;
                    schedModDailyDetailEntity.nSupervisorID = LoginUser_OldId;
                    timeAideWindowContext.SaveChanges();

                  var totalPeriodHours=  timeAideWindowContext.tSchedModDailyDetails.Where(w => w.nUserID == model.nUserID &&
                                                            w.sWeekID == model.sWeekID && w.nWorkDayType == 1)
                                                            .Select(s => s.dblDayHours ?? 0).Sum();

                    var schedModPeriodSummEntity = timeAideWindowContext.tSchedModPeriodSumms.Find(model.nSchedModPeriodSummId);
                    if (schedModPeriodSummEntity != null)
                    {
                        schedModPeriodSummEntity.dblPeriodHours = totalPeriodHours;
                        schedModPeriodSummEntity.dModifiedDate = DateTime.Now;
                        schedModPeriodSummEntity.nSupervisorID = LoginUser_OldId;
                        timeAideWindowContext.SaveChanges();
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
                var schInfo = timeAideWindowContext.tSchedules
                                .Where(w => w.ID == model.nScheduleId).FirstOrDefault();
                var dayInfo = timeAideWindowContext.tScheduleDayInfoes
                              .Where(w => w.nSchedID == model.nScheduleId && w.nDayofWeek == punchDayOfWk).FirstOrDefault();

                if (dayInfo != null)
                {
                    var schPunchCount = schInfo.nPunchNum;
                    //Punch in/out 1
                    var dIn1 = dayInfo.dIn1Hr ?? punchDate;
                    var dOut1 = dayInfo.dOut1Hr ?? punchDate;
                    var in1DayOffSet = 0;
                    var out1DayOffSet = 0;
                    if(dIn1.ToString("tt")=="PM" && dOut1.ToString("tt")=="AM")
                    {
                        out1DayOffSet = 1;
                    }
                    TimeSpan in1Time = new TimeSpan(in1DayOffSet, dIn1.Hour, dIn1.Minute, dIn1.Second);
                    TimeSpan out1Time = new TimeSpan(out1DayOffSet, dOut1.Hour, dOut1.Minute, dOut1.Second);
                    
                    var diffInOut1Mins = out1Time.Subtract(in1Time).TotalMinutes;                    
                    var in1DateTime = punchDate.Add(in1Time);
                    var out1DateTime = in1DateTime.AddMinutes(Math.Abs(diffInOut1Mins));
                    //Punch in/out 2
                    var dIn2 = dayInfo.dIn2Hr ?? punchDate;
                    var dOut2 = dayInfo.dOut2Hr ?? punchDate;
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
                                dayHours = Math.Round((diffInOut1Mins / 60),2);
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
                                dayHours = Math.Round((diffInOut1Mins / 60)+ (diffInOut2Mins / 60), 2);
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
            return Json(new { status = status, message = message, punchData= model });
        }
        
        public JsonResult GetDashboardMyCurrentScheduleWidget(int? id)
        {
           // var userId = 248;//SessionHelper.LoginId;
            var userId = id?? SessionHelper.LoginId;
            var userInfo = timeAideWebContext.UserInformation.Where(w=>w.Id == userId).Select(s=>new { EmployeeId=s.EmployeeId, OldCompanyId=s.Company.Old_Id}).FirstOrDefault();
            string status = "Success";
            string message = "Successfully fetch!";
            EmployeeAttendenceSchedule widgetData = new EmployeeAttendenceSchedule();
            UserFilterViewModel schFilter = null;
            try
            {
                schFilter = new UserFilterViewModel() { ScheduleDate = DateTime.Today, EmployeeId = userInfo.EmployeeId };
                 var userAttendanceSch = getAttendanceScheduleList(schFilter).Where(w=>w.OldCompanyId== userInfo.OldCompanyId).
                                                                                FirstOrDefault();

                if (userAttendanceSch != null) {
                   var totalDays= getAttendanceScheduleDetailData(userAttendanceSch.Id, userAttendanceSch.OldCompanyId ?? 0).
                                                    Where(w=>w.nWorkDayType==1).Count();

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timeAideWebContext.Dispose();
                if(timeAideWindowContext!=null)
                timeAideWindowContext.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: UserContactInformation/Edit/5



    }
}
