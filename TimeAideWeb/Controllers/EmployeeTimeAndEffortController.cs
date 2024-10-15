
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
using TimeAide.Data;
using Antlr.Runtime.Tree;
//using DocumentFormat.OpenXml.Bibliography;

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
    public class EmployeeTimeAndEffortController : BaseTAWindowRoleRightsController<EmployeeTimeAndEffort>
    {
        
        [ChildActionOnly]
        public ActionResult GetTimeAEffortFilters()
        {
            try
            {
                SetFilterDropdownList();

                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeTimeAndEffort", "GetTimeAEffortFilters");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult Index()
        {
            try
            {
                var endingDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1); ;
                var endingDaysBefore = (DateTime.Today - endingDate).Days; //end limit

                ViewBag.TAEEndDD = string.Format("-{0}d", endingDaysBefore);
                TempData["InitDate"] = endingDate;
                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "EmployeeTimeAndEffort", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }        
        
        
        public ActionResult GetTimeAndEffort(UserFilterViewModel recFilter)
        {
            try
            {
                var model = getTimeAndEffortGridData(recFilter);
                if (model.Count() > 0)
                {
                    ViewBag.SelectedTAEIds = model.Select(x => x.TAEId).Aggregate((s1, s2) => s1 + "," + s2);
                                        
                }
                return PartialView("TAESupervisorView", model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "GetScheduleCalendar");
                return PartialView("Error", handleErrorInfo);
            }
        }
        public ActionResult TimeAndEffortApprovalPopup(TimeAndEffortApprovalViewModel model)
        {
            try
            {
                if (model.ActionType == "I")
                {
                   var taeSignEntity= timeAideWindowContext.tblTAESignature.Where(x => x.intTAESignID == model.TAEIds).FirstOrDefault();
                    if (taeSignEntity != null)
                    {
                        model.EmployeeId = taeSignEntity.intUserID;
                        model.EmployeeName = taeSignEntity.strUserName;
                        model.ApprovalTypeId = (taeSignEntity.bitSupervisorSigned??false)?1:0;
                        var approvalTypes = TimeAndEffortApproval.Unapproved.ToSelectList();
                        ViewBag.EditApprovalTypeId = new SelectList(approvalTypes,"Value","Text", model.ApprovalTypeId);
                    }
                }

                return PartialView("TAEApprovalPopup", model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeAndEffort", "TimeAndEffortApprovalPopup");
                return PartialView("Error", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult TAEffortApproval(TimeAndEffortApprovalViewModel model)
        {
            string status = "Success";
            string message = "Successfully Updated!";
            int? nullIntVal = null;
            DateTime? nullDateTime = null;
            try
            {
                if (string.IsNullOrEmpty(model.TAEIds))
                {
                    throw new Exception("Invalid time and effort record.");
                }
                else
                {
                    var tAEIds = model.TAEIds.Split(',');
                    var tAERecordList = timeAideWindowContext.tblTAESignature.Where(x => tAEIds.Contains(x.intTAESignID));
                    if (model.ActionType == "I")
                    {
                        var tAERecord = tAERecordList.FirstOrDefault();
                       

                        tAERecord.bitSupervisorSigned = model.ApprovalTypeId == 0 ? false : true;
                        tAERecord.intSupervisorID = model.ApprovalTypeId == 0 ? nullIntVal : SessionHelper.LoginEmployeeId;
                        tAERecord.strSupervisorEntry = model.ApprovalTypeId == 0 ? "":SessionHelper.LoginEmployeeName; 
                        tAERecord.dtSupervisorDateTime = model.ApprovalTypeId == 0 ? nullDateTime : DateTime.Now;

                    }
                    else if(model.ActionType == "A")
                    {
                       var tAEFilterRecList= tAERecordList.Where(x => (x.bitSupervisorSigned ?? false) == (model.ApprovalTypeId == 0 ? true : false));

                        foreach( var tAERecord in tAEFilterRecList)
                        {
                            tAERecord.bitSupervisorSigned = model.ApprovalTypeId == 0 ? false : true;
                            tAERecord.intSupervisorID = model.ApprovalTypeId == 0 ? nullIntVal : SessionHelper.LoginEmployeeId;
                            tAERecord.strSupervisorEntry = model.ApprovalTypeId == 0 ? "" : SessionHelper.LoginEmployeeName;
                            tAERecord.dtSupervisorDateTime = model.ApprovalTypeId == 0 ? nullDateTime : DateTime.Now;

                        }
                    }
                    timeAideWindowContext.SaveChanges();
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
        public ActionResult GetDashboardCurrentTimeAEffortWidget(int? id)
        {
            var statusCode = "Success"; //NotFound//NotApproved 
            var userId = id ?? SessionHelper.LoginId;
            var userInfo = timeAideWebContext.UserInformation.Where(w => w.Id == userId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();

            var effortDate = DateTime.Today.AddMonths(-1);
            var timeAEffortUserList = DataHelper.spSS_GetTimeAndEffortDataForWeb<EmployeeTimeAndEffort>(timeAideWindowContext, userInfo.EmployeeId ?? 0, "", effortDate);
            ViewBag.EffortMonth = effortDate.ToString("MMMM yyyy");
            if (timeAEffortUserList.Count() ==0)
            {
                statusCode = "NotFound";
            }
            else
            {
                statusCode = timeAEffortUserList.FirstOrDefault().IsSupervisorApproved == false ? "NotApproved" : statusCode;
            }
            ViewBag.StatusCode= statusCode;
            return PartialView("TimeAEffortWidget", timeAEffortUserList);
        }
        public ActionResult SelfServiceTimeAndEffort(string id)
        {
            //int? id = SessionHelper.LoginId;
            ViewBag.selfServiceTAEBeforeDD = "";
            ViewBag.selfServiceTAEEndDD = "";
            var userId = id == null ? SessionHelper.LoginId : int.Parse(Encryption.DecryptURLParm(id));
            ViewBag.userId = userId;
            var appConfigTAEBeforeMM = timeAideWebContext.ApplicationConfiguration.Where(w => w.ClientId == SessionHelper.SelectedClientId && w.ApplicationConfigurationName == "SelfServiceTAEBeforeMonths")
                                                                                      .Select(w => (w.ApplicationConfigurationValue)).FirstOrDefault();
            int intTAEBeforeMM ;          

           if( int.TryParse(appConfigTAEBeforeMM, out intTAEBeforeMM))
            {
                intTAEBeforeMM++;
                intTAEBeforeMM *= -1;
                var startOfCurrMonthDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                int intTAEBeforeDD = (startOfCurrMonthDate - startOfCurrMonthDate.AddMonths(intTAEBeforeMM)).Days; //before days limit
                var monthDays=  (DateTime.Today - startOfCurrMonthDate.AddMonths(-1)).Days - 1; //end limit
               
                ViewBag.selfServiceTAEBeforeDD = string.Format("-{0}d", intTAEBeforeDD);
                ViewBag.selfServiceTAEEndDD = string.Format("-{0}d", monthDays);
                ViewBag.initDate = startOfCurrMonthDate.AddMonths(-1);
            }
            
        
            return PartialView("SelfServiceTAE");
        }
        public ActionResult GetSelfServiceTAEDetail(UserFilterViewModel tAEFilter)
        {
            EmployeeTimeAndEffortViewModel model = null;
            //  var userId = 248;//SessionHelper.LoginId;
            var userId = tAEFilter.EmployeeId == 0 ? SessionHelper.LoginId : tAEFilter.EmployeeId.Value;
            try
            {
                var userInfo = timeAideWebContext.UserInformation.Find(userId);
                if(userInfo != null) {
                 tAEFilter.EmployeeId = userInfo.EmployeeId;
                 tAEFilter.TAEApprovalTypeId= (int)TimeAndEffortApproval.Approved;
                var gridDate = getTimeAndEffortGridData(tAEFilter);
                    model = gridDate.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "TimeAndEffort", "GetSelfServiceTAEDetail");
                return PartialView("Error", handleErrorInfo);
            }

            return PartialView("SelfServiceTAEDetail", model);
        }
        public ActionResult TAEEmployeeApprovalPop(TimeAndEffortApprovalViewModel model)
        {
            try
            {
                model.ApprovalTypeId = (int)TimeAndEffortApproval.Approved;           

                return PartialView("TAEEmployeeApproval", model);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "Time&Effort", "TAEEmployeeApproval");
                return PartialView("Error", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult TAEEmployeeApproval(TimeAndEffortApprovalViewModel model)
        {
            string status = "Success";
            string message = "Successfully Approved!";

            try
            {
                if (string.IsNullOrEmpty(model.TAEIds))
                {
                    throw new Exception("Invalid time and effort record.");
                }
                else
                {
                    var tAEIds = model.TAEIds.Split(',');
                    var tAERecord = timeAideWindowContext.tblTAESignature.Where(x => tAEIds.Contains(x.intTAESignID)).FirstOrDefault();
                    if (tAERecord!=null)
                    {                       
                        tAERecord.bitEmployeeSigned = model.ApprovalTypeId == 0 ? false : true;
                        tAERecord.strEmployeeEntry = tAERecord.strUserName;
                        tAERecord.dtEmployeeDateTime = DateTime.Now;

                    }
                    
                    timeAideWindowContext.SaveChanges();
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

        private void SetFilterDropdownList()
        {
           
            ViewBag.SupApprovalTypeId = TimeAndEffortApproval.Unapproved.ToSelectList();
            ViewBag.EmpApprovalTypeId = ViewBag.SupApprovalTypeId;
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
        private IEnumerable<EmployeeTimeAndEffortViewModel> getTimeAndEffortGridData(UserFilterViewModel tAEFilter)
        {
            IList<EmployeeTimeAndEffortViewModel> tAEGridViewModel = new List<EmployeeTimeAndEffortViewModel>();

            try
            {
                var tAWFilterData = geTimeAndEffortFilteredData(tAEFilter);
                if (tAWFilterData.Count>0)
                {
                    var employeeList= tAWFilterData.Select(s=>s.UserId).Distinct().ToList();
                    var startDate = tAWFilterData.Min(m => m.PunchDate);
                    var endDate = tAWFilterData.Max(m => m.PunchDate);
                    var effortDateRange = new List<DateTime>();
                    while (startDate <= endDate)
                    {
                        effortDateRange.Add(startDate);
                        startDate = startDate.AddDays(1);
                    }
                    ViewBag.EffortDateRange= effortDateRange;
                    foreach (var employeeId in employeeList)
                    {
                        var empTAERec = tAWFilterData.Where(w=>w.UserId == employeeId);
                        if (empTAERec != null) {
                            var firstRec = empTAERec.FirstOrDefault();
                            var empTAEViewRecord = new EmployeeTimeAndEffortViewModel { EmployeeId= employeeId,EmployeeName=firstRec.UserName,EffortYear=firstRec.Year,EffortMonth=firstRec.Year, TAEId= firstRec.TAEId,IsSupervisorApproved=firstRec.IsSupervisorApproved,IsEmployeeApproved=firstRec.IsEmployeeApproved };
                            
                            empTAEViewRecord.EffortTypeRange = empTAERec.Select(s => s.CompensationName).Distinct().OrderBy(o=>o).ToList();
                            empTAEViewRecord.TimeAndEffortDetail = new List<EmployeeTimeAndEffortDetailViewModel>();
                            
                            foreach(var effortDate in effortDateRange)
                            {
                                var tAEDetail = new EmployeeTimeAndEffortDetailViewModel { EffortDate= effortDate };
                                tAEDetail.EffortInfo = new List<EffortInfoViewModel>();
                                foreach (var effortType in empTAEViewRecord.EffortTypeRange)
                                {
                                    var tAEInfo = new EffortInfoViewModel {EffortTypeName= effortType };
                                    var effort=empTAERec.Where(w => w.PunchDate == effortDate && w.CompensationName == effortType).FirstOrDefault();
                                    if(effort != null)
                                    {
                                        tAEInfo.EffortHrs = effort.EffortHours;
                                    }
                                    tAEDetail.EffortInfo.Add(tAEInfo);
                                }
                                empTAEViewRecord.TimeAndEffortDetail.Add(tAEDetail);
                            }
                            tAEGridViewModel.Add(empTAEViewRecord);
                        }
                        
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tAEGridViewModel;

        }
        private List<EmployeeTimeAndEffort> geTimeAndEffortFilteredData(UserFilterViewModel tAEFilter)
        {
            List<EmployeeTimeAndEffort> retEmployeeTimeAndEffort = null ;
           
            try
            {
               
                var employeeName = tAEFilter.EmployeeName;
                var employeeId = tAEFilter.EmployeeId ?? 0;
                //tAEFilter.EmployeeName = "";
                //tAEFilter.EmployeeId = 0;
                var supEmployeesList= getSupervisorEmployeesList(tAEFilter);
                if (supEmployeesList != null)
                {
                    var timeAEffortUserList = DataHelper.spSS_GetTimeAndEffortDataForWeb<EmployeeTimeAndEffort>(timeAideWindowContext, employeeId, employeeName, tAEFilter.ScheduleDate ?? DateTime.Today)
                                              .Where(x => (tAEFilter.TAEApprovalTypeId == null) || (x.IsSupervisorApproved == (tAEFilter.TAEApprovalTypeId == 0 ? false : true)))
                                              .Where(x => (tAEFilter.TAEEmpApprovalTypeId == null) || (x.IsEmployeeApproved == (tAEFilter.TAEEmpApprovalTypeId == 0 ? false : true)));
                    if (timeAEffortUserList != null)
                    {
                       var empIds = supEmployeesList.Select(s => s.EmployeeId).ToArray();
                        retEmployeeTimeAndEffort = timeAEffortUserList.Where(w=> empIds.Contains(w.UserId)).ToList();
                    }
                } 

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retEmployeeTimeAndEffort;

        }
       
        private IEnumerable<UserInformationViewModel> getSupervisorEmployeesList(UserFilterViewModel schFilter)
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
