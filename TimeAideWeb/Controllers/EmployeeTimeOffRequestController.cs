
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
using TimeAide.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TimeAide.Web.Controllers
{
    public class EmployeeTimeOffRequestController : BaseTAWindowRoleRightsController<EmployeeTimeOffRequest>
    {
        // GET: EmployeeTimeOffRequest
        public ActionResult Index()
        {
            ViewBag.TimeOffTypeId = new SelectList(getTimeoffTypes(), "AccrualTypeId", "TimeOffTypeName").OrderBy(o=>o.Text);
            ViewBag.TimeOffStatusId = new SelectList(timeAideWebContext.ChangeRequestStatus, "Id", "ChangeRequestStatusName").OrderBy(o => o.Text);
            return PartialView();
        }
      
        public ActionResult IndexBySupervior(EmployeeTimeOffRequest requestFilter)
        {
            UserFilterViewModel empFilter = new UserFilterViewModel() { EmployeeId = requestFilter.EmployeeId,
                                                                        EmployeeName = requestFilter.AccrualType };
            var empList = getEmployeeList(empFilter);
            requestFilter.StartDate = requestFilter.StartDate ?? DateTime.Today;
            var employeeTimeoffList = timeAideWebContext.EmployeeTimeOffRequest
                                                        .Where( 
                                                                w => w.DataEntryStatus==1 && empList.Contains(w.UserInformationId) &&
                                                               (requestFilter.TransType=="All"? true: w.TransType== requestFilter.TransType) &&
                                                               (requestFilter.ChangeRequestStatusId==0? true: requestFilter.ChangeRequestStatusId == w.ChangeRequestStatusId)&&
                                                               (w.StartDate>= requestFilter.StartDate) &&
                                                               (requestFilter.EndDate==null?true: w.StartDate <= requestFilter.EndDate)
                                                              ).OrderBy(o => o.UserInformationId).OrderByDescending(d=>d.StartDate);
            foreach(var timeoffReq in employeeTimeoffList)
            {
                if (timeoffReq.ChangeRequestStatusId == 1)
                {
                    timeoffReq.CanTakeAction = CanTakeAction(timeoffReq.Id);
                }
            }
            return PartialView(employeeTimeoffList);
        }
        public ActionResult IndexByEmployee(int? id)
        {
            return GetIndexByEmployee(id);
        }

        private ActionResult GetIndexByEmployee(int? id)
        {
            try
            {
                // AllowView();
                var userId = id ?? 0;
                ViewBag.UserId = userId;
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TimeOffRequest", "IndexByEmployee");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            return PartialView("IndexByEmployee");
        }

        public ActionResult IndexSelfService(string id)
        {
            var userId = id == null ? SessionHelper.LoginId : int.Parse(Encryption.DecryptURLParm(id));
            return GetIndexByEmployee(userId);
        }
        public ActionResult GetEmployeeTimeOffRequestList(int? id)
        {
            var userId = id ?? 0;            
            var employeeTimeoffList = timeAideWebContext.EmployeeTimeOffRequest
                                                        .Where(w=>w.UserInformationId==userId &&w.DataEntryStatus==1)
                                                        .OrderByDescending(o=>o.StartDate);

            return PartialView("EmployeeTimeOffRequestList",employeeTimeoffList);
        }
        public ActionResult CreateTimeOff(int? id)
        {
            var userId = id ?? SessionHelper.LoginId;
            var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId)
                                              .Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id, CompanyLogo = s.Company.CompanyLogo })
                                              .FirstOrDefault();


            //var userIdParameter = new SqlParameter("@UserID", userInfo.EmployeeId);        
            //var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
            //decimal dayHours = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnSS_GetHoursInDay(@UserID,@SearchDate)", userIdParameter, searchDateParameter).FirstOrDefault();
            decimal dayHours = getDayHours(userInfo.EmployeeId??0);

            // timeOffType.Balance = Math.Round(balance, 2);
            //var isValid= DataHelper.fnComp_CheckTimeOffValidation(timeAideWindowContext,userInfo.EmployeeId??0, DateTime.Today);


            ViewBag.timeOffRequestStartDD = "+1d";
            ViewBag.timeOffRequestEndDD = "+60d";
            ViewBag.userId = userId;
            ViewBag.dayHours = dayHours;
          //  var appConfigStartDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "TimeOffRequestStartDays")
          //                                                                         .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();
            var appConfigStartDD = ApplicationConfigurationService.GetApplicationConfiguration("TimeOffRequestStartDays").ApplicationConfigurationValue;

            //var appConfigEndDD = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "TimeOffRequestEndDays")
            //                                                                       .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();

            var appConfigEndDD = ApplicationConfigurationService.GetApplicationConfiguration("TimeOffRequestEndDays").ApplicationConfigurationValue;
            int intStartDD;
            int intEndDD;

            if (int.TryParse(appConfigStartDD, out intStartDD))
            {

                ViewBag.timeOffRequestStartDD = string.Format("+{0}d", appConfigStartDD);
            }
            if (int.TryParse(appConfigEndDD, out intEndDD))
            {
                ViewBag.timeOffRequestEndDD = string.Format("+{0}d", appConfigEndDD);
            }
            var initDate = DateTime.Today.AddDays(intStartDD);
            ViewBag.timeOffRequestStartLimit = initDate;
            ViewBag.timeOffRequestEndLimit = DateTime.Today.AddDays(intEndDD);
            var initFromDate = initDate<DateTime.Today? DateTime.Today: initDate;
            


            var model = new EmployeeTimeOffRequest { UserInformationId = userId, EmployeeId = userInfo.EmployeeId, StartDate= initFromDate, EndDate= initFromDate };
            ViewBag.TimeOffTypeId = new SelectList(getTimeoffTypes(), "AccrualTypeId", "TimeOffTypeName" ).OrderBy(o => o.Text);
            return PartialView(model);
        }
        public ActionResult ViewTimeOffCalendar(EmployeeTimeOffRequest model)
        {
            var calendarViewModel = new TimeOffCalendarViewModel();
            if(model.StartDate!=null && model.EndDate != null)
            {
                var calendarStartOfMMDate = new DateTime(model.StartDate.Value.Year, model.StartDate.Value.Month, 1);
                var calendarEndOfMMDate = new DateTime(model.EndDate.Value.Year, model.EndDate.Value.Month, 1);
                var calendarMonths = new List<string>();
                var baseCalendarDate = calendarStartOfMMDate;
                do
                {
                    calendarMonths.Add(baseCalendarDate.ToString("yyyy_M"));
                    baseCalendarDate = baseCalendarDate.AddMonths(1);
                }
                while (baseCalendarDate <= calendarEndOfMMDate);
                calendarViewModel.CalendarStartDate = model.StartDate.Value;
                calendarViewModel.CalendarEndDate = model.EndDate.Value;               
                calendarViewModel.CalenderMonths = string.Join(",",calendarMonths.ToArray());
            }
            return PartialView(calendarViewModel);
        }
        public ActionResult AjaxRenderCalendarMonth(TimeOffRenderMonthViewModel model)
        {
            int year = model.RenderYear;
            var month = model.RenderMonth;
            var totalCalMonthDays = 42;
            var startDate = new DateTime(year,month,1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var daysofCurrMonth = DateTime.DaysInMonth(year, month);
            var prevMonthDays = (int)startDate.DayOfWeek;
            var prevMonthStartDate = startDate.AddDays(prevMonthDays*(-1));
            var nextMonthEndDate = prevMonthStartDate.AddDays(totalCalMonthDays);
            var timeOffcalendarMonthModel = new TimeOffCalendarMonthViewModel();
            timeOffcalendarMonthModel.StartDate = prevMonthStartDate;
            timeOffcalendarMonthModel.EndDate = nextMonthEndDate;
            timeOffcalendarMonthModel.CalendarMonth = month;
            timeOffcalendarMonthModel.DisplayName = startDate.ToString("MMMM yyyy");
            timeOffcalendarMonthModel.CalendarMonthDays = new List<TimeOffCalendarDayViewModel>();
            
            var scheduleHolidays= DataHelper.spSS_UserScheduleBasedHolidays<UserScheduledHolidayViewModel>(timeAideWindowContext, model.EmployeeId, prevMonthStartDate, nextMonthEndDate);
            var dailyBalances = ValidateTimeOffForDailyBalances(new EmployeeTimeOffRequest() { EmployeeId = model.EmployeeId,TransType=model.TransType,AccrualType=model.AccrualType, StartDate=model.TimeOffStartDate,EndDate=model.TimeOffEndDate,DayHours=model.TimeOffDayHours });
            for (int i=0;i< totalCalMonthDays; i++)
            {
                var calendarDay= new TimeOffCalendarDayViewModel();
                var dayDateVal = timeOffcalendarMonthModel.StartDate.AddDays(i);
                calendarDay.DisplayName = dayDateVal.ToString("dd");
                calendarDay.DayDate = dayDateVal;
                calendarDay.IsHoliday = scheduleHolidays.Where(w=>w.Holiday== dayDateVal).Count() > 0;
                var dailyBalance= dailyBalances.Where(w=>w.BalanceDate== dayDateVal).FirstOrDefault();
                if (i == 0)
                {
                    calendarDay.DisplayName = dayDateVal.ToString("MMM dd");
                }
                else if(dayDateVal.AddDays(-1).Month != dayDateVal.Month)
                {

                    calendarDay.DisplayName = dayDateVal.ToString("MMM dd");
                }
                if(dayDateVal>=model.TimeOffStartDate && dayDateVal<= model.TimeOffEndDate)
                {
                    calendarDay.IsSelected = true;
                }
                if (dailyBalance != null && model.AccrualType!="NO")
                {
                    var accType = dailyBalance.AccrualType;
                    var availBal = Math.Round(dailyBalance.Available,2);
                    var status = availBal < 0 ? "Error" : "Success";
                    var openingBal= Math.Round(dailyBalance.OpeningBalance, 2);
                    var openingBalSts = openingBal < 0 ? "Error" : "Success";
                    calendarDay.Event1 = $"{accType},{availBal},{status}";
                    calendarDay.Event2 = $"{accType},{openingBal},{openingBalSts}";
                }
                calendarDay.IsCurrentMonthDay = (dayDateVal.Month == month);
                calendarDay.IsToday = (dayDateVal == DateTime.Today);
                timeOffcalendarMonthModel.CalendarMonthDays.Add(calendarDay);
            }
            ViewBag.WorkingDays = getWorkDays(model.EmployeeId, model.TimeOffStartDate, model.TimeOffEndDate);
             var retVal= CheckIsValidTimeOffDailyBalance(dailyBalances);
            ViewBag.RenderStatus = retVal.Status;
            ViewBag.RenderMessage = retVal.Message;
            return PartialView("RenderTimeOffCalendarMonth", timeOffcalendarMonthModel);
        }

        private dynamic CheckIsValidTimeOffDailyBalance(IList<UserDailyBalanceViewModel> dailyBalances)
        {
            var status = "Success";
            var message="";

            var invalidBalance = dailyBalances.Where(w=>w.Available<0).FirstOrDefault();
            if (invalidBalance != null)
            {
                status = "Error";
                message = $"There is insufficient available balance({Math.Round(invalidBalance.Available,2)}) for time-off day({invalidBalance.BalanceDate.ToString("MMM dd")})";
            }
            return new {Status=status,Message=message };
        }
        private IList<UserDailyBalanceViewModel> ValidateTimeOffForDailyBalances(EmployeeTimeOffRequest model)
        {
            var userDailyBalances = new List<UserDailyBalanceViewModel>();
            if (model.AccrualType != "SIF")
            {
                userDailyBalances = DataHelper.spSS_CompensationDailyBalancesWeb<UserDailyBalanceViewModel>(timeAideWindowContext, model.EmployeeId.Value, model.AccrualType, model.StartDate.Value, model.EndDate.Value);
            }
            else
            {
                userDailyBalances = DataHelper.spSS_SickInFamilyDailyBalancesWeb<UserDailyBalanceViewModel>(timeAideWindowContext, model.EmployeeId.Value, model.TransType, model.StartDate.Value, model.EndDate.Value);
            }
            decimal initBalance = 0;
            int counter = 0;
            int currMonth = 0;
            //foreach (var dailyBalance in userDailyBalances)
            //{
            //    if (counter == 0)
            //    {
            //        initBalance = dailyBalance.Balance;
            //        currMonth = dailyBalance.BalanceDate.Month;
            //    }
            //    else
            //    {
            //        currMonth = userDailyBalances[counter - 1].BalanceDate.Month;
            //    }
            //    if(currMonth!= dailyBalance.BalanceDate.Month)
            //    {
            //        initBalance = dailyBalance.Balance;
            //        currMonth = dailyBalance.BalanceDate.Month;
            //    }
            //    initBalance -= model.DayHours.Value;
            //    dailyBalance.Available = initBalance;
            //    counter++;
            //}
            foreach (var dailyBalance in userDailyBalances)
            {
                if (model.AccrualType == "NO")
                {
                    dailyBalance.OpeningBalance = 0;
                    dailyBalance.Available = 0;
                    continue;
                }
                dailyBalance.OpeningBalance = dailyBalance.Balance - (model.DayHours.Value * counter);
                dailyBalance.Available = dailyBalance.Balance - (model.DayHours.Value*(counter+1));
                counter++;
            }
            return userDailyBalances;
        }
        public JsonResult SaveTimeOffRequest(EmployeeTimeOffRequest model)
        {
            string status = "Success";
            string message = "Time-Off request is initiated successfully!";
            var requestId = 0;
            using (var requestSavingDBTrans = timeAideWebContext.Database.BeginTransaction())
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    decimal dayHours = getDayHours(model.EmployeeId ?? 0);

                    if (model.WorkingDays <= 1)
                    {
                        var isOverlapped = timeAideWebContext.EmployeeTimeOffRequest
                                           .Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.ChangeRequestStatusId <= 2
                                           && ((w.StartDate >= model.StartDate && w.StartDate <= model.EndDate) || (w.EndDate >= model.StartDate && w.EndDate <= model.EndDate)|| (w.StartDate <= model.StartDate && w.EndDate >= model.EndDate))
                                           && (w.WorkingDays>1)
                                           ).Count();
                        if (isOverlapped > 0)
                        {
                            throw new Exception("Time-Off date range is being overlapped with already requested time.");
                        }
                        else
                        {
                            var isTimeOffTransOverlapped = timeAideWebContext.EmployeeTimeOffRequest
                                            .Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.ChangeRequestStatusId <= 2
                                            && ((w.StartDate >= model.StartDate && w.StartDate <= model.EndDate) || (w.EndDate >= model.StartDate && w.EndDate <= model.EndDate) || (w.StartDate <= model.StartDate && w.EndDate >= model.EndDate))
                                            && (w.WorkingDays <= 1) && w.TransType==model.TransType)
                                            .Count();
                            if (isTimeOffTransOverlapped > 0)
                            {
                                throw new Exception($"Time-Off type ({model.TransType}) is already requested.");
                            }
                            var alreadyRequestedTimeoffhrsForDay = timeAideWebContext.EmployeeTimeOffRequest
                                                              .Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.ChangeRequestStatusId <= 2
                                                              && ((w.StartDate >= model.StartDate && w.StartDate <= model.EndDate) || (w.EndDate >= model.StartDate && w.EndDate <= model.EndDate) || (w.StartDate <= model.StartDate && w.EndDate >= model.EndDate))
                                                              && (w.WorkingDays <= 1)
                                                              ).Select(s=>s.DayHours).Sum();

                            var totalAppliedHrsADay = (alreadyRequestedTimeoffhrsForDay??0) + model.DayHours;
                            
                            if(totalAppliedHrsADay> dayHours)
                            {
                                var msg = $"Time-off ({model.StartDate.Value.ToString("MM/dd/yyyy")}-{model.EndDate.Value.ToString("MM/dd/yyyy")}), already requested({alreadyRequestedTimeoffhrsForDay}hrs) + currently requested({model.DayHours}hrs) should not be exceeded the total day hours({dayHours}hrs).";
                                throw new Exception(msg);
                            }
                        
                        }   
                    }
                    else
                    {
                        var isOverlapped = timeAideWebContext.EmployeeTimeOffRequest
                                            .Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.ChangeRequestStatusId <= 2
                                            && ((w.StartDate >= model.StartDate && w.StartDate <= model.EndDate) || (w.EndDate >= model.StartDate && w.EndDate <= model.EndDate) || (w.StartDate <= model.StartDate && w.EndDate >= model.EndDate)))
                                            .Count();
                        if (isOverlapped > 0)
                        {
                            throw new Exception("Time-Off date range is being overlapped with already requested time.");
                        }
                    }
                    //throw new Exception("testing");
                    var isValid = DataHelper.fnComp_CheckTimeOffValidation(timeAideWindowContext, model.EmployeeId ?? 0, model.StartDate.Value);
                    if (isValid == false)
                    {
                        throw new Exception("Payroll period is closed for requested time-off dates.");
                    }
                    var retVal = CheckIsValidTimeOffDailyBalance(ValidateTimeOffForDailyBalances(new EmployeeTimeOffRequest() { EmployeeId = model.EmployeeId, TransType=model.TransType ,AccrualType = model.AccrualType, StartDate = model.StartDate, EndDate = model.EndDate, DayHours = model.DayHours }));
                    if (retVal.Status == "Error")
                    {
                        throw new Exception(retVal.Message);
                    }
                    var workflowErrList = WorkflowService.CanWorkflowIntiated(model.UserInformationId, (int)WorkflowService.ChangeRequestTriggerType.EMP_TIMEOFF_REQ);
                    if (workflowErrList.Count == 0)
                    {
                        model.ChangeRequestStatusId = 1; //in-progress status
                        timeAideWebContext.EmployeeTimeOffRequest.Add(model);
                        timeAideWebContext.SaveChanges();
                        ProcessRequestWorkflow(model, "Start");
                        requestSavingDBTrans.Commit();
                        requestSavingTimeAideWindowDBTrans.Commit();
                    }
                    else
                    {

                        status = "Error";
                        message = string.Join(";", workflowErrList.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    requestSavingDBTrans.Rollback();
                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message, nWeekId= model.Id });
        }
        public ActionResult GetTimeOffRequestStatusHistory(int? id)
        {
            var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
            ViewBag.WorkflowTriggerRequestDetail = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.OrderByDescending(o=>o.Id).ToList();
            return PartialView("TimeOffRequestStatusHistory");
        }
        public JsonResult AjaxGetTimeOffDetail(CompensationBalance timeOffType)
        {
            string status = "Success";
            string message = "Successfully fetch!";
           // CompensationBalance timeOffTypeData = null;

            try
            {
                // timeAideWindowContext.
                var userIdParameter = new SqlParameter("@UserID", timeOffType.EmployeeId);
                var transTypeParameter = new SqlParameter("@TransType", timeOffType.TransType);
                var accrualTypeParameter = new SqlParameter("@AccrualType", timeOffType.AccrualType);
                var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
                decimal balance = 0;
                if (timeOffType.AccrualType == "SIF")
                {
                    balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_SickInFamilyBalance(@UserID,@TransType,@SearchDate)", userIdParameter, transTypeParameter, searchDateParameter).FirstOrDefault();
                }
                else
                {
                    balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                }
                    timeOffType.Balance = Math.Round(balance, 2);
                //punchData = model;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, timeOffTypeData = timeOffType });
        }

        public JsonResult AjaxGetTimeOffCalculation(EmployeeTimeOffRequest model)
        {
            string status = "Success";
            string message = "Successfully fetch!";
            // CompensationBalance timeOffTypeData = null;

            try
            {
                if (model.AccrualType != "")
                {
                    // timeAideWindowContext.
                    var userIdParameter = new SqlParameter("@UserID", model.EmployeeId);
                    var transTypeParameter = new SqlParameter("@TransType", model.TransType);
                    var accrualTypeParameter = new SqlParameter("@AccrualType", model.AccrualType);
                    var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
                    // var startDateParameter = new SqlParameter("@StartDate", model.StartDate);
                    //  var endDateParameter = new SqlParameter("@EndDate", model.EndDate);
                    decimal balance = 0;
                    if (model.AccrualType == "SIF")
                    {
                        balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_SickInFamilyBalance(@UserID,@TransType,@SearchDate)", userIdParameter, transTypeParameter, searchDateParameter).FirstOrDefault();
                    }
                    else
                    {
                        balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                    }
                        model.Balance = Math.Round(balance, 2);
                    int workingDays = getWorkDays(model.EmployeeId, model.StartDate, model.EndDate);
                    //int workingDays = timeAideWindowContext.Database.SqlQuery<int>("SELECT dbo.fnSS_GetScheduledDayCount(@USERID,@StartDate,@EndDate)", userId1Parameter, startDateParameter, endDateParameter).FirstOrDefault();
                    model.WorkingDays = workingDays;
                    //model.RequestedTime = model.DayHours * model.WorkingDays;
                    if (model.AccrualType != "NO")
                    {
                        model.RemainingTime = model.Balance - model.RequestedTime;
                    }
                }
                //punchData = model;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, timeOffCalcData = model });
        }
        private decimal getDayHours(int employeeId)
        {
            var userIdParameter = new SqlParameter("@UserID", employeeId);
            var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
            decimal dayHours = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnSS_GetHoursInDay(@UserID,@SearchDate)", userIdParameter, searchDateParameter).FirstOrDefault();
            return dayHours;
        }
        private int getWorkDays(int? employeeId,DateTime? startDate, DateTime? endDate)
        {
            var userIdParameter = new SqlParameter("@UserID", employeeId);
            var startDateParameter = new SqlParameter("@StartDate", startDate);
            var endDateParameter = new SqlParameter("@EndDate", endDate);

            int workingDays = timeAideWindowContext.Database.SqlQuery<int>("SELECT dbo.fnSS_GetScheduledDayCount(@USERID,@StartDate,@EndDate)", userIdParameter, startDateParameter, endDateParameter).FirstOrDefault();
            
            return workingDays;
        }
        private List<dynamic> getTimeoffTypes()
        {
         var timeoffTypeList=   timeAideWindowContext.tTransDef
                .Join(timeAideWindowContext.tblSS_TransdefTimeOffRequest,
                o => o.Name,
                i => i.strTransName,
                (o, i) => new
                {
                    TimeOffTypeName = o.Name,
                    AccrualTypeId = o.boolUseSickInFamily?"SIF": o.sAccrualImportName,
                    IsTimeOffRequest = i.intTimeOffRequest
                })
                .Where(w => w.IsTimeOffRequest == -1)
                .OrderBy(o=>o.TimeOffTypeName).ToList<dynamic>();
            return timeoffTypeList;
        }
        private int[] getEmployeeList(UserFilterViewModel empFilter)
        {
            int[] userInformationList = null;
            try
            {
                var employeeId = empFilter.EmployeeId??0;
                var employeeName = empFilter.EmployeeName==null?"": empFilter.EmployeeName;
                var positionId = 0;
                var employeeStatusId = 0;
                var selectedSuperviorId = SessionHelper.LoginId;
                //Calling Db Procedure
                userInformationList = timeAideWebContext.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                        employeeStatusId, selectedSuperviorId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId)
                                        .Select(s => s.Id).ToArray();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userInformationList;
        }
        private bool CanTakeAction(int id)
        {
            var isCantakeAction = false;
            var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
            //var WorkflowTriggerRequestDetail = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
            try
            {
                WorkflowTriggerRequestDetail lastWorkflowLevel = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.Where(w => w.WorkflowActionTypeId == 1).OrderByDescending(r => r.Id).FirstOrDefault();
                if (lastWorkflowLevel != null)
                {
                    var ApprovedlevelCount = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.Where(w => w.WorkflowActionTypeId == 2).Count();
                    var currentLevel = ApprovedlevelCount + 1;
                    ViewBag.WorkflowLevel = currentLevel;
                    //ViewBag.WorkflowLevel = lastWorkflowLevel.WorkflowLevel.WorkflowLevelType.Id.ToString();
                }
                else
                {
                    ViewBag.WorkflowLevel = "Closed";
                }

                WorkflowTriggerRequestDetail lastStep = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
                if (lastStep != null)
                {
                    var actionStatusId = lastStep.WorkflowActionTypeId;
                    List<UserInformationViewModel> approveByList = new List<UserInformationViewModel>();
                    if (lastStep.WorkflowLevel.WorkflowLevelTypeId == 1)
                    {//supervisor level
                       var approveBySupervisorList = timeAideWebContext.SP_GetEmployeeSupervisors<UserInformationViewModel>(timeOffRequest.UserInformationId, SessionHelper.SelectedClientId);
                        approveByList.AddRange(approveBySupervisorList);
                    }
                    if (lastStep.WorkflowLevel.WorkflowLevelGroup.Count > 0)
                    {
                       var  approveByGroupList = timeAideWebContext.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(lastStep.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                        approveByList.AddRange(approveByGroupList);
                    }

                    if (approveByList.Any(u => u.Id == SessionHelper.LoginId) && actionStatusId == 1)
                    {
                        isCantakeAction = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isCantakeAction;
        }
        public ActionResult CancelTimeOffRequest(int id)
        {
            try
            {
                //AllowEdit();
               
                var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
                               
                timeOffRequest.ChangeRequestRemarks = "";
                return PartialView(timeOffRequest);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult CancelTimeOffRequest(EmployeeTimeOffRequest model)
        {
            string status = "Success";
            string message = "Time-Off request is cancelled successfully";
            using (var requestApprovalDBTrans = timeAideWebContext.Database.BeginTransaction())
            {
                try
                {
                    //AllowAdd();
                    //db.Entry(ChangeRequestAddress).State = EntityState.Modified;
                    EmployeeTimeOffRequest timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(i => i.Id == model.Id);
                    if(timeOffRequest.ChangeRequestStatusId!=1) //should be in progress
                    {
                        throw new Exception("Request status has been changed during proceess");
                    }
                    timeOffRequest.ChangeRequestStatusId = model.ChangeRequestStatusId;
                    timeOffRequest.ChangeRequestRemarks = model.ChangeRequestRemarks;
                    WorkflowTriggerRequest workflowTriggerRequest = timeAideWebContext.WorkflowTriggerRequest.FirstOrDefault(t => t.EmployeeTimeOffRequestId == timeOffRequest.Id);
                    WorkflowService.ProcessRequestCancellation(timeAideWebContext, workflowTriggerRequest, timeOffRequest);
                   // ProcessRequestWorkflow(timeOffRequest, "NextLevel");

                   requestApprovalDBTrans.Commit();
                }

                catch (AuthorizationException ex)
                {
                    requestApprovalDBTrans.Rollback();
                    status = "Error";
                    message = ex.ErrorMessage;
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    requestApprovalDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }

            }
            return Json(new { status = status, message = message });
        }
        public ActionResult ApproveTimeOffRequest(int id)
        {
            try
            {
                if(!(ViewBag.AllowView || ViewBag.AllowEdit))
                    AllowEdit();

               // ViewBag.CanTakeAction = false;
                var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
                //ViewBag.WorkflowTriggerRequestDetail = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                //WorkflowTriggerRequestDetail lastStep = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.Where(w=>w.WorkflowActionTypeId==1).OrderByDescending(r => r.Id).FirstOrDefault();
                //if (lastStep != null)
                //{
                //   ViewBag.WorkflowLevel= lastStep.WorkflowLevel.WorkflowLevelType.Id.ToString();
                //}
                //else
                //{
                //    ViewBag.WorkflowLevel = "Closed";
                //}
                //var actionStatusId = lastStep.WorkflowActionTypeId;
                //List<UserInformationViewModel> approveByList = new List<UserInformationViewModel>();
                //if (lastStep.WorkflowLevel.WorkflowLevelTypeId == 1)
                //{//supervisor level
                //    approveByList = timeAideWebContext.SP_GetEmployeeSupervisors<UserInformationViewModel>(timeOffRequest.UserInformationId, SessionHelper.SelectedClientId);
                //}
                //else
                //{
                //    approveByList = timeAideWebContext.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(lastStep.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                //}

                //if (approveByList.Any(u => u.Id == SessionHelper.LoginId) && actionStatusId==1)
                if (CanTakeAction(id))
                {
                   // if (CheckPendingMandatoryDocument(id))
                   // {
                   ////     timeOffRequest.CanRequiredDocument = true;
                   // }
                   // else
                   // {
                        //ViewBag.CanTakeAction = true;
                        timeOffRequest.CanTakeAction = true;
                        var userIdParameter = new SqlParameter("@UserID", timeOffRequest.UserInformation.EmployeeId);
                        var accrualTypeParameter = new SqlParameter("@AccrualType", timeOffRequest.AccrualType);
                        var transTypeParameter = new SqlParameter("@TransType", timeOffRequest.TransType);
                        var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
                    //decimal balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                    decimal balance = 0;
                    if (timeOffRequest.AccrualType == "SIF")
                    {
                        balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_SickInFamilyBalance(@UserID,@TransType,@SearchDate)", userIdParameter, transTypeParameter, searchDateParameter).FirstOrDefault();
                    }
                    else
                    {
                        balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                    }
                    timeOffRequest.Balance = Math.Round(balance, 2);
                        if (timeOffRequest.AccrualType != "NO")
                        {
                            timeOffRequest.RemainingTime = timeOffRequest.Balance - timeOffRequest.RequestedTime;
                        }
                    //}
                }
                
                timeOffRequest.ChangeRequestRemarks = "";
                return PartialView(timeOffRequest);
            }
            catch (AuthorizationException ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult ViewTimeOffRequest(int id)
        {
            try
            {
                if (!ViewBag.AllowView)
                    AllowView();
                var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
                TimeAideContext db = new TimeAideContext();
                int workflowTriggerRequestId = timeOffRequest.WorkflowTriggerRequest.OrderByDescending(c=>c.Id).FirstOrDefault().Id;
                if (!db.NotificationLogMessageReadBy.Any(n => n.WorkflowTriggerRequestId == workflowTriggerRequestId && n.ReadById == SessionHelper.LoginId))
                {
                    NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                    notificationLogMessageReadBy.WorkflowTriggerRequestId = workflowTriggerRequestId;
                    notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                    db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                    db.SaveChanges();
                    ViewBag.MarkAsRead = true;
                }
                //AllowEdit();
                // ViewBag.CanTakeAction = false;
                //var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
                //ViewBag.WorkflowTriggerRequestDetail = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                //WorkflowTriggerRequestDetail lastStep = timeOffRequest.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.OrderByDescending(r => r.Id).FirstOrDefault();
                //var actionStatusId = lastStep.WorkflowActionTypeId;
                //List<UserInformationViewModel> approveByList = new List<UserInformationViewModel>();
                //if (lastStep.WorkflowLevel.WorkflowLevelTypeId == 1)
                //{//supervisor level
                //    approveByList = timeAideWebContext.SP_GetEmployeeSupervisors<UserInformationViewModel>(timeOffRequest.UserInformationId, SessionHelper.SelectedClientId);
                //}
                //else
                //{
                //    approveByList = timeAideWebContext.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(lastStep.WorkflowLevel.Id, SessionHelper.SelectedClientId);
                //}

                //if (approveByList.Any(u => u.Id == SessionHelper.LoginId) && actionStatusId==1)
                if (CanTakeAction(id))
                {
                    if (CheckPendingMandatoryDocument(id))
                    {
                       // timeOffRequest.CanRequiredDocument = true;
                    }
                    else
                    {
                        //ViewBag.CanTakeAction = true;
                        timeOffRequest.CanTakeAction = false;
                        var userIdParameter = new SqlParameter("@UserID", timeOffRequest.UserInformation.EmployeeId);
                        var accrualTypeParameter = new SqlParameter("@AccrualType", timeOffRequest.AccrualType);
                        var transTypeParameter = new SqlParameter("@TransType", timeOffRequest.TransType);
                        var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
                        // decimal balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                        decimal balance = 0;
                        if (timeOffRequest.AccrualType == "SIF")
                        {
                            balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_SickInFamilyBalance(@UserID,@TransType,@SearchDate)", userIdParameter, transTypeParameter, searchDateParameter).FirstOrDefault();
                        }
                        else
                        {
                            balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                        }
                        timeOffRequest.Balance = Math.Round(balance, 2);
                        if (timeOffRequest.AccrualType != "NO")
                        {
                            timeOffRequest.RemainingTime = timeOffRequest.Balance - timeOffRequest.RequestedTime;
                        }
                    }
                }

                timeOffRequest.ChangeRequestRemarks = "";
                return PartialView(timeOffRequest);
            }
            catch (AuthorizationException ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequest", "ApproveTimeOffRequest");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult ApproveTimeOffRequestBySupervior(int id)
        {
             try
                {
                    AllowEdit();                    
                    var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(c => c.Id == id);
                   if (CanTakeAction(id))
                    {
                        if (CheckPendingMandatoryDocument(id))
                        {
                            timeOffRequest.CanRequiredDocument = true;
                        }
                        else
                        {
                        //ViewBag.CanTakeAction = true;
                        timeOffRequest.CanTakeAction = true;
                        var userIdParameter = new SqlParameter("@UserID", timeOffRequest.UserInformation.EmployeeId);
                        var accrualTypeParameter = new SqlParameter("@AccrualType", timeOffRequest.AccrualType);
                        var transTypeParameter = new SqlParameter("@TransType", timeOffRequest.TransType);
                        var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
                        //decimal balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                        decimal balance = 0;
                        if (timeOffRequest.AccrualType == "SIF")
                        {
                            balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_SickInFamilyBalance(@UserID,@TransType,@SearchDate)", userIdParameter, transTypeParameter, searchDateParameter).FirstOrDefault();
                        }
                        else
                        {
                            balance = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnComp_RemainingBalance(@UserID,@AccrualType,@SearchDate)", userIdParameter, accrualTypeParameter, searchDateParameter).FirstOrDefault();
                        }
                        timeOffRequest.Balance = Math.Round(balance, 2);
                        if (timeOffRequest.AccrualType != "NO")
                        {
                            timeOffRequest.RemainingTime = timeOffRequest.Balance - timeOffRequest.RequestedTime;
                        }
                    }
                    }

                    timeOffRequest.ChangeRequestRemarks = "";
                    return PartialView("TimeOffRequestApprovalPopup", timeOffRequest);
                }
                catch (AuthorizationException ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    Exception exception = new Exception(ex.ErrorMessage);
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "TimeOffRequest", "ApproveTimeOffRequest");
                    return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }
                catch (Exception ex)
                {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeOffRequest", "ApproveTimeOffRequest");
                    return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                }
        }
        [HttpPost]
        public JsonResult ApproveTimeOffRequest(EmployeeTimeOffRequest model)
        {
            string status = "Success";
            string message = "Action is taken successfully!";
            Boolean isValidForApproval = true;
            using (var requestApprovalDBTrans = timeAideWebContext.Database.BeginTransaction())
            using (var requestApprovalTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    //AllowAdd();
                    //db.Entry(ChangeRequestAddress).State = EntityState.Modified;

                    EmployeeTimeOffRequest timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.FirstOrDefault(i => i.Id == model.Id);

                    var isValid = DataHelper.fnComp_CheckTimeOffValidation(timeAideWindowContext, timeOffRequest.UserInformation.EmployeeId ?? 0, timeOffRequest.StartDate.Value);
                    if (isValid == false && model.ChangeRequestStatusId == 2)
                    {
                        isValidForApproval = false;
                        throw new Exception("Payroll period is closed for requested time-off dates.");

                    }
                    if (CheckPendingMandatoryDocument(model.Id))
                    {
                       // isValidForApproval = false;
                        throw new Exception("Time-Off mandatory/conditional mandatory document(s) are not yet submitted. Time-off request can't be approved.");
                      
                    }
                    timeOffRequest.ChangeRequestStatusId = model.ChangeRequestStatusId;
                    timeOffRequest.ChangeRequestRemarks = model.ChangeRequestRemarks;
                    ProcessRequestWorkflow(timeOffRequest, "NextLevel");
                    //WorkflowTriggerRequest workflowTriggerRequest = timeAideWebContext.WorkflowTriggerRequest.FirstOrDefault(t => t.EmployeeTimeOffRequestId == timeOffRequest.Id);
                    //WorkflowService.GetNextWorkflowLevel<EmployeeTimeOffRequest>(timeAideWebContext, workflowTriggerRequest, timeOffRequest);
                    requestApprovalDBTrans.Commit();
                    requestApprovalTimeAideWindowDBTrans.Commit();
                }

                catch (AuthorizationException ex)
                {
                    requestApprovalDBTrans.Rollback();
                    requestApprovalTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.ErrorMessage;
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    requestApprovalDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }

            }
            return Json(new { status = status, message = message, canApprove= isValidForApproval });
        }

        private bool CheckPendingMandatoryDocument(int id)
        {
            var isPendingRequiredDocument = false;
            var showTimeOffDoc=ApplicationConfigurationService.GetConfigurationStatus("ShowTimeOffDocument");
            if (!showTimeOffDoc) return isPendingRequiredDocument;
            var timeOffRequest = timeAideWebContext.EmployeeTimeOffRequest.Find(id);
            var pendingRequiredCnt= timeAideWebContext.EmployeeTimeOffRequestDocument
                               .Where(w => w.EmployeeTimeOffRequestId == id && w.DataEntryStatus == 1 &&
                                (w.SubmissionType == "Mandatory"||(w.SubmissionType == "Conditional Mandatory" && w.TimeoffDays<timeOffRequest.WorkingDays)) && w.Status == "NA").Count();
            if (pendingRequiredCnt > 0) isPendingRequiredDocument = true;

            return isPendingRequiredDocument;
        }
        private void ProcessRequestWorkflow(EmployeeTimeOffRequest timeOffRequest, string workflowExecutionType)
        {
            if (workflowExecutionType == "Start")
            {
                WorkflowTriggerRequestDetail workflowTriggerRequestDetail = WorkflowService.StratWorkflow(timeAideWebContext,(int)WorkflowService.ChangeRequestTriggerType.EMP_TIMEOFF_REQ);
                workflowTriggerRequestDetail.WorkflowTriggerRequest.EmployeeTimeOffRequest = timeOffRequest;
                if (workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow.IsZeroLevel)
                {
                    workflowTriggerRequestDetail.WorkflowActionTypeId = 2;
                    workflowTriggerRequestDetail.ActionRemarks = "Zero Level Approved";
                    timeOffRequest.ChangeRequestStatusId = 2;
                    WorkflowService.TimeAideWindowContext = this.timeAideWindowContext;
                    timeOffRequest.UserInformation = timeAideWebContext.UserInformation.Find(timeOffRequest.UserInformationId);
                    WorkflowService.ApplyChanges(timeAideWebContext, timeOffRequest as EmployeeTimeOffRequest);
                    timeAideWebContext.SaveChanges();
                    //var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(model.UserInformationId ?? 0, SessionHelper.SelectedClientId);
                    //Dictionary<string, string> toEmails = userInformationList.ToDictionary(k => k.ShortFullName, v => v.LoginEmail);
                    //TimeAide.Services.Helpers.UtilityHelper.SendEmailByWorkflowClosingNotification(model.UserInformationId.Value, changeRequest.Id, workflowTriggerRequestDetail.WorkflowTriggerRequest.WorkflowTrigger.Workflow, toEmails, db);
                    WorkflowService.ProcesstWorkflowClosingNotification(timeAideWebContext, workflowTriggerRequestDetail.WorkflowTriggerRequest, timeOffRequest);
                }
                else
                {
                    timeAideWebContext.SaveChanges();
                    try
                    {
                      UtilityHelper.SendEmailByWorkflow(timeOffRequest.UserInformationId, workflowTriggerRequestDetail, timeOffRequest.Id, timeAideWebContext);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            else// process approval level
            {
                WorkflowService.TimeAideWindowContext = this.timeAideWindowContext;
                WorkflowTriggerRequest workflowTriggerRequest = timeAideWebContext.WorkflowTriggerRequest.FirstOrDefault(t => t.EmployeeTimeOffRequestId == timeOffRequest.Id);
                WorkflowService.GetNextWorkflowLevel<EmployeeTimeOffRequest>(timeAideWebContext, workflowTriggerRequest, timeOffRequest);
            }
        }
    }
}