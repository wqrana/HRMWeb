using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Reports;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class ReportsController : TimeAideWebBaseControllers
    {
        private TimeAideContext db = new TimeAideContext();

        // GET: Reports
        public ActionResult Index(int? id)
        {
            ReportViewModel model = new ReportViewModel();

            if (id.HasValue)
            {
                ViewBag.ShowFilterByEmployee = true;
                ViewBag.ReportType = "Employee";

                model.ReportId = id;
                var rptId = (TimeAide_REPORTS)id;

                if (rptId == TimeAide_REPORTS.REP_OSHA_300 || rptId == TimeAide_REPORTS.REP_OSHA_300A)
                {
                    ViewBag.ShowFilterByEmployee = false;
                    ViewBag.ReportType = "OSHA";
                    model.ReportYear = DateTime.Today.Year;
                    model.AverageNoOfEmployees = 0;
                    model.TotalHoursByEmployees = 0;
                }
                model.ReportId = id;
                model.ShowHideReportsFilters = new ShowHideReportsFilters(id);
                model.ReportName = TimeAideReports.ReportNames[rptId];
                model.ReportGroupName = TimeAideReports.ReportGroups[TimeAideReports.ReportGroupAssignments[rptId]].ToString();

                model.ReportCriteriaTemplateList = db.GetAllByCompany<ReportCriteriaTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                                 .Where(w => w.ReportId == id).Select(s => new { id = s.Id, text = s.ReportCriteriaTemplateName });

                model.CompanyList = db.Company.Select(s => new { id = s.Id, text = s.CompanyName });
                model.LocationsList = db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.LocationName }).OrderBy(o => o.Text);

                model.SuperviorList = EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.CompanyId == SessionHelper.SelectedCompanyId && w.Id != SessionHelper.LoginId).Select(s => new { id = s.Id, text = s.ShortFullName }).OrderBy(o => o.text);
                // model.DepartmentList = db.GetAll<Department>(SessionHelper.SelectedClientId).Select(s => new SelectListItem {Value = s.Id.ToString(), Text = s.DepartmentName });
                model.DepartmentList = db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.DepartmentName }).OrderBy(o => o.Text);
                //model.SubDepartmentList = db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SubDepartmentName });
                model.SubDepartmentList = SubDepartmentService.SubDepartments().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SubDepartmentName }).OrderBy(o => o.Text);
                //model.SubDepartmentList= SubDepartmentService.SubDepartments(null).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SubDepartmentName });
                model.EmployeeTypeList = db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.EmployeeTypeName }).OrderBy(o => o.Text); ;
                model.EmploymentTypeList = db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.EmploymentTypeName }).OrderBy(o => o.Text) ;
                model.EmploymentStatusList = db.GetAll<EmploymentStatus>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.EmploymentStatusName }).OrderBy(o => o.Text);

                model.PositionList = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.PositionName }).OrderBy(o => o.Text);
                model.EmployeeStatusList = db.GetAll<EmployeeStatus>().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.EmployeeStatusName }).OrderBy(o => o.Text);
                model.DegreeList = db.GetAll<Degree>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.DegreeName }).OrderBy(o => o.Text);

                model.TrainingList = db.GetAll<Training>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.TrainingName }).OrderBy(o => o.Text);
                model.TrainingTypeList = db.GetAllByCompany<TrainingType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.TrainingTypeName }).OrderBy(o => o.Text);

                model.CredentialList = db.GetAll<Credential>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CredentialName }).OrderBy(o => o.Text);

                model.CustomFieldList = db.GetAllByCompany<CustomField>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CustomFieldName }).OrderBy(o => o.Text);
                model.CustomFieldTypeList = db.GetAll<CustomFieldType>().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.CustomFieldTypeName }).OrderBy(o => o.Text);

                model.BenefitList = db.GetAll<Benefit>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.BenefitName }).OrderBy(o => o.Text);
                model.ActionTypeList = db.GetAll<ActionType>(SessionHelper.SelectedClientId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.ActionTypeName }).OrderBy(o => o.Text);
              
                model.LocationList = db.GetAllByCompany<Location>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.LocationName }).OrderBy(o => o.text);
            }
            return View(model);
        }
        public JsonResult GetCompanySubDeptByDept(string departmentIds)
        {
            IEnumerable<SelectListItem> subDepartmentsRet = null;
            var subDepartments = db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
            if (departmentIds != "")
            {
                var selectedDeptList = departmentIds.Split(',').ToArray();
                subDepartmentsRet = subDepartments
                      .Where(c => !c.DepartmentId.HasValue || selectedDeptList.Contains((c.DepartmentId ?? 0).ToString()))
                      .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SubDepartmentName }).ToList();
            }
            else
            {
                subDepartmentsRet = subDepartments.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SubDepartmentName }).ToList();
            }
            JsonResult jsonResult = new JsonResult()
            {
                Data = subDepartmentsRet,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public ActionResult AdhocReportPopup(ReportViewModel reportViewModel)
        {
            var rptId = (TimeAide_REPORTS)reportViewModel.ReportId;
            reportViewModel.CompanyId = SessionHelper.SelectedCompanyId;
            reportViewModel.FromDate = DateTime.Today.AddYears(-50);
            reportViewModel.ToDate = DateTime.Today;
            return PartialView(reportViewModel);
        }
        public PartialViewResult JobCertificatePrintView(ReportViewModel reportViewModel)
        {
            ViewBag.PrintView = "Not Found";

            try
            {
                var userInformationId = reportViewModel.AttendanceSchId ?? 0;
                var signeeId = reportViewModel.SuperviorId ?? 0;
                var templateId = reportViewModel.ReportId ?? 0;
                var templateBody = JobCertificationPlaceHolder(userInformationId, signeeId, templateId, reportViewModel.FromDate ?? DateTime.Now);
                ViewBag.PrintView = templateBody;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "Report", "JobCertificatePrintView");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                // Console.WriteLine(ex.Message);
            }
            return PartialView();
        }
        public ActionResult TerminationLetterPopup(int id, string openType)
        {
            return GetTerminationLetterPopup(id, openType);
        }
        public ActionResult JobCertificationPopup(int id, string openType)
        {
            return GetJobCertificationPopup(id, openType);
        }

        public ActionResult SelfServicejobCertification(int? id)
        {

            return GetJobCertificationPopup(id??SessionHelper.LoginId, "2");
        }

        private ActionResult GetJobCertificationPopup(int id, string openType)
        {
            var reportViewModel = new ReportViewModel();
            var userInformation = db.SP_UserInformationById<UserInformation>(id);
            var dept = db.Department.Find(userInformation.DepartmentId ?? 0);
            var cmp = db.Company.Find(userInformation.CompanyId);
            reportViewModel.EmployeeSelectionIds = userInformation.EmployeeId.ToString();
            reportViewModel.AttendanceSchId = userInformation.Id;
            reportViewModel.CriteriaType = openType == null ? 1 : 2;
            int? selectedTemplateId = null;
            int? selectedSigneeId = null;
            selectedTemplateId = cmp.DefaultLetterTemplateId;
            selectedSigneeId = cmp.DefaultLetterSigneeId;
            reportViewModel.SuperviorId = selectedSigneeId;
            if (dept != null)
            {
                reportViewModel.DepartmentSelectionIds = dept.Id.ToString();
                if (dept.JobCertificationSignee != null)
                {
                    reportViewModel.SuperviorId = dept.JobCertificationSignee.Id;
                    reportViewModel.ActionTypeSelectionIds = dept.JobCertificationSignee.Name;

                }
                if (dept.JobCertificationTemplate != null)
                {
                    selectedTemplateId = dept.JobCertificationTemplate.Id;
                }
            }
            reportViewModel.JobCertificationTemplateList = new SelectList(db.GetAllByCompany<JobCertificationTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.TemplateTypeId == null|| w.TemplateTypeId==1), "Id", "Name", selectedTemplateId);
            reportViewModel.JobCertificationSigneeList = new SelectList(db.GetAllByCompany<JobCertificationSignee>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "Name", reportViewModel.SuperviorId);
            return PartialView("JobCertificationPopup", reportViewModel);
        }
        private ActionResult GetTerminationLetterPopup(int id, string openType)
        {
            var reportViewModel = new ReportViewModel();
            var userInformation = db.SP_UserInformationById<UserInformation>(id);
            var dept = db.Department.Find(userInformation.DepartmentId ?? 0);
            reportViewModel.EmployeeSelectionIds = userInformation.EmployeeId.ToString();
            reportViewModel.AttendanceSchId = userInformation.Id;
            reportViewModel.CriteriaType = openType == null ? 1 : 2;
            int? selectedTemplateId = null;
            if (dept != null)
            {
                reportViewModel.DepartmentSelectionIds = dept.Id.ToString();
                if (dept.JobCertificationSignee != null)
                {
                    reportViewModel.SuperviorId = dept.JobCertificationSignee.Id;
                    reportViewModel.ActionTypeSelectionIds = dept.JobCertificationSignee.Name;

                }
               
            }
            reportViewModel.JobCertificationTemplateList = new SelectList(db.GetAllByCompany<JobCertificationTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w =>  w.TemplateTypeId == 2), "Id", "Name", selectedTemplateId);
            reportViewModel.JobCertificationSigneeList = new SelectList(db.GetAllByCompany<JobCertificationSignee>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "Name", reportViewModel.SuperviorId);
            return PartialView("TerminationLetterPopup", reportViewModel);
        }
        private string JobCertificationPlaceHolder(int userId, int signeeId, int templateId,DateTime effectiveDate)
        {
            //var userInfo = db.SP_UserInformationById<UserInformation>(userId);
            var userInfo = db.SP_UserInformationWithEmploymentById<UserInformationViewModel>(userId);
            var company = db.Company.Find(userInfo.CompanyId);
            var companyLogo = company.CompanyLogo;
            string companyLogoStr = "";
            string signatureStr = "";
            if (companyLogo != null && companyLogo.Length > 0)
            {
                var base64 = Convert.ToBase64String(companyLogo);
                companyLogoStr = string.Format("data:image/jpg;base64,{0}", base64);
            }
            var signee = db.JobCertificationSignee.Find(signeeId);
            if (signee.Signature != null && signee.Signature.Length > 0)
            {
                var base64 = Convert.ToBase64String(signee.Signature);
                signatureStr = string.Format("data:image/jpg;base64,{0}", base64);
            }
            var employmentId = userInfo.EmploymentId == null ? 0 : userInfo.EmploymentId;
            var employmentHistoryId = userInfo.EmploymentHistoryId == null ? 0 : userInfo.EmploymentHistoryId;
            var payInformationHistoryId = userInfo.PayInformationHistoryId == null ? 0 : userInfo.PayInformationHistoryId;
            /*
            var employmentHiringEntity = db.Employment.Where(w => w.UserInformationId == userInfo.Id && w.DataEntryStatus == 1 && w.TerminationDate == null)
                                                        .OrderByDescending(o => o.Id).FirstOrDefault();
            var employmentId = employmentHiringEntity == null ? 0 : employmentHiringEntity.Id;
            var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == userInfo.Id && w.DataEntryStatus == 1 && w.EmploymentId == employmentId && w.EndDate == null)
                                                        .OrderByDescending(o => o.StartDate).FirstOrDefault();
            var payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == userInfo.Id && w.DataEntryStatus == 1 && w.EmploymentId == employmentId && w.EndDate == null)
                                                        .OrderByDescending(o => o.StartDate).FirstOrDefault();
           */
            var employmentHiringEntity = db.Employment.Find(employmentId);
            var employmentHistoryEntity = db.EmploymentHistory.Find(employmentHistoryId);
            var payInfoHistoryEntity = db.PayInformationHistory.Find(payInformationHistoryId);

            var template = db.JobCertificationTemplate.Find(templateId);
            string templateBody = template.TemplateBody;
            var imagesizeStyle = "style = 'max-width:150px;max-height:100px;'";
            string employeeId = userInfo.EmployeeId.ToString();
            string last4OfSSN = userInfo.SSNEnd;
            string firstName = userInfo.FirstName;
            string middleIntial = userInfo.MiddleInitial;
            string firstLastName = userInfo.FirstLastName;
            string secondLastName = userInfo.SecondLastName;
            string fullName = userInfo.ShortFullName;
            string companyName = company.CompanyName;
            DateTime? hireDateDefualt = employmentHiringEntity != null ? employmentHiringEntity.OriginalHireDate : null;
            DateTime? terminationDateDefault = employmentHiringEntity != null ? employmentHiringEntity.TerminationDate : null;
            string hireDate = hireDateDefualt!= null? hireDateDefualt.Value.ToString("MMMM dd,yyyy") :"";
            string hireDateSpa = hireDateDefualt != null ? DateToSpanishFormat.GetDate(hireDateDefualt.Value): "";

            string terminationDate = terminationDateDefault != null ? terminationDateDefault.Value.ToString("MMMM dd,yyyy") : "";
            string terminationDateSpa = terminationDateDefault != null ? DateToSpanishFormat.GetDate(terminationDateDefault.Value) : "";

            string location = employmentHistoryEntity != null ? employmentHistoryEntity.Location != null ? employmentHistoryEntity.Location.LocationName : "" : "";
            string department = employmentHistoryEntity!=null? employmentHistoryEntity.Department.DepartmentName:"";
            
            string subDept = employmentHistoryEntity != null ? employmentHistoryEntity.SubDepartment!=null? employmentHistoryEntity.SubDepartment.SubDepartmentName:"" : ""; 
            string employeeType = employmentHistoryEntity != null ? employmentHistoryEntity.EmployeeType!=null?employmentHistoryEntity.EmployeeType.EmployeeTypeName:"":"";
            string employmentType = employmentHistoryEntity != null ? employmentHistoryEntity.EmploymentType != null ? employmentHistoryEntity.EmploymentType.EmploymentTypeName : "" : "";
            string employmentStatus = employmentHiringEntity != null ? employmentHiringEntity.EmploymentStatus!=null? employmentHiringEntity.EmploymentStatus.EmploymentStatusName:"":"" ;
            string position = employmentHistoryEntity != null ? employmentHistoryEntity.Position!=null? employmentHistoryEntity.Position.PositionName:"":"";
            string payRate = payInfoHistoryEntity!=null? payInfoHistoryEntity.RateAmount.ToString("F2") :"";
            string payFrequency = payInfoHistoryEntity != null ? payInfoHistoryEntity.PayFrequency!=null? payInfoHistoryEntity.PayFrequency.PayFrequencyName:"":"";
            string periodHours = payInfoHistoryEntity != null ? (payInfoHistoryEntity.PeriodHours??0).ToString() : "";
            string commissionRate = payInfoHistoryEntity != null ? (payInfoHistoryEntity.CommRateAmount??0).ToString("F2") : "";
            string signeeName = signee.Name;
            string signeePosition = signee.Position;            
            templateBody = templateBody.Replace("[CompanyLogo]","<image src='"+ companyLogoStr+"' "+ imagesizeStyle);
            templateBody = templateBody.Replace("[Signature]", "<image src='" + signatureStr + "' "+ imagesizeStyle);

            templateBody = templateBody.Replace("[EmployeeId]", employeeId);
            templateBody = templateBody.Replace("[FirstName]", firstName);
            templateBody = templateBody.Replace("[MiddleInitial]", middleIntial);
            templateBody = templateBody.Replace("[FirstLastName]", firstLastName);
            templateBody = templateBody.Replace("[SecondLastName]", secondLastName);
            templateBody = templateBody.Replace("[ShortFullName]", fullName);
            templateBody = templateBody.Replace("[Last4OfSSN]", last4OfSSN);
            templateBody = templateBody.Replace("[CompanyName]", companyName);
            templateBody = templateBody.Replace("[HireDate]", hireDate);
            templateBody = templateBody.Replace("[HireDateSpa]", hireDateSpa);
            templateBody = templateBody.Replace("[TerminationDate]", terminationDate);
            templateBody = templateBody.Replace("[TerminationDateSpa]", terminationDateSpa);
            templateBody = templateBody.Replace("[Location]", location);
            templateBody = templateBody.Replace("[Department]", department);
            templateBody = templateBody.Replace("[SubDepartment]", subDept);
            templateBody = templateBody.Replace("[EmployeeType]", employeeType);            
            templateBody = templateBody.Replace("[EmploymentType]", employmentType);
            templateBody = templateBody.Replace("[EmploymentStatus]", employmentStatus);
            templateBody = templateBody.Replace("[Position]", position);
            templateBody = templateBody.Replace("[PayRate]", payRate);
            templateBody = templateBody.Replace("[PayFrequency]", payFrequency);
            templateBody = templateBody.Replace("[PeriodHours]", periodHours);
            templateBody = templateBody.Replace("[CommissionRate]", commissionRate);
            templateBody = templateBody.Replace("[SigneeName]", signeeName);
            templateBody = templateBody.Replace("[SigneePosition]", signeePosition);
            templateBody = templateBody.Replace("[EffectiveDate]", effectiveDate.ToString("MMMM dd,yyyy"));
            templateBody = templateBody.Replace("[EffectiveDateSpa]", DateToSpanishFormat.GetDate(effectiveDate));
            return templateBody;
        }
        public PartialViewResult ReportView(ReportViewModel reportViewModel)
        {
            //SSRS Server Reports
            ReportViewer reportViewer;
            try
            {
                reportViewer = GetReportView(reportViewModel);
                ViewBag.ReportView = reportViewer;


            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "Report", "ReportView");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                // Console.WriteLine(ex.Message);
            }
            return PartialView("ReportView");
        }
        public JsonResult ReportViewAsDownload(ReportViewModel reportViewModel)
        {
            string status = "Success";
            string message = "";
            string downloadFile = "";
            //SSRS Server Reports
            ReportViewer reportViewer;
            try
            {

                reportViewer = GetReportView(reportViewModel);
                var rptId = (TimeAide_REPORTS)reportViewModel.ReportId;
                var rptFileName = TimeAideReports.ReportFileNames[rptId];
                downloadFile = SaveReportAsPDF(rptFileName, reportViewer);

            }
            catch (Exception ex)
            {
                //HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "Report", "ReportView");
                //return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                // Console.WriteLine(ex.Message);
                status = "Error";
                message = ex.Message;
            }
            var retResult = new { status = status, message = message, downloadFile = downloadFile };
            return Json(retResult);
        }
        public PartialViewResult LocalReportView(ReportViewModel reportViewModel)
        {
            //Local Microsoft RDLC report
            var rptId = (TimeAide_REPORTS)reportViewModel.ReportId;
            var rptFileName = TimeAideReports.ReportFileNames[rptId];
            var rptDataTableName = TimeAideReports.ReportDataTableNames[rptId];
            var reportpath = "Reports" + "/" + rptFileName;
            ReportViewer rv = new ReportViewer();
            DataTable reportDataTable = null;
            rv.ProcessingMode = ProcessingMode.Local;
           // rv.SizeToReportContent = true;
            rv.Width = Unit.Percentage(100);
            rv.AsyncRendering = false;
            ReportDataHelper reptDataHelper = new ReportDataHelper();
            if (rptId == TimeAide_REPORTS.REP_EMP_SELFSERVICESCH)
            {
                if (reportViewModel.ReportCriteriaTemplateId == null) // Get TImeAide7 schedule
                {
                    reportDataTable = ToDataTable(reptDataHelper.getSelfServiceAttendanceRptData(reportViewModel.AttendanceSchId.Value), rptDataTableName);
                }
                else //get web based schedule data
                {
                    reportDataTable = ToDataTable(reptDataHelper.getSelfServiceWebWebAttendanceSchRptData(reportViewModel.AttendanceSchId.Value,db), rptDataTableName);
                }
             }
            if (rptId == TimeAide_REPORTS.REP_EMP_ATTENDANCEMAINSCH)
            {
                if (reportViewModel.ReportCriteriaTemplateId == null) // Get TImeAide7 schedule
                {
                    reportDataTable = ToDataTable(reptDataHelper.getAttendanceScheduleMainRptData(reportViewModel.EmployeeSelectionIds), rptDataTableName);
                }
                else //Get web based schedule data
                {
                    reportDataTable = ToDataTable(reptDataHelper.getAttendanceWebScheduleMainRptData(reportViewModel.EmployeeSelectionIds,db), rptDataTableName);
                }
            }
            
            if (rptId == TimeAide_REPORTS.REP_EMP_TIMESHEET)
            {
                reportDataTable = ToDataTable(reptDataHelper.getEmployeeTimesheetRptData(reportViewModel.EmployeeSelectionIds), rptDataTableName);

            }
            if (rptId == TimeAide_REPORTS.REP_EMP_PAYSTUB)
            {
                var batchId = reportViewModel.EmployeeSelectionIds;
                var companyLogo = db.Company.Where(w => w.Id == SessionHelper.SelectedCompanyId).Select(s => s.CompanyLogo).FirstOrDefault();
                var payStubData = reptDataHelper.getEmployeePayStubRptData(reportViewModel.EmployeeSelectionIds, reportViewModel.AttendanceSchId ?? 0);
                payStubData.EmployeePayStubCompany.CompanyLogo = companyLogo;
                var reportDataTableNames = rptDataTableName.Split(',');
                if (reportDataTableNames.Length > 1)
                {
                    foreach (var dataTableName in reportDataTableNames)
                    {
                        switch (dataTableName)
                        {
                            case "PayStubBatchDS":
                                rptDataTableName = "PayStubBatchDS";
                                var employeePayBatchData = new List<EmployeePayStubBatch>();
                                employeePayBatchData.Add(payStubData.EmployeePayStubBatch);
                                reportDataTable = ToDataTable(employeePayBatchData, rptDataTableName);
                                break;
                            case "PayStubCompanyInfoDS":
                                var rptDataTableName1 = "PayStubCompanyInfoDS";
                                var employeeCompanyInfoData = new List<EmployeePayStubCompany>();
                                employeeCompanyInfoData.Add(payStubData.EmployeePayStubCompany);
                                var reportDataTable1 = ToDataTable(employeeCompanyInfoData, rptDataTableName1);
                                rv.LocalReport.DataSources.Add(new ReportDataSource(rptDataTableName1, reportDataTable1));
                                break;
                            case "PayStubCompensationDS":
                                var rptDataTableName2 = "PayStubCompensationDS";
                                var employeeCompensationData = payStubData.EmployeePayStubCompensations;
                                var reportDataTable2 = ToDataTable(employeeCompensationData, rptDataTableName2);
                                rv.LocalReport.DataSources.Add(new ReportDataSource(rptDataTableName2, reportDataTable2));
                                break;
                            case "PayStubWithholdingDS":
                                var rptDataTableName3 = "PayStubWithholdingDS";
                                var employeeWithholdingData = payStubData.EmployeePayStubWithholdings;
                                var reportDataTable3 = ToDataTable(employeeWithholdingData, rptDataTableName3);
                                rv.LocalReport.DataSources.Add(new ReportDataSource(rptDataTableName3, reportDataTable3));
                                break;
                        }
                    }
                }
            }
            if (rptId == TimeAide_REPORTS.REP_EMP_TAE)
            {
                reportDataTable = ToDataTable(reptDataHelper.getEmployeeTAERptData(reportViewModel.EmployeeSelectionIds), rptDataTableName);

            }
            // var reportData = rdh.getEmployeeDetailRptData();
            // rv.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmployeeDetailRpt.rdlc";
            rv.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + reportpath;
            // rv.LocalReport.DataSources.Add(new ReportDataSource("DS_UserInformation", dt));
            rv.LocalReport.DataSources.Add(new ReportDataSource(rptDataTableName, reportDataTable));
            ViewBag.ReportView = rv;

            return PartialView("LocalReportView");
        }

        private ReportViewer GetReportView(ReportViewModel reportViewModel)
        {
            SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder(db.Database.Connection.ConnectionString);
            var ssrsConnectionStr = string.Format("Data source={0};initial catalog={1}", connStrBuilder.DataSource, connStrBuilder.InitialCatalog);

            var rptId = (TimeAide_REPORTS)reportViewModel.ReportId;
            var rptFileName = TimeAideReports.ReportFileNames[rptId];
            var reportServerPath = ConfigurationManager.AppSettings["ReportServiceUrl"];
            var reportServerFolder = ConfigurationManager.AppSettings["ReportServiceRootFolder"];
            reportServerFolder = reportServerFolder == "" ? "TimeAideReportServer" : reportServerFolder;
            //var reportpath = "/TimeAideReportServer/"+ rptFileName;
            var reportpath = "/" + reportServerFolder + "/" + rptFileName;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Remote;
            reportviewer.SizeToReportContent = true;
            reportviewer.AsyncRendering = false;
            // reportviewer.ServerReport.ReportServerUrl = new Uri("http://waqarq:8080/ReportServer");
            reportviewer.ServerReport.ReportServerUrl = new Uri(reportServerPath);
            reportviewer.ServerReport.ReportPath = reportpath;
            // reportviewer.ServerReport.ReportPath = "/Report Project1/Report1";
            //setting report parameters
            List<ReportParameter> reportParms = new List<ReportParameter>();
            reportParms.Add(new ReportParameter("connectionString", ssrsConnectionStr));
            reportParms.Add(new ReportParameter("employeeIds", reportViewModel.EmployeeSelectionIds == null ? "" : reportViewModel.EmployeeSelectionIds));
            reportParms.Add(new ReportParameter("superviserId", reportViewModel.SuperviorId == null ? SessionHelper.LoginId.ToString() : reportViewModel.SuperviorId.ToString()));
            reportParms.Add(new ReportParameter("clientId", SessionHelper.SelectedClientId.ToString()));
            reportParms.Add(new ReportParameter("companyId", reportViewModel.CompanyId == null ? SessionHelper.SelectedCompanyId.ToString() : reportViewModel.CompanyId.ToString()));

            if (rptId == TimeAide_REPORTS.REP_OSHA_300 || rptId == TimeAide_REPORTS.REP_OSHA_300A || rptId == TimeAide_REPORTS.REP_OSHA_301)
            {
                //employeeIncidentId
                if (rptId == TimeAide_REPORTS.REP_OSHA_301)
                {
                    reportParms.Add(new ReportParameter("employeeIncidentId", (reportViewModel.IncidentId ?? 0).ToString()));
                }
                else
                {
                    reportParms.Add(new ReportParameter("reportYear", (reportViewModel.ReportYear ?? 2021).ToString()));
                    reportParms.Add(new ReportParameter("locationId", (reportViewModel.LocationId ?? 0).ToString()));
                    if (rptId == TimeAide_REPORTS.REP_OSHA_300A)
                    {
                        reportParms.Add(new ReportParameter("averageEmployees", (reportViewModel.AverageNoOfEmployees ?? 0).ToString()));
                        reportParms.Add(new ReportParameter("totalHoursWorked", (reportViewModel.TotalHoursByEmployees ?? 0).ToString()));
                    }
                }
            }
            else
            {
                reportParms.Add(new ReportParameter("locationIds", reportViewModel.LocationSelectionIds == null ? "" : reportViewModel.LocationSelectionIds));
                reportParms.Add(new ReportParameter("deportmentIds", reportViewModel.DepartmentSelectionIds == null ? "" : reportViewModel.DepartmentSelectionIds));
                reportParms.Add(new ReportParameter("subDeportmentIds", reportViewModel.SubDepartmentSelectionIds == null ? "" : reportViewModel.SubDepartmentSelectionIds));

                reportParms.Add(new ReportParameter("employeeTypeIds", reportViewModel.EmployeeTypeSelectionIds == null ? "" : reportViewModel.EmployeeTypeSelectionIds));
                reportParms.Add(new ReportParameter("employmentTypeIds", reportViewModel.EmploymentTypeSelectionIds == null ? "" : reportViewModel.EmploymentTypeSelectionIds));
                reportParms.Add(new ReportParameter("employmentStatusIds", reportViewModel.EmploymentStatusSelectionIds == null ? "" : reportViewModel.EmploymentStatusSelectionIds));
                reportParms.Add(new ReportParameter("positionIds", reportViewModel.PositionSelectionIds == null ? "" : reportViewModel.PositionSelectionIds));

                reportParms.Add(new ReportParameter("statusIds", reportViewModel.StatusSelectionIds == null ? "" : reportViewModel.StatusSelectionIds));


                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY || rptId == TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE
                   || rptId == TimeAide_REPORTS.REP_HIRING_HISTORY || rptId == TimeAide_REPORTS.REP_HIRING_NEW
                   || rptId == TimeAide_REPORTS.REP_HIRING_TERMINATION
                   || rptId == TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY || rptId == TimeAide_REPORTS.REP_PAYINFO_HISTORY
                   || rptId == TimeAide_REPORTS.REP_EMPLOYEE_TR
                   )
                {
                    reportParms.Add(new ReportParameter("fromDate", reportViewModel.FromDate.ToString()));
                    reportParms.Add(new ReportParameter("toDate", reportViewModel.ToDate.ToString()));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_EDU)
                {
                    reportParms.Add(new ReportParameter("degreeIds", reportViewModel.DegreeSelectionIds == null ? "" : reportViewModel.DegreeSelectionIds));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_TR)
                {
                    reportParms.Add(new ReportParameter("trainingIds", reportViewModel.TrainingSelectionIds == null ? "" : reportViewModel.TrainingSelectionIds));
                    reportParms.Add(new ReportParameter("trainingTypeIds", reportViewModel.TrainingTypeSelectionIds == null ? "" : reportViewModel.TrainingTypeSelectionIds));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL)
                {
                    reportParms.Add(new ReportParameter("credentialIds", reportViewModel.CredentialSelectionIds == null ? "" : reportViewModel.CredentialSelectionIds));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD)
                {
                    reportParms.Add(new ReportParameter("customFieldIds", reportViewModel.CustomFieldSelectionIds == null ? "" : reportViewModel.CustomFieldSelectionIds));
                    reportParms.Add(new ReportParameter("customFieldTypeIds", reportViewModel.CustomFieldTypeSelectionIds == null ? "" : reportViewModel.CustomFieldTypeSelectionIds));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT)
                {
                    reportParms.Add(new ReportParameter("benefitIds", reportViewModel.BenefitSelectionIds == null ? "" : reportViewModel.BenefitSelectionIds));
                }
                if (rptId == TimeAide_REPORTS.REP_EMPLOYEE_ACTION)
                {
                    reportParms.Add(new ReportParameter("actionTypeIds", reportViewModel.ActionTypeSelectionIds == null ? "" : reportViewModel.ActionTypeSelectionIds));
                }
            }
            reportviewer.ShowParameterPrompts = false;
            reportviewer.ServerReport.SetParameters(reportParms);

            return reportviewer;
        }

        private string SaveReportAsPDF(string reportfileName, ReportViewer viewer)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            string downloadFileName = reportfileName + ".pdf";
            FilePathHelper filePathHelper = new FilePathHelper();
            string serverFilePath = filePathHelper.GetPath("ReportTempFolder", downloadFileName);

            byte[] bytes = viewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            //var exportfile = File(bytes, mimeType, fileName);
            FileStream file =
            default(FileStream);

            file = new FileStream(serverFilePath, FileMode.Create);
            file.Write(bytes, 0, bytes.Length);

            file.Close();
            file.Dispose();

            return downloadFileName;
            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "." + extension);
            //Response.BinaryWrite(bytes); // create the file
            //Response.Flush(); // send it to the client to download
        }

        public ActionResult DownloadReportAsPDF(string downloadFileName)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadFile = null;
            byte[] fileBytes;
            FilePathHelper filePathHelper = new FilePathHelper();
            serverFilePath = filePathHelper.GetPath("ReportTempFolder", downloadFileName);

            if (!string.IsNullOrEmpty(serverFilePath))
            {

                downloadFile = new FileInfo(serverFilePath);

                if (downloadFile.Exists)
                {
                    fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadFile.Name);
                }
            }



            return null;

        }
        public JsonResult CalcOSHA300AValues(ReportViewModel reportViewModel)
        {
            string status = "Success";
            string message = "";
            string dataCount = "";
            //SSRS Server Reports

            try
            {
                int companyId = reportViewModel.CompanyId ?? 0;
                int reportyear = reportViewModel.ReportYear ?? DateTime.Now.Year;
                int locationId = reportViewModel.LocationId ?? 0;
                string calcType = reportViewModel.StatusSelectionIds;
                var queryStr = "Select dbo.fn_calculateOSHAEmployeesOrHours({0},{1},{2},{3})";
                Object[] parameters = { companyId, reportyear, locationId, calcType };
                //Calling scalar value function
                var retval = db.Database.SqlQuery<int>(queryStr, parameters).FirstOrDefault();
                dataCount = retval.ToString();
            }
            catch (Exception ex)
            {
                //HandleErrorInfo handleErrorInfo = new HandleErrorInfo(ex, "Report", "ReportView");
                //return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
                // Console.WriteLine(ex.Message);
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            var retResult = new { status = status, message = message, dataValue = dataCount };
            return Json(retResult);
        }
        public static DataTable ToDataTable<T>(List<T> l_oItems, string dataTableName)
        {
            try
            {
                DataTable oReturn = new DataTable(dataTableName);
                object[] a_oValues;
                int i;
                PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo oProperty in a_oProperties)
                {
                    oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
                }

                foreach (T oItem in l_oItems)
                {
                    a_oValues = new object[a_oProperties.Length];

                    for (i = 0; i < a_oProperties.Length; i++)
                    {
                        a_oValues[i] = a_oProperties[i].GetValue(oItem, null);

                    }

                    oReturn.Rows.Add(a_oValues);
                }

                return oReturn;
            }
            catch (Exception ex)
            {
                //ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ToDataTable");
                throw ex;
            }
        }
        public static Type BaseType(Type oType)
        {
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            else
            {
                return oType;
            }
        }
    }

}
