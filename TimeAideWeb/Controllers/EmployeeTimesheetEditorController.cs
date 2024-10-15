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
using Antlr.Runtime.Tree;
using System.Text;
using System.Web.UI;

namespace TimeAide.Web.Controllers
{
    public class EmployeeTimesheetEditorController : BaseTAWindowRoleRightsController<EmployeeTimesheetEditor>
    {
        public ActionResult Index()
        {
            try
            {
                var loginUser = timeAideWebContext.UserInformation.Include("SupervisorSubDepartment.SubDepartment")
                                                                  .Include("SupervisorDepartment.Department")
                                                                  .Include("SupervisorEmployeeType.EmployeeType").FirstOrDefault(u => u.Id == SessionHelper.LoginId);
                var contextWin = DataHelper.GetSelectedClientTAWinEFContext();
                if (loginUser.SupervisorCompany.Count == 0 || loginUser.SupervisorCompany.Where(c => c.CompanyId == SessionHelper.SelectedCompanyId).Count() == 0)
                {
                    ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName").OrderBy(o => o.Text);
                    ViewBag.SubDepartmentId = new SelectList(new List<SubDepartment>(), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    ViewBag.EmployeeTypeId = new SelectList(new List<EmployeeType>(), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                    ViewBag.HasCompanyId = false;
                }
                else
                {
                    ViewBag.HasCompanyId = true;
                    if (loginUser.SupervisorDepartment.Count == 0)
                        ViewBag.DepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
                    else
                        ViewBag.DepartmentId = new SelectList(loginUser.SupervisorDepartment.Select(d => d.Department), "Id", "DepartmentName").OrderBy(o => o.Text);
                    if (loginUser.SupervisorSubDepartment.Count == 0)
                        ViewBag.SubDepartmentId = new SelectList(timeAideWebContext.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    else
                        ViewBag.SubDepartmentId = new SelectList(loginUser.SupervisorSubDepartment.Select(s => s.SubDepartment), "Id", "SubDepartmentName").OrderBy(o => o.Text);
                    if (loginUser.SupervisorEmployeeType.Count == 0)
                        ViewBag.EmployeeTypeId = new SelectList(timeAideWebContext.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                    else
                        ViewBag.EmployeeTypeId = new SelectList(loginUser.SupervisorEmployeeType.Select(d => d.EmployeeType), "Id", "EmployeeTypeName").OrderBy(o => o.Text);
                }
                List<SelectListItem> ReportTypeList = new List<SelectListItem>();
                ReportTypeList.Add(new SelectListItem { Text = "Timesheet", Value = "1" });
               // ReportTypeList.Add(new SelectListItem { Text = "Punch Date", Value = "2" });
                ViewBag.ReportTypeId = new SelectList(ReportTypeList, "Value", "Text");

                return PartialView();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "AttendanceSchedule", "Index");
                return PartialView("Error", handleErrorInfo);
            }
        }

        public ActionResult Timesheet(tReportWeek model)
        {
            TimeAideContext db = new TimeAideContext();
            //model.e_id = model.UserInformationId;
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            model.SelectedEmployeeIds = SupervisorHelper.GetSupervisedEmployeeIds(model);
            var reportWeek = DataHelper.SP_GetTimesheetReportWeek<tReportWeek>(contextWinDb, model);
            ViewBag.SelectedReportWeekIds = string.Join(",", reportWeek.Select(s => s.tpwID.ToString()));

            return PartialView(reportWeek);
        }
        public ActionResult TimesheetDetail(tReportWeek model)
        {
            var contextWinDb = DataHelper.GetSelectedClientTAWinEFContext();
            var punchDate = new tPunchDate() { nWeekID = model.nWeekID };
            var punchDateList = DataHelper.SP_GetPunchDate<tPunchDate>(contextWinDb, punchDate);
            return PartialView(punchDateList);
        }
        
        public ActionResult AddTransaction(string timesheetDetailIds,string openType)
        {
            IEnumerable<tPunchDate> model = getTransDateDetail(timesheetDetailIds);
           
            var employeeid = model.FirstOrDefault().e_id;
            ViewBag.dayHours = getDayHours(employeeid);
            ViewBag.selectedIds = timesheetDetailIds;
            ViewBag.openType = openType;
            var transTypeList = timeAideWindowContext.tTransDef.Select(s => new { Id = s.ID.ToString()+","+ (s.boolUseSickInFamily? "SIF" : s.sAccrualImportName)+","+s.nIsMoneyTrans.ToString(), Name = s.Name }).ToList();
             ViewBag.TransTypeId = new SelectList(transTypeList, "Id", "Name").OrderBy(o => o.Text);
            return PartialView(model);
        }
        public ActionResult EditTransaction(string timesheetDetailIds, int transId)
        {
            var model = timeAideWindowContext.tPunchPair.Where(w=>w.tppID== transId).FirstOrDefault();

            var employeeid = model.e_id.Value;
            ViewBag.dayHours = getDayHours(employeeid);           
            var transTypeList = timeAideWindowContext.tTransDef.Select(s => new {TransDefId=s.ID, Id = s.ID.ToString() + "," + (s.boolUseSickInFamily ? "SIF" : s.sAccrualImportName) + "," + s.nIsMoneyTrans.ToString(), Name = s.Name }).ToList();
            ViewBag.TransTypeId = new SelectList(transTypeList, "Id", "Name").OrderBy(o => o.Text);
            var selectedTransType = transTypeList.Where(w => w.Name == model.sType).FirstOrDefault();
            ViewBag.SelectedTransVal = selectedTransType.Id;
            ViewBag.SelectedAccrualType = selectedTransType.Id.Split(',')[1];
            ViewBag.selectedIds = timesheetDetailIds;
            return PartialView("EditSingleTransaction",model);
        }
        public JsonResult SaveEditTransaction(TimeOffRenderMonthViewModel model)
        {
            string status = "Success";
            string message = "Transaction is saved successfully!";            
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    var transId = int.Parse(model.ReferenceIds);
                    var transEntity= timeAideWindowContext.tPunchPair.Where(w=>w.tppID == transId).FirstOrDefault();
                    if (transEntity != null)
                    {
                        var oldTransType = transEntity.sType;
                        var oldValue = transEntity.HoursWorked;
                        var employeeId = transEntity.e_id??0;
                        //update data
                        transEntity.sType = model.TransType;
                        transEntity.HoursWorked = (double)model.TimeOffDayHours;
                        transEntity.b_Processed = false;
                        transEntity.sTDesc = model.TransNote;
                        //Save Audit
                        var auditEntryList = new List<tAuditInfo>();
                         var auditEntry = new tAuditInfo();
                         auditEntry.DTTimeStamp = DateTime.Now;
                         auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                         auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                         auditEntry.sAdminAction = "Modified";
                         auditEntry.sRecordAffected = "Transactions";
                         auditEntry.nUserIDAffected = transEntity.e_id;
                         auditEntry.sUserNameAffected = transEntity.e_name;
                         auditEntry.sFieldName = transEntity.sType;
                         auditEntry.PrevValue = $"Old:{transEntity.DTPunchDate:MM/dd/yy} {oldTransType}={oldValue}";
                         auditEntry.NewValue = $"New:{transEntity.DTPunchDate:MM/dd/yy} {transEntity.sType}={transEntity.HoursWorked}";
                         auditEntry.sNote = model.TransNote;
                         auditEntryList.Add(auditEntry);
                        
                        if (auditEntryList.Count > 0)
                        {
                            DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                        }
                        timeAideWindowContext.SaveChanges();
                        requestSavingTimeAideWindowDBTrans.Commit();                                             
                        //timesheet recompute by calling service.
                        recomputeTimesheetData(employeeId);
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult DeleteTransaction(string timesheetDetailIds,string transId, string openType)
        {
            IEnumerable<TimesheetEditorTransactionViewModel> model = getDeleteTransOfDateList(timesheetDetailIds);
            ViewBag.selectedIds = timesheetDetailIds;
            ViewBag.openType = openType;
            if (openType == "EPDate")
            {
                var retModel = model.Where(w=>w.tppID == int.Parse(transId)).FirstOrDefault();
                return PartialView("DeleteSingleTransaction", retModel);
            }
            
            return PartialView(model);
        }
        public ActionResult AddPunches(string timesheetDetailIds, string openType)
        {
            IEnumerable<tPunchDate> model = getTransDateDetail(timesheetDetailIds);
                              
            ViewBag.selectedIds = timesheetDetailIds;
            ViewBag.openType = openType;


            return PartialView(model);
        }

        public ActionResult DeletePunches(string timesheetDetailIds, string openType,string deletePunchId)
        {
            List<TimesheetEditorPunchDetailViewModel> model = new List<TimesheetEditorPunchDetailViewModel>();
            IEnumerable<tPunchDate> timesheetData = getTransDateDetail(timesheetDetailIds);
            //var employeeid = timesheetData.FirstOrDefault().e_id;
            foreach (tPunchDate selectedRow in timesheetData)
            {
                var punchDetail = new TimesheetEditorPunchDetailViewModel()
                {
                    e_id = selectedRow.e_id,
                    e_name = selectedRow.e_name,
                    PunchDate = selectedRow.DTPunchDate.Value,
                    nWeekID = selectedRow.nWeekID??0,
                    PunchData = getPunchDateDetail(selectedRow.e_id, selectedRow.DTPunchDate.Value)

                };
                model.Add(punchDetail);
            }
            if (openType == "EPDate")
            {
                var retModel = model.FirstOrDefault();
                retModel.PunchData = retModel.PunchData.Where(w => w.tpdID == int.Parse(deletePunchId));
                return PartialView("DeleteSinglePunch", retModel);
            }
            return PartialView(model);
        }
        public JsonResult AjaxValidatePunches(TimesheetEditorPunchesViewModel model)
        {
            string status = "Success";
            string message = "Punch(es) is/are validated successfully!";
            double inOut1Hrs = 0.00;
            double inOut2Hrs = 0.00;
            model.PunchIn1 = model.PunchIn1Overnight ? model.PunchIn1.AddHours(24) : model.PunchIn1;
            model.PunchOut1 = model.PunchOut1Overnight && model.PunchCount>1 ? model.PunchOut1.AddHours(24) : model.PunchOut1;
            model.PunchIn2 = model.PunchIn2Overnight && model.PunchCount >2 ? model.PunchIn2.AddHours(24) : model.PunchIn2;
            model.PunchOut2 = model.PunchOut2Overnight && model.PunchCount > 2 ? model.PunchOut2.AddHours(24) : model.PunchOut2;
            switch (model.PunchCount)
            {
               
                case 2:                    
                    var diffInOut1Mins = (model.PunchOut1.Subtract(model.PunchIn1).TotalMinutes);
                    if (diffInOut1Mins < 0)
                    {
                        status = "Error";
                        message="Punch Out1 should not be before Punch In1";
                    }
                    inOut1Hrs = Math.Round((diffInOut1Mins / 60), 2);
                    break;
                case 4:                    
                    var diffInOut11Mins = (model.PunchOut1.Subtract(model.PunchIn1).TotalMinutes);
                    if (diffInOut11Mins < 0)
                    {
                        status = "Error";
                        message="Punch Out1 should not be before Punch In1";
                    }
                    
                    var diffInOut22Mins = (model.PunchOut2.Subtract(model.PunchIn2).TotalMinutes);
                    if (diffInOut22Mins < 0)
                    {
                        status = "Error";
                        message = "Punch Out2 should not be before Punch In2";
                    }
                    if ((model.PunchIn2 >= model.PunchIn1 && model.PunchIn2 <= model.PunchOut1)
                            || (model.PunchOut2 >= model.PunchIn1 && model.PunchOut2 <= model.PunchOut1))
                    {
                        status = "Error";
                        message = "Punch In2/Out2 should not be overlapped with Punch In1/Out1.";
                    }
                    inOut1Hrs = Math.Round((diffInOut11Mins / 60), 2);
                    inOut2Hrs = Math.Round(((diffInOut22Mins) / 60), 2);
                    break;
            }
            return Json(new { status = status, message = message, inOut1Hrs = inOut1Hrs, inOut2Hrs = inOut2Hrs });
        }
        public JsonResult AjaxComputePunchesTime(TimesheetEditorPunchesViewModel model)
        {
            string status = "Success";
            string message = "";
            double inOut1Hrs = 0.00;
            double inOut2Hrs = 0.00;
            model.PunchIn1 = model.PunchIn1Overnight ? model.PunchIn1.AddHours(24) : model.PunchIn1;
            model.PunchOut1 = model.PunchOut1Overnight && model.PunchCount > 1 ? model.PunchOut1.AddHours(24) : model.PunchOut1;
            model.PunchIn2 = model.PunchIn2Overnight && model.PunchCount > 2 ? model.PunchIn2.AddHours(24) : model.PunchIn2;
            model.PunchOut2 = model.PunchOut2Overnight && model.PunchCount > 2 ? model.PunchOut2.AddHours(24) : model.PunchOut2;
            
                if (model.PunchCount >= 2) {
                var diffInOut1Mins = (model.PunchOut1.Subtract(model.PunchIn1).TotalMinutes);

                inOut1Hrs = Math.Round((diffInOut1Mins / 60), 2);
            }
            if (model.PunchCount == 4) { 
                    
                    var diffInOut22Mins = (model.PunchOut2.Subtract(model.PunchIn2).TotalMinutes);
                    
                    inOut2Hrs = Math.Round(((diffInOut22Mins) / 60), 2);

            }
            return Json(new { status = status, message = message, inOut1Hrs = inOut1Hrs, inOut2Hrs = inOut2Hrs });
        }
        public JsonResult SavePunches(TimesheetEditorPunchesViewModel model)
            {
                string status = "Success";
                string message = "Punch(es) are processed successfully!";
                long nWeekId = 0;
                int employeeId = 0; 
                model.PunchIn1 = model.PunchIn1Overnight ? model.PunchIn1.AddHours(24) : model.PunchIn1;
                model.PunchOut1 = model.PunchOut1Overnight && model.PunchCount > 1 ? model.PunchOut1.AddHours(24) : model.PunchOut1;
                model.PunchIn2 = model.PunchIn2Overnight && model.PunchCount > 2 ? model.PunchIn2.AddHours(24) : model.PunchIn2;
                model.PunchOut2 = model.PunchOut2Overnight && model.PunchCount > 2 ? model.PunchOut2.AddHours(24) : model.PunchOut2;

            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
                {
                    try
                    {
                    IList<tPunchData> punchDataForAudit = new List<tPunchData>();
                    IEnumerable<tPunchDate> punchDateList = getTransDateDetail(model.ReferenceIds);
                    
                    var defualtPunchDataEntity = new tPunchData();
                    
                    if (punchDateList.Count() > 0)
                    {
                       var firstRecord = punchDateList.FirstOrDefault();
                        employeeId = firstRecord.e_id;
                        nWeekId = firstRecord.nWeekID??0;
                        defualtPunchDataEntity.e_id = employeeId;
                        defualtPunchDataEntity.e_name = firstRecord.e_name;
                        defualtPunchDataEntity.g_id = -1;
                        defualtPunchDataEntity.b_Processed = false;
                        defualtPunchDataEntity.sModType = "a";
                        defualtPunchDataEntity.nAdminID = SessionHelper.LoginEmployeeId;
                        defualtPunchDataEntity.sNote = model.Note;
                        defualtPunchDataEntity.e_mode = "1";
                        defualtPunchDataEntity.e_type = "0";
                        defualtPunchDataEntity.e_result = "0";
                        defualtPunchDataEntity.b_Modified = true;
                        defualtPunchDataEntity.e_group = 0;
                        defualtPunchDataEntity.e_user = "0";
                        defualtPunchDataEntity.nJobCodeID = 0;

                        foreach (var punchDate in punchDateList)
                        {
                            defualtPunchDataEntity.DTPunchDate = punchDate.DTPunchDate;
                           
                            if (model.PunchCount >= 1)
                            {
                                //Punch-in 1
                                var punchIn1Span = model.PunchIn1.Subtract(DateTime.Today);
                                var punchIn1Data = (tPunchData)defualtPunchDataEntity.Clone();
                               
                                punchIn1Data.DTPunchDateTime = punchIn1Data.DTPunchDate+punchIn1Span;
                                punchIn1Data.DTPunchDateTime_Original = punchIn1Data.DTPunchDateTime;
                                punchIn1Data.DTPunchDateTime_Rounded = punchIn1Data.DTPunchDateTime;
                                timeAideWindowContext.tPunchData.Add(punchIn1Data);
                                punchDataForAudit.Add(punchIn1Data);
                            }
                            if(model.PunchCount >= 2)
                            {
                                //Punch-out 1
                                var punchOut1Span = model.PunchOut1.Subtract(DateTime.Today);
                                var punchOut1Data = (tPunchData)defualtPunchDataEntity.Clone();                               
                                punchOut1Data.DTPunchDateTime = punchOut1Data.DTPunchDate + punchOut1Span;
                                punchOut1Data.DTPunchDateTime_Original = punchOut1Data.DTPunchDateTime;
                                punchOut1Data.DTPunchDateTime_Rounded = punchOut1Data.DTPunchDateTime;
                                timeAideWindowContext.tPunchData.Add(punchOut1Data);
                                punchDataForAudit.Add(punchOut1Data);

                            }
                            if (model.PunchCount == 4)
                            {
                               // Punch -in 2
                                var punchIn2Span = model.PunchIn2.Subtract(DateTime.Today);
                                var punchIn2Data = (tPunchData)defualtPunchDataEntity.Clone();

                                punchIn2Data.DTPunchDateTime = punchIn2Data.DTPunchDate + punchIn2Span;
                                punchIn2Data.DTPunchDateTime_Original = punchIn2Data.DTPunchDateTime;
                                punchIn2Data.DTPunchDateTime_Rounded = punchIn2Data.DTPunchDateTime;
                                timeAideWindowContext.tPunchData.Add(punchIn2Data);
                                punchDataForAudit.Add(punchIn2Data);
                                //Punch-out 2
                                var punchOut2Span = model.PunchOut2.Subtract(DateTime.Today);
                                var punchOut2Data = (tPunchData)defualtPunchDataEntity.Clone();

                                punchOut2Data.DTPunchDateTime = punchOut2Data.DTPunchDate + punchOut2Span;
                                punchOut2Data.DTPunchDateTime_Original = punchOut2Data.DTPunchDateTime;
                                punchOut2Data.DTPunchDateTime_Rounded = punchOut2Data.DTPunchDateTime;
                                timeAideWindowContext.tPunchData.Add(punchOut2Data);
                                punchDataForAudit.Add(punchOut2Data);
                            }
                        }
                        timeAideWindowContext.SaveChanges();
                        //Audit detail for added punches
                        if (punchDataForAudit.Count > 0)
                        {
                            var auditEntryList = new List<tAuditInfo>();
                            foreach (var addedPunch in punchDataForAudit)
                            {
                                var auditEntry = new tAuditInfo();
                                auditEntry.DTTimeStamp = DateTime.Now;
                                auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                                auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                                auditEntry.sAdminAction = "Added";
                                auditEntry.sRecordAffected = "Punches";
                                auditEntry.nUserIDAffected = addedPunch.e_id;
                                auditEntry.sUserNameAffected = addedPunch.e_name;
                                auditEntry.sFieldName = addedPunch.DTPunchDate.Value.ToString("MM/dd/yyyy");
                                auditEntry.NewValue = $"Add:{addedPunch.DTPunchDateTime:MM/dd/yyyy hh:mm tt}";
                                auditEntry.nWeekID = nWeekId;
                                auditEntry.sNote = addedPunch.sNote;
                                auditEntryList.Add(auditEntry);
                            }
                            if (auditEntryList.Count > 0)
                            {
                                DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                            }
                            timeAideWindowContext.SaveChanges();
                        }
                            requestSavingTimeAideWindowDBTrans.Commit();
                        // recomputeTimesheetPunchesData(employeeId, punchDateList.Min(m=>m.DTPunchDate), punchDateList.Max(m=>m.DTPunchDate));                       
                        //timesheet recompute by calling service.
                        recomputeTimesheetData(employeeId);
                    }

                }
                    catch (Exception ex)
                    {
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                        requestSavingTimeAideWindowDBTrans.Rollback();
                        status = "Error";
                        message = ex.Message;
                    }
                }
                return Json(new { status = status, message = message, nWeekId = nWeekId });
            }
        public ActionResult EditPunchData(int punchId,string timesheetDetailIds)
        {
            var model = timeAideWindowContext.tPunchData.Where(w=>w.tpdID == punchId)
                                        .Select(s=>new TimesheetEditorPunchDataViewModel() {tpdID=s.tpdID,e_id=s.e_id,e_name=s.e_name,DTPunchDate=s.DTPunchDate.Value,DTPunchDateTime=s.DTPunchDateTime.Value })
                                        .FirstOrDefault();

            var hourDiffence =Math.Abs(model.DTPunchDate.Subtract(model.DTPunchDateTime).TotalHours);
            model.isOvernight=hourDiffence > 24;
            ViewBag.selectedId = timesheetDetailIds;
            return PartialView("EditSinglePunch",model);
        }
        public JsonResult AjaxSaveEditPunchData(TimesheetEditorPunchDataViewModel model)
        {
            string status = "Success";
            string message = "Punch is successfully updated!";
            long nWeekId = 0;
            int employeeId = 0;
            model.DTPunchDateTime = model.isOvernight ? model.DTPunchDateTime.AddHours(24) : model.DTPunchDateTime;
         
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    IList<tPunchData> punchDataForAudit = new List<tPunchData>();
                    IEnumerable<tPunchDate> punchDateList = getTransDateDetail(model.referenceId.ToString());
                    var punchDate = punchDateList.FirstOrDefault();
                    nWeekId = punchDate.nWeekID??0;
                    employeeId = punchDate.e_id;
                    var editPunchDataEntity = timeAideWindowContext.tPunchData.Where(w => w.tpdID == model.tpdID).FirstOrDefault();

                    if (editPunchDataEntity !=null)
                    {
                        editPunchDataEntity.b_Processed = false;
                        editPunchDataEntity.sModType = "e";
                        editPunchDataEntity.nAdminID = SessionHelper.LoginEmployeeId;
                        editPunchDataEntity.sNote = model.sNote;
                        var punchTimeSpan = model.DTPunchDateTime.Subtract(DateTime.Today);
                        var oldPunchTime = editPunchDataEntity.DTPunchDateTime;
                        editPunchDataEntity.DTPunchDateTime = model.DTPunchDate + punchTimeSpan;
                        editPunchDataEntity.DTPunchDateTime_Original = editPunchDataEntity.DTPunchDateTime;
                        editPunchDataEntity.DTPunchDateTime_Rounded = editPunchDataEntity.DTPunchDateTime;

                        var punchPairList = timeAideWindowContext.tPunchPair.Where(w => w.e_id == editPunchDataEntity.e_id && w.DTPunchDate == editPunchDataEntity.DTPunchDate && w.bTrans == false && w.b_Processed == true).ToList();
                        if (punchPairList.Count() > 0)
                        {
                            foreach (var pair in punchPairList)
                            {
                                pair.b_Processed = false;
                            }
                        }
                        punchDataForAudit.Add(editPunchDataEntity);
                                                
                        timeAideWindowContext.SaveChanges();
                        //Audit detail for added punches
                        if (punchDataForAudit.Count > 0)
                        {
                            var auditEntryList = new List<tAuditInfo>();
                            foreach (var addedPunch in punchDataForAudit)
                            {
                                var auditEntry = new tAuditInfo();
                                auditEntry.DTTimeStamp = DateTime.Now;
                                auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                                auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                                auditEntry.sAdminAction = "Modified";
                                auditEntry.sRecordAffected = "Punches";
                                auditEntry.nUserIDAffected = addedPunch.e_id;
                                auditEntry.sUserNameAffected = addedPunch.e_name;
                                auditEntry.sFieldName = addedPunch.DTPunchDate.Value.ToString("MM/dd/yyyy");
                                auditEntry.NewValue = $"New:{addedPunch.DTPunchDateTime:MM/dd/yyyy hh:mm tt}";
                                auditEntry.PrevValue= $"Old:{oldPunchTime:MM/dd/yyyy hh:mm tt}";
                                auditEntry.nWeekID = nWeekId;
                                auditEntry.sNote = addedPunch.sNote;
                                auditEntryList.Add(auditEntry);
                            }
                            if (auditEntryList.Count > 0)
                            {
                                DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                            }
                            timeAideWindowContext.SaveChanges();
                        }
                        requestSavingTimeAideWindowDBTrans.Commit();
                        // recomputeTimesheetPunchesData(employeeId, punchDateList.Min(m=>m.DTPunchDate), punchDateList.Max(m=>m.DTPunchDate));                       
                        //timesheet recompute by calling service.
                        recomputeTimesheetData(employeeId);
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message});
        }
        public ActionResult EditPunchDate(int timesheetDetailId)
        {            
            var punchDate = timeAideWindowContext.tPunchDates.Where(w => w.tpdID== timesheetDetailId).FirstOrDefault();

            var model = new TimesheetEditorPunchDateViewModel()
            {
                punchDate = punchDate.DTPunchDate.Value,
                e_id = punchDate.e_id,
                e_name = punchDate.e_name,
                tpdID = timesheetDetailId,
                nWeekID= punchDate.nWeekID,
            };
            setPunchDateScheduleInfo(model);

            return PartialView(model);
        }

        private void setPunchDateScheduleInfo(TimesheetEditorPunchDateViewModel punchDate)
        {
            var userSchedule = timeAideWindowContext.tSchedModPeriodSumms
                   .Where(w => punchDate.punchDate >= w.dStartDate && punchDate.punchDate <= w.dEndDate && w.nUserID==punchDate.e_id)
                   .Join(timeAideWindowContext.tusers,
                    usch => usch.nUserID,
                     ur => ur.id,
                     (usch, ur) => new 
                     {   Id = usch.ID,                        
                         CompanyId = ur.nCompanyID??0                  
                        
                     }).FirstOrDefault();
            
            var userSchDetail = timeAideWindowContext.tSchedModDailyDetails.Where(w => w.nUserID == punchDate.e_id &&
                                                               w.dPunchDate >= punchDate.punchDate && w.dPunchDate <= punchDate.punchDate).FirstOrDefault();
        
            if(userSchedule!=null && userSchDetail != null)
            {
                punchDate.schId = userSchedule.Id;
                punchDate.schDetailId = userSchDetail.ID;
                punchDate.e_companyId = userSchedule.CompanyId;

            }
        }
        public JsonResult AjaxRecomputeEditPunchDate(int timesheetDetailId)
        {
            string status = "Success";
            string message = "Successfully updated!";
           
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    var punchDateEntity=timeAideWindowContext.tPunchDates.Where(w=> w.tpdID==timesheetDetailId).FirstOrDefault();

                    var ppEntity=timeAideWindowContext.tPunchPair.Where(w => w.e_id == punchDateEntity.e_id && w.DTPunchDate == punchDateEntity.DTPunchDate && w.b_Processed == true).FirstOrDefault();
                    if(ppEntity!=null)
                    {
                        ppEntity.b_Processed = false;
                    }
                    else
                    {
                        ppEntity = new tPunchPair();
                        ppEntity.e_id = punchDateEntity.e_id;
                        ppEntity.e_name = punchDateEntity.e_name;
                        ppEntity.DTPunchDate = punchDateEntity.DTPunchDate;
                        ppEntity.DTimeIn = punchDateEntity.DTPunchDate;
                        ppEntity.DTimeOut = punchDateEntity.DTPunchDate;
                        ppEntity.sType = "HR";
                        ppEntity.pCode = "R";
                        ppEntity.b_Processed = false;
                        ppEntity.bTrans = false;
                        ppEntity.sTDesc = "AUTO";
                        timeAideWindowContext.tPunchPair.Add(ppEntity);
                    }
                    timeAideWindowContext.SaveChanges();
                    //timesheet recompute by calling service.
                    requestSavingTimeAideWindowDBTrans.Commit();
                    recomputeTimesheetData(punchDateEntity.e_id);
                    //requestSavingTimeAideWindowDBTrans.Commit();                  
                  
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message});
        }
        public ActionResult AjaxEditPunchDateDetail(int timesheetDetailId)
        {
            var punchDate = timeAideWindowContext.tPunchDates.Where(w => w.tpdID == timesheetDetailId).FirstOrDefault();
            var schDetail=timeAideWindowContext.tSchedModDailyDetails.Where(w => w.nUserID == punchDate.e_id &&
                                                               w.dPunchDate == punchDate.DTPunchDate).FirstOrDefault();
            var userWebInfo = timeAideWebContext.UserInformation.Where(w => w.Id == punchDate.e_id && w.ClientId== SessionHelper.SelectedClientId).Select(s => new { EmployeeId = s.EmployeeId, OldCompanyId = s.Company.Old_Id }).FirstOrDefault();
            
            var model = new TimesheetEditorPunchDateEditViewModel()
            {
                e_id = punchDate.e_id,
                e_name = punchDate.e_name,
                tpdID = timesheetDetailId,
                nWeekID = punchDate.nWeekID,
                PunchDate = punchDate.DTPunchDate.Value,
                nSchedID = punchDate.nSchedID,
                nSchedDetailID = schDetail==null ? 0 : schDetail.ID,
                sPunchSummary = punchDate.sPunchSummary,
                sExceptions = punchDate.sExceptions,
                sDaySchedule = punchDate.sDaySchedule,
                sHoursSummary = punchDate.sHoursSummary,
                TransactionData = getDeleteTransOfDateList(timesheetDetailId.ToString()),
                PunchData= getPunchDateDetail(punchDate.e_id, punchDate.DTPunchDate.Value).Select(s=> new TimesheetEditorPunchDataViewModel() { DTPunchDate = s.DTPunchDate.Value, DTPunchDateTime=s.DTPunchDateTime.Value, tpdID=s.tpdID})
        };

            return PartialView("RenderEditPunchDateDetail", model);
        }
        public JsonResult ConfirmDeleteTransaction(string ids,string[] notes)
        {
            string status = "Success";
            string message = "Transaction(s) is/are deleted successfully!";
            Int64 nWeekId = 0;
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    var result = DataHelper.spSS_DeleteTimesheeTransactionFromWeb<TransactionResultViewModel>(timeAideWindowContext, ids);

                    if (result != null)
                    {
                        var auditEntryList = new List<tAuditInfo>();
                        var noteIndex = 0;
                        foreach (var transactionResult in result)
                        {
                            var auditEntry = new tAuditInfo();
                            auditEntry.DTTimeStamp = DateTime.Now;
                            auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                            auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                            auditEntry.sAdminAction = "Deleted";
                            auditEntry.sRecordAffected = "Transactions";
                            auditEntry.nUserIDAffected = transactionResult.e_id;
                            auditEntry.sUserNameAffected = transactionResult.e_name;
                            auditEntry.sFieldName = transactionResult.sType;
                            auditEntry.PrevValue = $"Del:{transactionResult.DTPunchDate:MM/dd/yy} {transactionResult.sType}={transactionResult.HoursWorked}";
                            
                            auditEntry.sNote = notes[noteIndex];
                            noteIndex++;
                            auditEntryList.Add(auditEntry);
                        }
                        if (auditEntryList.Count > 0)
                        {
                            DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                        }
                        timeAideWindowContext.SaveChanges();
                        requestSavingTimeAideWindowDBTrans.Commit();
                        var firstTrans = result.FirstOrDefault();
                        var employeeId = firstTrans.e_id;
                        nWeekId = firstTrans.nWeekID;
                        recomputeTimesheetData(employeeId);
                       
                    }                  

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message, nWeekId = nWeekId });
        }
        public JsonResult ConfirmDeletePunches(string ids,long nWeekId,string note)
        {
            string status = "Success";
            string message = "Punch(es) are deleted successfully!";
            //Int64 nWeekId = 0;
            using (var punchSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    var auditEntryList = new List<tAuditInfo>();
                    var punchesList = ids.Split(',');
                    var punchDataList = timeAideWindowContext.tPunchData.Where(w => punchesList.Contains(w.tpdID.ToString())).ToList();

                    if (punchDataList.Count() > 0)
                    {
                        foreach (var punch in punchDataList)
                        {
                            punch.sModType = "d";
                            punch.b_Processed = false;
                            punch.sNote = note;

                            var punchPairList= timeAideWindowContext.tPunchPair.Where(w => w.e_id == punch.e_id && w.DTPunchDate== punch.DTPunchDate && w.bTrans==false && w.b_Processed==true).ToList();
                            if (punchPairList.Count() > 0)
                            {
                                foreach(var pair in punchPairList)
                                {
                                    pair.b_Processed = false;
                                }
                            }
                        }

                        timeAideWindowContext.SaveChanges();

                        foreach (var punch in punchDataList)
                        {
                            var auditEntry = new tAuditInfo();
                            auditEntry.DTTimeStamp = DateTime.Now;
                            auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                            auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                            auditEntry.sAdminAction = "Deleted";
                            auditEntry.sRecordAffected = "Punches";
                            auditEntry.nUserIDAffected = punch.e_id;
                            auditEntry.sUserNameAffected = punch.e_name;
                            auditEntry.sFieldName = punch.DTPunchDate.Value.ToString("MM/dd/yyyy");
                            auditEntry.PrevValue = $"Del:{punch.DTPunchDateTime:MM/dd/yyyy hh:mm tt}";
                            auditEntry.nWeekID = nWeekId;
                            auditEntry.sNote = note;
                            auditEntryList.Add(auditEntry);
                        }
                        if (auditEntryList.Count > 0)
                        {
                            DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                        }
                        timeAideWindowContext.SaveChanges();
                        punchSavingTimeAideWindowDBTrans.Commit();
                        var firstPunch = punchDataList.FirstOrDefault();
                        var employeeId = firstPunch.e_id;
                        recomputeTimesheetData(employeeId ?? 0);
                    }
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);

                    punchSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message, nWeekId = nWeekId });
        }
        public ActionResult AjaxRenderTransactionDates(TimeOffRenderMonthViewModel model)
        {
            var transTypeId = int.Parse(model.TransType);
           var transDefEntity= timeAideWindowContext.tTransDef.Find(transTypeId);

            IEnumerable<tPunchDate> punchDateList = getTransDateDetail(model.ReferenceIds);

            var transDates=punchDateList.OrderBy(o=>o.DTPunchDate).Select(s => s.DTPunchDate).ToArray();
            var dailyBalances = ValidateTransactionDailyBalances(new EmployeeTimeOffRequest() { EmployeeId = model.EmployeeId, AccrualType = model.AccrualType,TransType= transDefEntity.Name, StartDate = transDates.Min(), EndDate = transDates.Max(), DayHours = model.TimeOffDayHours }, transDates);
            if(model.AccrualType==model.CurrentAccrualType && model.AccrualType != "NO")
            {
                dailyBalances[0].OpeningBalance += model.CurrentDayHours;
                dailyBalances[0].Available += model.CurrentDayHours;
            }
            var retVal = CheckIsValidTransactionDailyBalance(dailyBalances);
            ViewBag.RenderStatus = retVal.Status;
            ViewBag.RenderMessage = retVal.Message;
            return PartialView("TransactionDateDetail", dailyBalances);
        }
        private decimal getDayHours(int employeeId)
        {
            var userIdParameter = new SqlParameter("@UserID", employeeId);
            var searchDateParameter = new SqlParameter("@SearchDate", DateTime.Today);
            decimal dayHours = timeAideWindowContext.Database.SqlQuery<decimal>("SELECT dbo.fnSS_GetHoursInDay(@UserID,@SearchDate)", userIdParameter, searchDateParameter).FirstOrDefault();
            return dayHours;
        }
        private IEnumerable<tPunchDate> getTransDateDetail(string ids)
        {
            IEnumerable<tPunchDate> punchDates = null;
            var selectedIds = ids.Split(',');
            if (selectedIds.Length > 0)
            {
                punchDates = timeAideWindowContext.tPunchDates.Where(w => selectedIds.Contains(w.tpdID.ToString()))
                                                                   .OrderBy(o=>o.DTPunchDate)
                                                                   .ToList();
            }
            return punchDates;
        }
        private IEnumerable<tPunchData> getPunchDateDetail(int empid, DateTime punchDate)
        {
            IEnumerable<tPunchData> punchDetail = null;

            punchDetail = timeAideWindowContext.tPunchData.Where(w => w.e_id== empid && w.DTPunchDate== punchDate && w.sModType!="d")
                                                                   .OrderBy(o => o.DTPunchDateTime)
                                                                   .ToList();
           
            return punchDetail;
        }
        private IEnumerable<TimesheetEditorTransactionViewModel> getDeleteTransOfDateList(string ids)
        {
            IEnumerable<TimesheetEditorTransactionViewModel> transList = null;
            var selectedIds = ids.Split(',');
            if (selectedIds.Length > 0)
            {
                var punchDatesList = timeAideWindowContext.tPunchDates.Where(w => selectedIds.Contains(w.tpdID.ToString()));

                transList = punchDatesList.Join(timeAideWindowContext.tPunchPair,
                 pDate => new { empId = pDate.e_id, punchDate = pDate.DTPunchDate, nWeekId = pDate.nWeekID },     //Condition from outer table
                  pPair => new { empId = pPair.e_id??0, punchDate = pPair.DTPunchDate, nWeekId = pPair.nWeekID },          //Condition from inner table
                  (pDate, pPair) => new TimesheetEditorTransactionViewModel
                  {
                      tppID = pPair.tppID,
                      e_id = pPair.e_id,
                      e_name = pPair.e_name,
                      DTPunchDate = pPair.DTPunchDate,
                      HoursWorked = pPair.HoursWorked,
                      sType = pPair.sType,
                      sTDesc = pPair.sTDesc,
                      pCode = pPair.pCode,
                      b_Processed = pPair.b_Processed,
                      bTrans = pPair.bTrans,
                      nWeekID = pPair.nWeekID
                  }).Where(w=>w.bTrans==true && w.b_Processed==true );

                transList = transList.Join(timeAideWindowContext.tTransDef,
                    trnLst => trnLst.sType,
                    trndef => trndef.Name,
                    (trnLst, trndef) => new TimesheetEditorTransactionViewModel
                    {
                        tppID = trnLst.tppID,
                        e_id = trnLst.e_id,
                        e_name = trnLst.e_name,
                        DTPunchDate = trnLst.DTPunchDate,
                        HoursWorked = trnLst.HoursWorked,
                        sType = trnLst.sType,
                        sTDesc = trnLst.sTDesc,
                        pCode = trnLst.pCode,
                        b_Processed = trnLst.b_Processed,
                        bTrans = trnLst.bTrans,
                        nWeekID = trnLst.nWeekID,
                        nIsMoneyTrans = trndef.nIsMoneyTrans

                    }
                    ).ToList();


            }
            return transList;
        }
        private IList<UserDailyBalanceViewModel> ValidateTransactionDailyBalances(EmployeeTimeOffRequest model, DateTime?[] transDates)
        {
            var userDailyBalances = DataHelper.spSS_TSTransactionDailyBalancesWeb<UserDailyBalanceViewModel>(timeAideWindowContext, model.EmployeeId.Value, model.AccrualType,model.TransType, model.StartDate.Value, model.EndDate.Value)
                                                .Where(w=> transDates.Contains(w.BalanceDate)).ToList< UserDailyBalanceViewModel>();
            decimal initBalance = 0;
            int counter = 0;
            
           
            foreach (var dailyBalance in userDailyBalances)
            {
                if (!transDates.Contains(dailyBalance.BalanceDate))
                {
                    continue;
                }
                    if (model.AccrualType == "NO")
                {
                    dailyBalance.OpeningBalance = 0;
                    dailyBalance.Available = 0;
                    continue;
                }
                dailyBalance.OpeningBalance = dailyBalance.Balance - (model.DayHours.Value * counter);
                dailyBalance.Available = dailyBalance.Balance - (model.DayHours.Value * (counter + 1));
                counter++;
            }
            return userDailyBalances;
        }
        private dynamic CheckIsValidTransactionDailyBalance(IList<UserDailyBalanceViewModel> dailyBalances)
        {
            var status = "Success";
            var message = "";

            var invalidBalance = dailyBalances.Where(w => w.Available < 0).FirstOrDefault();
            if (invalidBalance != null)
            {
                status = "Error";
                message = $"There is insufficient available balance({Math.Round(invalidBalance.Available, 2)}) for transaction date({invalidBalance.BalanceDate.ToString("MMM dd")})";
            }
            return new { Status = status, Message = message };
        }
        public JsonResult SaveTransaction(TimeOffRenderMonthViewModel model)
        {
            string status = "Success";
            string message = "Transaction is saved successfully!";
            Int64 nWeekId = 0;     
            using (var requestSavingTimeAideWindowDBTrans = timeAideWindowContext.Database.BeginTransaction())
            {
                try
                {
                    var result=DataHelper.spSS_AddTimesheeTransactionFromWeb<TransactionResultViewModel>(timeAideWindowContext, model.ReferenceIds, model.TransType, model.TimeOffDayHours, model.TransNote);
                                         
                    
                    if (result != null)
                    {
                        var auditEntryList = new List<tAuditInfo>();
                        foreach (var transactionResult in result)
                        {
                            var auditEntry = new tAuditInfo();
                            auditEntry.DTTimeStamp = DateTime.Now;
                            auditEntry.nAdminID = SessionHelper.LoginEmployeeId;
                            auditEntry.sAdminName = SessionHelper.LoginEmployeeName;
                            auditEntry.sAdminAction = "Added";
                            auditEntry.sRecordAffected = "Transactions";
                            auditEntry.nUserIDAffected = transactionResult.e_id;
                            auditEntry.sUserNameAffected = transactionResult.e_name;
                            auditEntry.sFieldName = transactionResult.sType;
                            auditEntry.NewValue = $"Add:{transactionResult.DTPunchDate:MM/dd/yy} {transactionResult.sType}={transactionResult.HoursWorked}";
                            auditEntry.sNote = model.TransNote;
                            auditEntryList.Add(auditEntry);
                        }
                        if (auditEntryList.Count > 0)
                        {
                            DataHelper.spSS_InsertAuditInfoFromWeb(timeAideWindowContext, auditEntryList);
                        }
                        timeAideWindowContext.SaveChanges();
                        //timesheet recompute by calling service.
                        requestSavingTimeAideWindowDBTrans.Commit();
                        recomputeTimesheetData(model.EmployeeId);
                        //requestSavingTimeAideWindowDBTrans.Commit();
                        nWeekId = result[0].nWeekID;
                    }
                    else
                    {
                        throw new Exception("Error while inserting data in tpunchpair");
                    }
                   
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                   
                    requestSavingTimeAideWindowDBTrans.Rollback();
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message,nWeekId= nWeekId });
        }
        private dynamic recomputeTimesheetData( int empId,DateTime? startTime=null, DateTime? endTime = null)
        {
            var status = "Success";
            var message = "Timesheet is computed successfully";
            TimeAideContext db = new TimeAideContext();
            var configuration = db.GetAllByCompany<WebPunchConfiguration>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).FirstOrDefault();
            if (configuration != null)
            {

                WebPunchLoginService.wsLogin loginService = new WebPunchLoginService.wsLogin(configuration.PunchServiceUrl + "wsLogin.asmx");
                WebPunchLoginService.clsLoginResult loginServiceResult = loginService.Login(configuration.APIKey, configuration.PunchServiceCompanyPassword);
                if (loginServiceResult.intLoginResult == 1)
                {
                    string eventStartTime = startTime==null?DateTime.Now.ToString("MM/dd/yyyy"): startTime.Value.ToString("MM/dd/yyyy");
                    string eventEndTime = endTime == null ? DateTime.Now.ToString("MM/dd/yyyy") : endTime.Value.ToString("MM/dd/yyyy");
                    WebPunchComputeService2.wsComputeTimeSheet computeService = new WebPunchComputeService2.wsComputeTimeSheet(configuration.PunchServiceUrl + "wsComputeTimeSheet.asmx");
                    WebPunchComputeService2.clsComputeEventResult computeEventResult = computeService.ComputeEvent(loginServiceResult.strSessionToken, empId.ToString(), eventStartTime, eventEndTime);
                }
                else
                {
                    throw new Exception($"There is an error while generating session token({loginServiceResult.strLoginResult}), please contact system admin.");
                }

            }
            else
            {
                throw new Exception("Service configuration is not available.");
            }
            return true;
          }       
    }
}
