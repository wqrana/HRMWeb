using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Reports;

namespace TimeAide.Web.ViewModel
{
    public class ReportViewModel
    {
        public int? ReportId { get; set; }
        public string ReportGroupName { get; set; }
        public string ReportName { get; set; }
        public int? ReportCriteriaTemplateId { get; set; }
        public int? CriteriaType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int? IncidentId { get; set; }
        public int? ReportYear { get; set; }
        public int? CompanyId { get; set; }
        public int? SuperviorId { get; set; }
        public int? LocationId { get; set; }
        public int? AverageNoOfEmployees { get; set; }
        public int? TotalHoursByEmployees { get; set; }
        public int? AttendanceSchId { get; set; }
        public string EmployeeSelectionIds { get; set; }
        public string DepartmentSelectionIds { get; set; }
        public string SubDepartmentSelectionIds { get; set; }
        public string EmployeeTypeSelectionIds { get; set; }
        public string EmploymentTypeSelectionIds { get; set; }
        public string EmploymentStatusSelectionIds { get; set; }
        public string PositionSelectionIds { get; set; }
        public string StatusSelectionIds { get; set; }
        public string DegreeSelectionIds { get; set; }
        public string TrainingSelectionIds { get; set; }
        public string TrainingTypeSelectionIds { get; set; }
        public string CredentialSelectionIds { get; set; }
        public string CustomFieldSelectionIds { get; set; }
        public string CustomFieldTypeSelectionIds { get; set; }
        public string BenefitSelectionIds { get; set; }
        public string ActionTypeSelectionIds { get; set; }
        public string LocationSelectionIds { get; set; }
        public ShowHideReportsFilters ShowHideReportsFilters { get; set; }
        public IEnumerable<dynamic> ReportCriteriaTemplateList { get; set; }
        public IEnumerable<dynamic> EmployeeStatusList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> SubDepartmentList { get; set; }
        public IEnumerable<dynamic> CompanyList { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeList { get; set; }
        public IEnumerable<SelectListItem> EmploymentTypeList { get; set; }
        public IEnumerable<SelectListItem> EmploymentStatusList { get; set; }
        public IEnumerable<SelectListItem> PositionList { get; set; }
        public IEnumerable<SelectListItem> DegreeList { get; set; }
        public IEnumerable<SelectListItem> TrainingList { get; set; }
        public IEnumerable<SelectListItem> TrainingTypeList { get; set; }
        public IEnumerable<SelectListItem> CredentialList { get; set; }
        public IEnumerable<SelectListItem> CustomFieldList { get; set; }
        public IEnumerable<SelectListItem> CustomFieldTypeList { get; set; }        
        public IEnumerable<SelectListItem> BenefitList { get; set; }
        public IEnumerable<SelectListItem> ActionTypeList { get; set; }
        public IEnumerable<SelectListItem> LocationsList { get; set; }
        public IEnumerable<dynamic> SuperviorList { get; set; }
        public IEnumerable<dynamic> LocationList { get; set; }
        public IEnumerable<dynamic> JobCertificationTemplateList { get; set; }
        public IEnumerable<dynamic> JobCertificationSigneeList { get; set; }
    }

    public class ShowHideReportsFilters
    {
        private int ReportId { get; set; }

        public ShowHideReportsFilters(int? RptId)
        {
            if (RptId == null)
            {
                ReportId = -1;
            }
            else if (RptId.HasValue)
            {
                ReportId = (int)RptId;
            }          
        }
        public bool ShowDateRange
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_DATE_RANGE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowLocations
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_LOCATIONS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowDepartments
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_DEPARTMENTS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowSubDepartments
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowEmployeeTypes
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowEmploymentTypes
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowEmploymentStatus
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowPositions
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_POSITIONS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowEmployeeStatus
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowDegrees
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_DEGREES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowTrainings
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_TRAININGS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowCredentials
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_CREDENTIALS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowCustomFields
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_CUSTOMFIELDS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowBenefits
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_BENEFITS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowActionTypes
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_ACTIONTYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowSuperviors
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_SUPERVIORS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowReportYear
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_REPORTYEAR))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowLocation
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_LOCATION))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowAvgNoOfEmployees
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_AVGEMPLOYEE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowTotalHoursByEmployee
        {
            get
            {
                if (TimeAideReports.ReportFilterAssignments[((TimeAide_REPORTS)ReportId)].Exists(element => element == TimeAide_REPORT_FILTERS.RF_TOTEMPLOYEEHOURS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}