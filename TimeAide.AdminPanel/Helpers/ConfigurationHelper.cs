using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TimeAide.AdminPanel.Helpers
{
    public class ConfigurationHelper
    {
        // private constructor
        private ConfigurationHelper()
        {
            //Property1 = "default value";
        }
        private static string _ExecutionType;
        public static string ExecutionType
        {
            get
            {
                if (String.IsNullOrEmpty(_ExecutionType))
                    _ExecutionType = ConfigurationManager.AppSettings["ExecutionType"];

                return _ExecutionType;
            }
            set
            {
                _ExecutionType = value;
            }
        }
        private static string _SourceDatabase;
        private static bool _IsSourceDatabaseAssigned = false;
        public static string SourceDatabase
        {
            get
            {
                if (String.IsNullOrEmpty(_SourceDatabase) && !_IsSourceDatabaseAssigned)
                    _SourceDatabase = ConfigurationManager.AppSettings["SourceDatabase"];

                return _SourceDatabase;
            }
            set
            {
                _SourceDatabase = value;
                _IsSourceDatabaseAssigned = true;
            }
        }
        private static string _TA7ExportDatabase;
        public static string TA7ExportDatabase
        {
            get
            {
                if (String.IsNullOrEmpty(_TA7ExportDatabase))
                    _TA7ExportDatabase = ConfigurationManager.AppSettings["TA7_ExportDatabase"]??"";

                return _TA7ExportDatabase;
            }
            set
            {
                _TA7ExportDatabase = value;                
            }
        }
        private static string _TAWExportDatabase;
        public static string TAWExportDatabase
        {
            get
            {
                if (String.IsNullOrEmpty(_TAWExportDatabase))
                    _TAWExportDatabase = ConfigurationManager.AppSettings["TAW_ExportDatabase"] ?? "";

                return _TAWExportDatabase;
            }
            set
            {
                _TAWExportDatabase = value;
            }
        }
        private static string _ClientName;
        public static string ClientName
        {
            get
            {
                if (String.IsNullOrEmpty(_ClientName))
                    _ClientName = ConfigurationManager.AppSettings["ClientName"];
                return _ClientName;
            }
            set
            {
                _ClientName = value;
            }
        }

        private static string _DefaultShortFullName;
        public static string DefaultShortFullName
        {
            get
            {
                if (String.IsNullOrEmpty(_DefaultShortFullName))
                    _DefaultShortFullName = ConfigurationManager.AppSettings["DefaultShortFullName"];
                return _DefaultShortFullName;
            }
            set
            {
                _DefaultShortFullName = value;
            }
        }

        private static string _DefaultAdminEmail;
        public static string DefaultAdminEmail
        {
            get
            {
                if (String.IsNullOrEmpty(_DefaultAdminEmail))
                    _DefaultAdminEmail = ConfigurationManager.AppSettings["DefaultAdminEmail"];
                return _DefaultAdminEmail;
            }
            set
            {
                _DefaultAdminEmail = value;
            }
        }

        private static string _FilesDownloadPath;
        public static string FilesDownloadPath
        {
            get
            {
                if (String.IsNullOrEmpty(_FilesDownloadPath))
                    _FilesDownloadPath = ConfigurationManager.AppSettings["FilesDownloadPath"];
                return _FilesDownloadPath;
            }
            set
            {
                _FilesDownloadPath = value;
            }
        }
        private static string _AppRootPath;
        public static string AppRootPath
        {
            get
            {
                if (String.IsNullOrEmpty(_AppRootPath))
                    _AppRootPath = ConfigurationManager.AppSettings["AppRootPath"];
                return _AppRootPath;
            }
            set
            {
                _AppRootPath = value;
            }
        }
        private static string _connectionString;
        public static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                    _connectionString = ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString;
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
        public static readonly IDictionary<ImportDataType, string> ImportDataTypeNames = new ReadOnlyDictionary<ImportDataType, string>(new Dictionary<ImportDataType, string>
        {
            {ImportDataType.PayrollTA7, "Payroll (TA7)"}
            
        });
        public static readonly IDictionary<ExportDataType, string> ExportDataTypeNames = new ReadOnlyDictionary<ExportDataType, string>(new Dictionary<ExportDataType, string>
        {
            {ExportDataType.AllFiles, "All Files"},
            {ExportDataType.CompanySetUpTA7, "Company SetUp(TA7)"},
            {ExportDataType.MasterfileTAW, "Master file(TAW)"},
             {ExportDataType.MasterfileTA7, "Master file(TA7)"},
            {ExportDataType.PositionInfoTAW, "Position Info(TAW)"},
            {ExportDataType.TaxInfoTA7, "Tax Info(TA7)"},
            {ExportDataType.AdditionalEarningsInfoTA7, "Additional Earnings Info(TA7)"},
            {ExportDataType.DeductionsGoalsTA7, "Deductions Goals(TA7)"},
            {ExportDataType.DirectDepositTA7, "Direct Deposit(TA7)"},
            {ExportDataType.EmergencyContactTAW, "Emergency Contact(TAW)"},
             {ExportDataType.AllowedAndTakenTA7, "Allowed And Taken(TA7)"},
             {ExportDataType.BalancesTA7, "Balances(TA7)"},
             {ExportDataType.WFNEmployeesTA7, "WFN Employees(TA7)"},
             {ExportDataType.WFNDirectDepositTA7, "WFN Direct Deposit(TA7)"},
             {ExportDataType.WFNBalancesTA7, "WFN Balances(TA7)"},
             {ExportDataType.WFNVAnSABalancesTA7, "WFN VA & SA Balances(TA7)"}
        });
        public static readonly IDictionary<ExportDataType, string> ExportDataDBPrefix = new ReadOnlyDictionary<ExportDataType, string>(new Dictionary<ExportDataType, string>
        {
            {ExportDataType.CompanySetUpTA7, "TA7"},
            {ExportDataType.MasterfileTAW, "TAW"},
            {ExportDataType.MasterfileTA7, "TA7"},
            {ExportDataType.PositionInfoTAW, "TAW"},
            {ExportDataType.TaxInfoTA7, "TA7"},
            {ExportDataType.AdditionalEarningsInfoTA7, "TA7"},
            {ExportDataType.DeductionsGoalsTA7, "TA7"},
            {ExportDataType.DirectDepositTA7, "TA7" },
            {ExportDataType.EmergencyContactTAW, "TAW" },
            {ExportDataType.AllowedAndTakenTA7, "TA7" },
            {ExportDataType.BalancesTA7, "TA7" },
            {ExportDataType.WFNEmployeesTA7, "TA7" },
            {ExportDataType.WFNDirectDepositTA7, "TA7" },
            {ExportDataType.WFNBalancesTA7, "TA7" },
            {ExportDataType.WFNVAnSABalancesTA7, "TA7" }

        });
        public static readonly IDictionary<ExportDataType, string> ExportDataDBProc = new ReadOnlyDictionary<ExportDataType, string>(new Dictionary<ExportDataType, string>
        {
            {ExportDataType.CompanySetUpTA7, "spED_CompanySetupInfo"},
            {ExportDataType.MasterfileTAW, "spED_EmployeeMasterfile"},
            {ExportDataType.MasterfileTA7, "spED_UserMasterfile"},
            {ExportDataType.PositionInfoTAW, "spED_EmployeePositionInfo"},
            {ExportDataType.TaxInfoTA7, "spED_UserTaxInfo"},
            {ExportDataType.AdditionalEarningsInfoTA7, "spED_UserAdditionalEarningsInfo"},
            {ExportDataType.DeductionsGoalsTA7, "spED_UserDeductionsAndGoals"},
            {ExportDataType.DirectDepositTA7, "spED_UserDirectDeposit"},
            {ExportDataType.EmergencyContactTAW, "spED_EmployeeEmergencyContact"},
            {ExportDataType.AllowedAndTakenTA7, "spED_UserAllowedAndTakenInfo"},
            {ExportDataType.BalancesTA7, "spED_UserBalancesInfo"},
            {ExportDataType.WFNEmployeesTA7, "spED_WFNUsersInfo"},
            {ExportDataType.WFNDirectDepositTA7, "spED_WFNEmployeeDirectDeposit"},
            {ExportDataType.WFNBalancesTA7, "spED_WFNBalancesInfo"},
            {ExportDataType.WFNVAnSABalancesTA7, "spED_WFNUserVAAndSABalances"}
        });
    }
}
