using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Reports
{
    //Report's Group
    public enum TimeAide_REPORT_GROUPS
    { RGP_EMPLOYEES = 0,
      RGP_HIRING_INFO,
      RGP_EMPLOYMENT_INFO,
      RGP_PAY_INFO,
      RGP_OSHA
    }
    //Reports
    public enum TimeAide_REPORTS
    {
        REP_EMPLOYEE_COMPLETE = 0     /*int value = 0*/,
        REP_EMPLOYEE_CONTACT      /*int value = 1*/,
        REP_EMPLOYEE_ADDRESS      /*int value = 2*/,
        REP_EMPLOYEE_HEADCOUNT /*int value = 3*/,
        REP_EMPLOYEE_BIRTHDAY /*int value = 4*/,
        REP_EMPLOYEE_BIRTHDATE /*int value = 5*/,
        REP_HIRING_CURRENT,
        REP_HIRING_HISTORY,       
        REP_HIRING_TERMINATION,
        REP_EMPLOYMENT_CURRENT,
        REP_EMPLOYMENT_HISTORY,
        REP_PAYINFO_CURRENT,
        REP_PAYINFO_HISTORY,
        REP_HIRING_NEW,
        REP_EMPLOYEE_EDU,
        REP_EMPLOYEE_TR,
        REP_EMPLOYEE_PREVIEW,
        REP_EMPLOYEE_DEPENDENT,
        REP_EMPLOYEE_CREDENTIAL,
        REP_EMPLOYEE_CUSTOMFIELD,
        REP_EMPLOYEE_BENEFIT,
        REP_EMPLOYEE_ACTION,
        REP_EMPLOYEE_HEALTHINSURANCE,
        REP_EMPLOYEE_DENTALINSURANCE,
        REP_OSHA_300,
        REP_OSHA_300A,
        REP_OSHA_301,
        REP_EMP_SELFSERVICESCH,
        REP_EMP_ATTENDANCEMAINSCH,
        REP_EMP_TIMESHEET,
        REP_EMP_PAYSTUB,
        REP_EMP_TAE,
    }
    //Report's Filter
    public enum TimeAide_REPORT_FILTERS
    {
        
        RF_NONE = 0,
        RF_ALL,
        RF_DATE_RANGE,
        RF_COMPANY,
        RF_DEPARTMENTS,
        RF_SUBDEPARTMENTS,       
        RF_EMPLOYEETYPES,
        RF_EMPLOYMENTTYPES,      
        RF_POSITIONS,
        RF_EMPLOYEESTATUS,
        RF_DEGREES,
        RF_TRAININGS,
        RF_TRAININGTYPES,
        RF_CREDENTIALS,
        RF_CUSTOMFIELDS,
        RF_BENEFITS,
        RF_ACTIONTYPES,
        RF_SUPERVIORS,
        RF_REPORTYEAR,
        RF_LOCATION,
        RF_AVGEMPLOYEE,
        RF_TOTEMPLOYEEHOURS,
        RF_EMPLOYMENTSTATUS,
        RF_CUSTOMFIELDTYPES,
        RF_LOCATIONS,

    }
    //Static Class for reports detail in application
    public static class TimeAideReports
    {
        public const string TimeAide_REPORT_PATH = "/Report Project1/";
        public static string GetReportPath(TimeAide_REPORTS rpt)
        {
            return TimeAide_REPORT_PATH + "" + ReportFileNames[rpt];
        }
       

        #region Report Groups
        /// <summary>
        /// Report Groups
        /// </summary>

        public static readonly IDictionary<TimeAide_REPORT_GROUPS, string> ReportGroups = new ReadOnlyDictionary<TimeAide_REPORT_GROUPS, string>(new Dictionary<TimeAide_REPORT_GROUPS, string>
        {
            {TimeAide_REPORT_GROUPS.RGP_EMPLOYEES, "Employee General"},
            {TimeAide_REPORT_GROUPS.RGP_HIRING_INFO, "Hiring Info."},
             {TimeAide_REPORT_GROUPS.RGP_EMPLOYMENT_INFO, "Employment Info."},
               {TimeAide_REPORT_GROUPS.RGP_PAY_INFO, "Pay Info."},
                {TimeAide_REPORT_GROUPS.RGP_OSHA, "OSHA Reports"},


        });
        #endregion
        #region Report Names
        /// <summary>
        /// Report Names
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {
            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE, "Employee Birthdate"},
            {TimeAide_REPORTS.REP_EMPLOYEE_COMPLETE, "Employee Complete"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CONTACT, "Employee Contact"},
            {TimeAide_REPORTS.REP_EMPLOYEE_ADDRESS, "Employee Address"},
            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY, "Employee Birthday"},
            {TimeAide_REPORTS.REP_EMPLOYEE_HEADCOUNT, "Employee Head Count"},
            {TimeAide_REPORTS.REP_EMPLOYEE_EDU, "Employee Education"},
            {TimeAide_REPORTS.REP_EMPLOYEE_TR, "Employee Training"},
            {TimeAide_REPORTS.REP_EMPLOYEE_PREVIEW, "Employee Performance"},
            {TimeAide_REPORTS.REP_EMPLOYEE_DEPENDENT, "Employee Dependent"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL, "Employee Credential"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD, "Employee CustomField"},
            {TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT, "Employee Benefit"},
            {TimeAide_REPORTS.REP_EMPLOYEE_ACTION, "Employee Action"},
            {TimeAide_REPORTS.REP_EMPLOYEE_HEALTHINSURANCE, "Emp. Health Insurance"},
             {TimeAide_REPORTS.REP_EMPLOYEE_DENTALINSURANCE, "Emp. Dental Insurance"},

            {TimeAide_REPORTS.REP_HIRING_CURRENT, "Hiring List - Current"},
            {TimeAide_REPORTS.REP_HIRING_NEW, "Hiring List - New"},            
            {TimeAide_REPORTS.REP_HIRING_HISTORY, "Hiring List - Historical"},            
            {TimeAide_REPORTS.REP_HIRING_TERMINATION, "Terminated Employee(s)"},

            {TimeAide_REPORTS.REP_EMPLOYMENT_CURRENT, "Employment List - Current"},
            {TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY, "Employment List - History"},

            { TimeAide_REPORTS.REP_PAYINFO_CURRENT, "Pay Info. List - Current"},
            {TimeAide_REPORTS.REP_PAYINFO_HISTORY, "Pay Info. List - History"},

             {TimeAide_REPORTS.REP_OSHA_300, "OSHA 300 Form"},
             {TimeAide_REPORTS.REP_OSHA_300A, "OSHA 300A Form"},
             
        });
        #endregion
        #region Report File Names
        /// <summary>
        /// List of the report file that goes with the tagged report.
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportFileNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {
            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE, "EmployeeBirthdateRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_COMPLETE, "EmployeeCompleteRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CONTACT, "EmployeeContactRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_ADDRESS, "EmployeeAddressRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY, "EmployeeBirthdayRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_HEADCOUNT, "EmployeeHeadCountRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_EDU, "EmployeeEducationListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_TR, "EmployeeTrainingListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_PREVIEW, "EmployeePerformanceReviewListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_DEPENDENT, "EmployeeDependentListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL, "EmployeeCredentialListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD, "EmployeeCustomFieldListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT, "EmployeeBenefitListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_ACTION, "EmployeeActionListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_HEALTHINSURANCE, "EmployeeHealthInsuranceListRpt"},
            {TimeAide_REPORTS.REP_EMPLOYEE_DENTALINSURANCE, "EmployeeDentalInsuranceListRpt"},

            {TimeAide_REPORTS.REP_HIRING_CURRENT, "EmployeeHiringListCurrentRpt"},
            { TimeAide_REPORTS.REP_HIRING_NEW, "EmployeeHiringListNewRpt"},
            {TimeAide_REPORTS.REP_HIRING_HISTORY, "EmployeeHiringListHistoryRpt"},            
            {TimeAide_REPORTS.REP_HIRING_TERMINATION, "EmployeeHiringListTerminatedRpt"},

            {TimeAide_REPORTS.REP_EMPLOYMENT_CURRENT, "EmployeeEmploymentListCurrentRpt"},
            {TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY, "EmployeeEmploymentListHistoryRpt"},

            {TimeAide_REPORTS.REP_PAYINFO_CURRENT, "EmployeePayInfoListCurrentRpt"},
            {TimeAide_REPORTS.REP_PAYINFO_HISTORY, "EmployeePayInfoListHistoryRpt"},

            {TimeAide_REPORTS.REP_OSHA_300, "OSHA300Rpt"},
            {TimeAide_REPORTS.REP_OSHA_300A, "OSHA300ARpt"},
            {TimeAide_REPORTS.REP_OSHA_301, "OSHA301Rpt"},
            {TimeAide_REPORTS.REP_EMP_SELFSERVICESCH, "SelfServiceScheduleRpt.rdlc"},
            {TimeAide_REPORTS.REP_EMP_ATTENDANCEMAINSCH, "AttendanceMainScheduleRpt.rdlc"},
            {TimeAide_REPORTS.REP_EMP_TIMESHEET, "EmployeeTimesheetRpt.rdlc"},
            {TimeAide_REPORTS.REP_EMP_PAYSTUB, "EmployeePayStubRpt.rdlc"},
             {TimeAide_REPORTS.REP_EMP_TAE, "EmployeeTAERpt.rdlc"},

        });
        #endregion
        #region Report Filter Names
        public static readonly IDictionary<TimeAide_REPORT_FILTERS, string> ReportFilterNames = new ReadOnlyDictionary<TimeAide_REPORT_FILTERS, string>(new Dictionary<TimeAide_REPORT_FILTERS, string>
        {
            
            { TimeAide_REPORT_FILTERS.RF_NONE, "No Filters for this Report."},
            { TimeAide_REPORT_FILTERS.RF_ALL, "All Filters for this Report."},
            {TimeAide_REPORT_FILTERS.RF_DATE_RANGE, "Date Range"},
            {TimeAide_REPORT_FILTERS.RF_COMPANY, "Company"},
            {TimeAide_REPORT_FILTERS.RF_DEPARTMENTS, "Department(s)"},
            {TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS, "Sub-Department(s)"},
            {TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES, "Employee Type(s)"},
            {TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES, "Employment Type(s)"},
            {TimeAide_REPORT_FILTERS.RF_POSITIONS, "Position(s)"},
            {TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS, "Employee Status"},
            {TimeAide_REPORT_FILTERS.RF_DEGREES, "Degree(s)"},
            {TimeAide_REPORT_FILTERS.RF_TRAININGS, "Training(s)"},
            {TimeAide_REPORT_FILTERS.RF_CUSTOMFIELDS, "CustomField(s)"},
            {TimeAide_REPORT_FILTERS.RF_BENEFITS, "Benefit(s)"},
            {TimeAide_REPORT_FILTERS.RF_ACTIONTYPES, "Action Type(s)"},
            {TimeAide_REPORT_FILTERS.RF_SUPERVIORS, "Supervior"},
            {TimeAide_REPORT_FILTERS.RF_REPORTYEAR, "Report Year"},
            {TimeAide_REPORT_FILTERS.RF_LOCATION, "Location"},
            {TimeAide_REPORT_FILTERS.RF_AVGEMPLOYEE, "Avg. No Employee"},
            {TimeAide_REPORT_FILTERS.RF_TOTEMPLOYEEHOURS, "Total Employee Hours"},
            });
        #endregion
        #region Report DataTable
        /// <summary>
        /// Report DataTable list
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORTS, string> ReportDataTableNames = new ReadOnlyDictionary<TimeAide_REPORTS, string>(new Dictionary<TimeAide_REPORTS, string>
        {
            {TimeAide_REPORTS.REP_EMP_SELFSERVICESCH, "EmployeeAttendanceScheduleDS"},
            {TimeAide_REPORTS.REP_EMP_ATTENDANCEMAINSCH, "EmployeeAttendanceScheduleDS"},
            {TimeAide_REPORTS.REP_EMP_TIMESHEET, "EmployeeTimesheetDS"},
            {TimeAide_REPORTS.REP_EMP_PAYSTUB, "PayStubBatchDS,PayStubCompanyInfoDS,PayStubCompensationDS,PayStubWithholdingDS"},
            {TimeAide_REPORTS.REP_EMP_TAE, "EmployeeTAEDS"}
        });
        #endregion

        #region Report Menu
        /// <summary>
        /// Report Menu Layout
        /// </summary>
        public static readonly IDictionary<TimeAide_REPORT_GROUPS, Dictionary<TimeAide_REPORTS, bool>> ReportMenu = new ReadOnlyDictionary<TimeAide_REPORT_GROUPS, Dictionary<TimeAide_REPORTS, bool>>(new Dictionary<TimeAide_REPORT_GROUPS, Dictionary<TimeAide_REPORTS, bool>>
        {
            {TimeAide_REPORT_GROUPS.RGP_EMPLOYEES,
                (new  Dictionary<TimeAide_REPORTS, bool>
                {
                   
                            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_COMPLETE, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_CONTACT, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_ADDRESS, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_HEADCOUNT, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_EDU, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_TR, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_PREVIEW, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_DEPENDENT, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_ACTION, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_HEALTHINSURANCE, true},
                            {TimeAide_REPORTS.REP_EMPLOYEE_DENTALINSURANCE, true}
                })
            },
             {TimeAide_REPORT_GROUPS.RGP_HIRING_INFO,
                (new  Dictionary<TimeAide_REPORTS, bool>
                {

                            {TimeAide_REPORTS.REP_HIRING_CURRENT, true},
                            {TimeAide_REPORTS.REP_HIRING_NEW, true},
                            {TimeAide_REPORTS.REP_HIRING_HISTORY, true},                            
                            {TimeAide_REPORTS.REP_HIRING_TERMINATION, true}                          


                })
            },
             
             {TimeAide_REPORT_GROUPS.RGP_EMPLOYMENT_INFO,
                (new  Dictionary<TimeAide_REPORTS, bool>
                {

                            {TimeAide_REPORTS.REP_EMPLOYMENT_CURRENT, true},
                            {TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY, true}                           
                            
                })
            },
            {TimeAide_REPORT_GROUPS.RGP_PAY_INFO,
                (new  Dictionary<TimeAide_REPORTS, bool>
                {

                            {TimeAide_REPORTS.REP_PAYINFO_CURRENT, true},
                            {TimeAide_REPORTS.REP_PAYINFO_HISTORY, true}

                })
            },
             {TimeAide_REPORT_GROUPS.RGP_OSHA,
                (new  Dictionary<TimeAide_REPORTS, bool>
                {

                            {TimeAide_REPORTS.REP_OSHA_300, true},
                             {TimeAide_REPORTS.REP_OSHA_300A, true}
                })
            }
        });
        #endregion
        #region Get Report Menu Tree
        /// <summary>
        /// Report Menu Tree
        /// </summary>
        public static IList<TimeAideReportMenu> GetReportMenuData()
        {
            IList<TimeAideReport> menuList = new List<TimeAideReport>();
            foreach (TimeAide_REPORT_GROUPS rgp in Enum.GetValues(typeof(TimeAide_REPORT_GROUPS)))
            {
                var gpReports = ReportMenu.Where(w => w.Key == rgp).FirstOrDefault();
                foreach (var rp in gpReports.Value)
                {
                    TimeAide_REPORTS r = rp.Key;
                    menuList.Add(new TimeAideReport(r, rgp));
                }
            }

            IList<TimeAideReportMenu> rptMenu = menuList.OrderBy(o => o.RptGroupId)
                                                        .OrderBy(o => o.RptId).GroupBy(g => g.RptGroupName)
                                                        .Select(s => new TimeAideReportMenu { GroupName = s.Key, GroupReports = s })
                                                        .ToList();

            return rptMenu;
        }
        #endregion

        #region  Group Reports Assignments
        public static readonly IDictionary<TimeAide_REPORTS, TimeAide_REPORT_GROUPS> ReportGroupAssignments = new ReadOnlyDictionary< TimeAide_REPORTS, TimeAide_REPORT_GROUPS>(new Dictionary< TimeAide_REPORTS, TimeAide_REPORT_GROUPS>
        {
             {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_ADDRESS,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_CONTACT,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_HEADCOUNT,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_COMPLETE,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_EDU,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_TR,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_PREVIEW,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_DEPENDENT,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_ACTION,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_HEALTHINSURANCE,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},
             {TimeAide_REPORTS.REP_EMPLOYEE_DENTALINSURANCE,TimeAide_REPORT_GROUPS.RGP_EMPLOYEES},

             {TimeAide_REPORTS.REP_HIRING_CURRENT,TimeAide_REPORT_GROUPS.RGP_HIRING_INFO},
             {TimeAide_REPORTS.REP_HIRING_NEW,TimeAide_REPORT_GROUPS.RGP_HIRING_INFO},
             {TimeAide_REPORTS.REP_HIRING_HISTORY,TimeAide_REPORT_GROUPS.RGP_HIRING_INFO},           
             {TimeAide_REPORTS.REP_HIRING_TERMINATION,TimeAide_REPORT_GROUPS.RGP_HIRING_INFO},

             {TimeAide_REPORTS.REP_EMPLOYMENT_CURRENT,TimeAide_REPORT_GROUPS.RGP_EMPLOYMENT_INFO},
             {TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY,TimeAide_REPORT_GROUPS.RGP_EMPLOYMENT_INFO},

             {TimeAide_REPORTS.REP_PAYINFO_CURRENT,TimeAide_REPORT_GROUPS.RGP_PAY_INFO},
             {TimeAide_REPORTS.REP_PAYINFO_HISTORY,TimeAide_REPORT_GROUPS.RGP_PAY_INFO},

             {TimeAide_REPORTS.REP_OSHA_300,TimeAide_REPORT_GROUPS.RGP_OSHA},
             {TimeAide_REPORTS.REP_OSHA_300A,TimeAide_REPORT_GROUPS.RGP_OSHA}

        });
        #endregion
        #region Report Filter Assignments
        public static readonly IDictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>> ReportFilterAssignments = new ReadOnlyDictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>>(new Dictionary<TimeAide_REPORTS, List<TimeAide_REPORT_FILTERS>>
        {
            {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDATE, (new List<TimeAide_REPORT_FILTERS>
            {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
             {TimeAide_REPORTS.REP_EMPLOYEE_ADDRESS, (new List<TimeAide_REPORT_FILTERS>
              {

                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            },
             {TimeAide_REPORTS.REP_EMPLOYEE_BIRTHDAY, (new List<TimeAide_REPORT_FILTERS>
              {

                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
              {TimeAide_REPORTS.REP_EMPLOYEE_COMPLETE, (new List<TimeAide_REPORT_FILTERS>
              {
               
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
              {TimeAide_REPORTS.REP_EMPLOYEE_CONTACT, (new List<TimeAide_REPORT_FILTERS>
              {

                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
              {TimeAide_REPORTS.REP_EMPLOYEE_HEADCOUNT, (new List<TimeAide_REPORT_FILTERS>
              {

                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
               {TimeAide_REPORTS.REP_EMPLOYEE_EDU, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_DEGREES,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
            {TimeAide_REPORTS.REP_EMPLOYEE_TR, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_TRAININGS,
                TimeAide_REPORT_FILTERS.RF_TRAININGTYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_PREVIEW, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_DEPENDENT, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_CREDENTIAL, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_CREDENTIALS,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_CUSTOMFIELD, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_CUSTOMFIELDS,
                TimeAide_REPORT_FILTERS.RF_CUSTOMFIELDTYPES,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_BENEFIT, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_BENEFITS,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_ACTION, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_ACTIONTYPES,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_HEALTHINSURANCE, (new List<TimeAide_REPORT_FILTERS>
              {
               
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
            {TimeAide_REPORTS.REP_EMPLOYEE_DENTALINSURANCE, (new List<TimeAide_REPORT_FILTERS>
              {

                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS
             })
            }
            ,
              {TimeAide_REPORTS.REP_HIRING_CURRENT, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
               {TimeAide_REPORTS.REP_HIRING_NEW, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
               {TimeAide_REPORTS.REP_HIRING_HISTORY, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
             {TimeAide_REPORTS.REP_HIRING_TERMINATION, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
             {TimeAide_REPORTS.REP_EMPLOYMENT_CURRENT, (new List<TimeAide_REPORT_FILTERS>
              {
               // TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
             {TimeAide_REPORTS.REP_EMPLOYMENT_HISTORY, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            }
            ,
             {TimeAide_REPORTS.REP_PAYINFO_CURRENT, (new List<TimeAide_REPORT_FILTERS>
              {
                //TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
              {TimeAide_REPORTS.REP_PAYINFO_HISTORY, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_DATE_RANGE,
                TimeAide_REPORT_FILTERS.RF_DEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_SUBDEPARTMENTS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEETYPES,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTTYPES,
                TimeAide_REPORT_FILTERS.RF_POSITIONS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYEESTATUS,
                TimeAide_REPORT_FILTERS.RF_SUPERVIORS,
                TimeAide_REPORT_FILTERS.RF_EMPLOYMENTSTATUS,
                TimeAide_REPORT_FILTERS.RF_LOCATIONS

             })
            },
              {TimeAide_REPORTS.REP_OSHA_300, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_REPORTYEAR,
                TimeAide_REPORT_FILTERS.RF_LOCATION                

             })
            },
              {TimeAide_REPORTS.REP_OSHA_300A, (new List<TimeAide_REPORT_FILTERS>
              {
                TimeAide_REPORT_FILTERS.RF_REPORTYEAR,
                TimeAide_REPORT_FILTERS.RF_LOCATION,
                TimeAide_REPORT_FILTERS.RF_AVGEMPLOYEE,
                TimeAide_REPORT_FILTERS.RF_TOTEMPLOYEEHOURS
             })
            }

        });
        #endregion
    }
    

    //Report Class for get report detail
    public class TimeAideReport
    {
       public TimeAide_REPORTS RptId { get; set; }
       public TimeAide_REPORT_GROUPS RptGroupId { get; set; }
        public string RptName { get; set; }
        public string RptGroupName { get; set; }
        public string RptFileName { get; set; }
        public string RptFilePath { get; set; }

        public TimeAideReport(TimeAide_REPORTS rptId, TimeAide_REPORT_GROUPS rptGroupId)
        {
            RptId = rptId;
            RptGroupId = rptGroupId;
            RptName = TimeAideReports.ReportNames[rptId];
            RptGroupName = TimeAideReports.ReportGroups[rptGroupId];
            RptFileName = TimeAideReports.ReportFileNames[rptId];
            RptFilePath = TimeAideReports.GetReportPath(rptId);
        }

        
    }
    public class TimeAideReportMenu
    {
        public string GroupName { get; set; }
        public IEnumerable<TimeAideReport> GroupReports { get; set; }
    }
 }
