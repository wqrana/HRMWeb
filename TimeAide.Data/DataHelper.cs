using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using TimeAide.Common.Helpers;
using System.Data;
using TimeAide.Web.Models;
using TimeAide.Models.ViewModel;

namespace TimeAide.Data
{
    public class DataHelper
    {
        private static Dictionary<int, string> _accuralPeriodType = new Dictionary<int, string>()
                        { { 0, "END OF Month" }, { 1, "Unvailable Balances" },{2,"Approval-Based Adjustment" }
                          ,{3,"Daily Based Adjustment" }  };
        private static Dictionary<int, string> _referenceType = new Dictionary<int, string>()
                        { { 0, "CALENDAR" } };
        private static Dictionary<int, string> _accuredResetType = new Dictionary<int, string>()
                        { { 0, "NEVER" }, { 1, "YEARLY" },{2,"JUNE 30 EXCESS RESET" } };
        private static Dictionary<int, string> _accumulationType = new Dictionary<int, string>()
                        { { 0, "FIXED HOURS" }, { 1, "HOURS MULTIPLIER" },{2,"DJ DISCOUNT TRANSACTIONS" },
                            { 3,"ENTRY BASED ADJUSTMENT" } };
        private static Dictionary<int, string> _excessMaxType = new Dictionary<int, string>()
                        { { 0, "NONE" }, { 1, "Monthly Based Excess" },{2,"Calendar Year Excess" },{3,"TBD Excess" } };
        private static Dictionary<int, string> _applicantAnswerType = new Dictionary<int, string>()
                        { { 1, "Yes/No" }, { 2, "Select All that Apply" },{3,"Select (1) that Apply" },{4,"Select Date" },{5,"Open Answer" } };
        private static Dictionary<int, string> _baseScheduleDayofWeek = new Dictionary<int, string>()
                        { { 1, "Sunday" }, { 2, "Monday" },{3,"Tuesday" },{4,"Wednesday" },{5,"Thursday" },{6,"Friday" },{7,"Saturday" } };
        private static Dictionary<string, string> _mediaType = new Dictionary<string, string>()
                        { { "F", "File" }, { "L", "Link" }};
        private static Dictionary<int, string> _timeoffDocumentType = new Dictionary<int, string>()
                        {{ 1, "Request" }, { 2, "Supporting" }};
        private static Dictionary<int, string> _timeoffSubmissionType = new Dictionary<int, string>()
                        {{ 1, "Optional" }, { 2, "Mandatory" }, { 3, "Conditional Mandatory" }};
        private static Dictionary<int, string> _templateType = new Dictionary<int, string>()
                        {{ 1, "Job Certification" }, { 2, "Termination Letter" }};
        public static Dictionary<int, string> AccuralPeriodType { get { return _accuralPeriodType; } }
        public static Dictionary<int, string> AccuralReferenceType { get { return _referenceType; } }
        public static Dictionary<int, string> AccuredResetType { get { return _accuredResetType; } }
        public static Dictionary<int, string> AccuralAccumulationType { get { return _accumulationType; } }
        public static Dictionary<int, string> AccuralExcessMaxType { get { return _excessMaxType; } }
        public static Dictionary<int, string> ApplicantAnswerType { get { return _applicantAnswerType; } }
        public static Dictionary<int, string> BaseScheduleDayOfWeek { get { return _baseScheduleDayofWeek; } }
        public static Dictionary<string, string> MediaType { get { return _mediaType; } }
        public static Dictionary<int, string> TimeOffDocumentType { get { return _timeoffDocumentType; } }
        public static Dictionary<int, string> TimeOffSubmissionType { get { return _timeoffSubmissionType; } }
        public static Dictionary<int, string> TemplateType { get { return _templateType; } }
        private static string ConvertToEFConnectionString(string conStr)
        {
            // Specify the provider name, server and database.
            var providerName = "System.Data.SqlClient";

            //var providerName = "System.Data.EntityClient";

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = conStr;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/TimeAideWindow.csdl|res://*/TimeAideWindow.ssdl|res://*/TimeAideWindow.msl";

            return entityBuilder.ToString();
        }
        public static bool ValidateTAWindowConnection(string connStr)
        {
            bool isValidated = true;
            try
            {
                var efConnStr = ConvertToEFConnectionString(connStr);
                TimeAideWindowContext dbContext = new TimeAideWindowContext(efConnStr);
                dbContext.tusers.Count();
            }
            catch (Exception ex)
            {
                isValidated = false;
            }

            return isValidated;
        }

        public static TimeAideWindowContext GetSelectedClientTAWinEFContext()
        {
            bool IsUseWebConfig = true;
            TimeAideWindowContext dbContext = null;
            var timeAideWebContext = new TimeAideContext();
            var client = timeAideWebContext.Client.Find(SessionHelper.SelectedClientId);
            if (client != null)
            {
                if (client.IsTimeAideWindow)
                {
                    if (!String.IsNullOrEmpty(client.DBServerName) &&
                          !String.IsNullOrEmpty(client.DBName) &&
                          !String.IsNullOrEmpty(client.DBUser) &&
                          !String.IsNullOrEmpty(client.DBPassword))
                    {
                        var connStrDb = string.Format("data source={0};initial catalog={1};User ID={2};Password={3};MultipleActiveResultSets=True;App=EntityFramework", client.DBServerName, client.DBName, client.DBUser, client.DBPassword);
                        var efConnStr = ConvertToEFConnectionString(connStrDb);
                        dbContext = new TimeAideWindowContext(efConnStr);
                        IsUseWebConfig = false;
                    }
                }

            }
            if (IsUseWebConfig && client != null)
            {
                var connStr = ClientDBConnectionHelper.GetClientConnection(client.Id);
                if (connStr != "")
                {
                    var efConnStr = ConvertToEFConnectionString(connStr);
                    dbContext = new TimeAideWindowContext(efConnStr);

                }
            }
            return dbContext;
        }
        public static TimeAideWindowContext GetClientTAWinEFContext(int clientId)
        {
            bool IsUseWebConfig = true;
            TimeAideWindowContext dbContext = null;
            var timeAideWebContext = new TimeAideContext();
            var client = timeAideWebContext.Client.Find(clientId);
            if (client != null)
            {
                if (client.IsTimeAideWindow)
                {
                    if (!String.IsNullOrEmpty(client.DBServerName) &&
                          !String.IsNullOrEmpty(client.DBName) &&
                          !String.IsNullOrEmpty(client.DBUser) &&
                          !String.IsNullOrEmpty(client.DBPassword))
                    {
                        var connStrDb = string.Format("data source={0};initial catalog={1};User ID={2};Password={3};MultipleActiveResultSets=True;App=EntityFramework", client.DBServerName, client.DBName, client.DBUser, client.DBPassword);
                        var efConnStr = ConvertToEFConnectionString(connStrDb);
                        dbContext = new TimeAideWindowContext(efConnStr);
                        IsUseWebConfig = false;
                    }
                }

            }
            if (IsUseWebConfig)
            {
                var connStr = ClientDBConnectionHelper.GetClientConnection(clientId);
                if (connStr != "")
                {
                    var efConnStr = ConvertToEFConnectionString(connStr);
                    dbContext = new TimeAideWindowContext(efConnStr);

                }
            }
            return dbContext;
        }
        public static List<T> SP_GetReportWeek<T>(TimeAideWindowContext dbContext, tReportWeek model) where T : tReportWeek
        {
            if (String.IsNullOrEmpty(model.SelectedEmployeeIds) && model.e_id.HasValue && model.e_id > 0)
                model.SelectedEmployeeIds = model.e_id.ToString();
            StringBuilder query = new StringBuilder("SELECT * FROM tReportWeek Where e_id in (" + (String.IsNullOrEmpty(model.SelectedEmployeeIds) ? "0" : model.SelectedEmployeeIds) + ") ");
            if (model.DTStartDate.HasValue && model.DTEndDate.HasValue)
            {
                //query.Append(" And DTStartDate >='" + model.DTStartDate + "'");
                //query.Append(" And DTStartDate <='" + model.DTEndDate + "'");

                //where DTEndDate >= @STARTDATE AND DTStartDate <= @ENDDATE

                query.Append(" And DTStartDate <='" + model.DTStartDate.Value.ToString("yyyy-MM-dd") + "'");
                query.Append(" And DTEndDate >='" + model.DTEndDate.Value.ToString("yyyy-MM-dd") + "'");
            }
            if (model.e_id.HasValue && model.e_id.Value > 0)
            {
                query.Append(" And e_id = '" + model.e_id + "'");
            }
            if (model.nJobTitleID.HasValue && model.nJobTitleID.Value > 0)
            {
                query.Append(" And nJobTitleID = '" + model.nJobTitleID + "'");
            }
            if (!String.IsNullOrEmpty(model.e_name))
            {
                query.Append(" And e_name like '%" + model.e_name + "%'");
            }
            if (model.nDept.HasValue && model.nDept.Value > 0)
            {
                query.Append(" And nDept = '" + model.nDept + "'");
            }
            if (model.nEmployeeType.HasValue && model.nEmployeeType.Value > 0)
            {
                query.Append(" And nEmployeeType = '" + model.nEmployeeType + "'");
            }
            query.Append("  order by DTStartDate desc ");
            try
            {
                var punchDateList = dbContext.Database
                   .SqlQuery<T>(query.ToString())
                   .ToList<T>();
                return punchDateList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<T> SP_GetTimesheetReportWeek<T>(TimeAideWindowContext dbContext, tReportWeek model) where T : tReportWeek
        {
            StringBuilder query = new StringBuilder("SELECT * FROM tReportWeek Where e_id in (" + (String.IsNullOrEmpty(model.SelectedEmployeeIds) ? "0" : model.SelectedEmployeeIds) + ") ");
            if (model.DTStartDate.HasValue && model.DTEndDate.HasValue)
            {

                //where DTEndDate >= @STARTDATE AND DTStartDate <= @ENDDATE
                //query.Append(" And DTStartDate <='" + model.DTStartDate.Value.ToString("yyyy-MM-dd") + "'");
                //query.Append(" And DTEndDate >='" + model.DTEndDate.Value.ToString("yyyy-MM-dd") + "'");
               
                query.Append(" And DTStartDate <='" + model.DTEndDate.Value.ToString("yyyy-MM-dd") + "'");
                query.Append(" And DTEndDate >='" + model.DTStartDate.Value.ToString("yyyy-MM-dd") + "'");
            }
            if (model.e_id.HasValue && model.e_id.Value > 0)
            {
                query.Append(" And e_id = '" + model.e_id + "'");
            }
            //if (model.nJobTitleID.HasValue && model.nJobTitleID.Value > 0)
            //{
            //    query.Append(" And nJobTitleID = '" + model.nJobTitleID + "'");
            //}
            if (!String.IsNullOrEmpty(model.e_name))
            {
                query.Append(" And e_name like '%" + model.e_name + "%'");
            }
            //if (model.nDept.HasValue && model.nDept.Value > 0)
            //{
            //    query.Append(" And nDept = '" + model.nDept + "'");
            //}
            //if (model.nEmployeeType.HasValue && model.nEmployeeType.Value > 0)
            //{
            //    query.Append(" And nEmployeeType = '" + model.nEmployeeType + "'");
            //}
            if (model.nReviewStatus.HasValue && model.nReviewStatus.Value >-1)
            {
                query.Append(" And nReviewStatus = " + model.nReviewStatus );
            }
            if (model.intUserReviewed > -1)
            {
                query.Append(" And intUserReviewed = " + model.intUserReviewed);
            }
            //query.Append("  order by DTStartDate desc ");
            query.Append("  order by e_id, DTStartDate ");
            try
            {
                var punchDateList = dbContext.Database
                   .SqlQuery<T>(query.ToString())
                   .ToList<T>();
                return punchDateList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static int SP_SelfService_ApproveTimesheet<T>(TimeAideWindowContext dbContext, tReportWeek model, bool isSupervisor) where T : tReportWeek
        {
            StringBuilder query = new StringBuilder("Update tReportWeek Set ");
            if (isSupervisor)
            {
                query.Append("nReviewSupervisorID = " + model.nReviewSupervisorID);
            }
            else
            {
                query.Append("nReviewStatus = 1");
            }
            query.Append(" WHERE(nWeekID = '" + model.nWeekID + "' And e_id = '" + model.e_id + "')");
            try
            {
                int noOfRowUpdated = dbContext.Database.ExecuteSqlCommand(query.ToString());
                return noOfRowUpdated;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static T GetReportWeek<T>(TimeAideWindowContext dbContext, tReportWeek model) where T : tReportWeek
        {
            StringBuilder query = new StringBuilder("SELECT * FROM tReportWeek ");
            query.Append(" WHERE(nWeekID = '" + model.nWeekID + "' And e_id = '" + model.e_id + "')");
            try
            {
                var punchDateList = dbContext.Database
                   .SqlQuery<T>(query.ToString())
                   .ToList<T>();
                return punchDateList.FirstOrDefault();
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<T> SP_GetPunchDate<T>(TimeAideWindowContext dbContext, tPunchDate model) where T : tPunchDate
        {
            try
            {
                //var list = dbContext.tPunchDate.Where(t => t.nWeekID == 1680210501).ToList();
                var punchDateList = dbContext.Database
                   .SqlQuery<T>("SELECT * FROM tPunchDate WHERE (nWeekID = '" + model.nWeekID + "') ORDER BY DTPunchDate")
                     .ToList<T>();
                return punchDateList;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }



        public static List<T> SP_GetUserCompensationDaily<T>(TimeAideWindowContext dbContext, int? employeeId, DateTime? effeciveDate) where T : viewUserCompensationDaily
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(" Select nUserID, dEffectiveDate, sAccrualType, dBalanceStartDate, dblAccruedHours,");
                query.Append(" dblAccrualDailyHours, dblStartBalance, dblAccruedBalance,dblTakenBalance, sAccrualDays,");
                query.Append(" dbo.fnComp_DayAccruedHours(nuserid,saccrualtype,dEffectiveDate,dEffectiveDate ) AS dblAccrualComputedHours,");
                query.Append(" dbo.fnComp_RemainingBalance(nuserid,saccrualtype,dEffectiveDate ) AS dblAvailableBalance");
                query.Append(" From [dbo].[viewUserCompensationDaily]");
                query.Append(" Where nUserID = '" + employeeId + "' and dEffectiveDate = '" + effeciveDate + "'");
                //query.Append(" ORDER BY DTPunchDate");
                var dailyCompensation = dbContext.Database.SqlQuery<T>(query.ToString()).ToList<T>();
                return dailyCompensation;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool fnComp_CheckTimeOffValidation(TimeAideWindowContext dbContext, int employeeId, DateTime timeOffFromDate)
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var fromDateParameter = new SqlParameter("@TimeOffFromDate", timeOffFromDate);
                bool isAllow = dbContext.Database.SqlQuery<bool>("SELECT dbo.fnComp_CheckTimeOffValidation(@UserID,@TimeOffFromDate)", userIdParameter, fromDateParameter).FirstOrDefault();

                return isAllow;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool fnComp_CheckTimeOffValidation(TimeAideWindowContext dbContext, int employeeId, DateTime timeOffFromDate,DateTime timeOffTODate)
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var fromDateParameter = new SqlParameter("@TimeOffFromDate", timeOffFromDate);
                bool isAllow = dbContext.Database.SqlQuery<bool>("SELECT dbo.fnComp_CheckTimeOffValidation(@UserID,@TimeOffFromDate)", userIdParameter, fromDateParameter).FirstOrDefault();

                return isAllow;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<T> spSS_UserScheduleBasedHolidays<T> (TimeAideWindowContext dbContext, int employeeId, DateTime startDate, DateTime endDate) where T: UserScheduledHolidayViewModel
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var stDateParameter = new SqlParameter("@CalendarStartDate", startDate);
                var edDateParameter = new SqlParameter("@CalendarEndDate", endDate);
                var schHolidays = dbContext.Database.SqlQuery<T>("dbo.spSS_UserScheduleHolidays @UserID,@CalendarStartDate,@CalendarEndDate ", userIdParameter, stDateParameter, edDateParameter).ToList();

                return schHolidays;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }        
        public static List<T> spSS_TSTransactionDailyBalancesWeb<T>(TimeAideWindowContext dbContext, int employeeId, string accrualType, string transType, DateTime startDate, DateTime endDate) where T : UserDailyBalanceViewModel
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var accTypeParameter = new SqlParameter("@AccrualType", accrualType);
                var transTypeParameter = new SqlParameter("@TransType", transType);
                var stDateParameter = new SqlParameter("@StartDate", startDate);
                var edDateParameter = new SqlParameter("@EndDate", endDate);
                var dailyBalances = dbContext.Database.SqlQuery<T>("dbo.spSS_TSTransactionDailyBalancesWeb @UserID,@AccrualType,@TransType,@StartDate,@EndDate ", userIdParameter, accTypeParameter, transTypeParameter, stDateParameter, edDateParameter).ToList();

                return dailyBalances;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<TransactionResultViewModel> spSS_AddTimesheeTransactionFromWeb<T>(TimeAideWindowContext dbContext, string referenceIds, string transType, decimal dayHours, string transNote) where T : TransactionResultViewModel
        { bool retVal=false;
            try
            {
                var refIdsParameter = new SqlParameter("@ReferenceIds", referenceIds);               
                var transTypeParameter = new SqlParameter("@TransType", transType);
                var dayHrsParameter = new SqlParameter("@DayHours", dayHours);
                var transNoteParameter = new SqlParameter("@TransNote", transNote);
                var retData=dbContext.Database.SqlQuery<TransactionResultViewModel>("dbo.spSS_AddTimesheeTransactionFromWeb @ReferenceIds,@TransType,@DayHours,@TransNote", refIdsParameter, transTypeParameter, dayHrsParameter, transNoteParameter).ToList();

                return retData;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<TransactionResultViewModel> spSS_DeleteTimesheeTransactionFromWeb<T>(TimeAideWindowContext dbContext, string transactionIds) where T : TransactionResultViewModel
        {
            bool retVal = false;
            try
            { 
                var transIdsParameter = new SqlParameter("@TransactionIds", transactionIds);
               
                var retData = dbContext.Database.SqlQuery<TransactionResultViewModel>("dbo.spSS_DeleteTimesheeTransactionFromWeb @TransactionIds", transIdsParameter).ToList();

                return retData;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool spSS_InsertAuditInfoFromWeb(TimeAideWindowContext dbContext, IEnumerable<tAuditInfo> auditData )
        {
            bool retVal = false;
            try
            {
                dbContext.tAuditInfo.AddRange(auditData);

                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<T> spSS_CompensationDailyBalancesWeb<T>(TimeAideWindowContext dbContext, int employeeId,string accrualType, DateTime startDate, DateTime endDate) where T : UserDailyBalanceViewModel
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var accTypeParameter = new SqlParameter("@AccrualType", accrualType);
                var stDateParameter = new SqlParameter("@StartDate", startDate);
                var edDateParameter = new SqlParameter("@EndDate", endDate);
                var dailyBalances = dbContext.Database.SqlQuery<T>("dbo.spSS_CompensationDailyBalancesWeb @UserID,@AccrualType,@StartDate,@EndDate ", userIdParameter, accTypeParameter, stDateParameter, edDateParameter).ToList();

                return dailyBalances;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<T> spSS_SickInFamilyDailyBalancesWeb<T>(TimeAideWindowContext dbContext, int employeeId, string transType, DateTime startDate, DateTime endDate) where T : UserDailyBalanceViewModel
        {
            try
            {
                var userIdParameter = new SqlParameter("@UserID", employeeId);
                var transTypeParameter = new SqlParameter("@TransType", transType);
                var stDateParameter = new SqlParameter("@StartDate", startDate);
                var edDateParameter = new SqlParameter("@EndDate", endDate);
                var dailyBalances = dbContext.Database.SqlQuery<T>("dbo.spSS_SickInFamilyDailyBalancesWeb @UserID,@TransType,@StartDate,@EndDate ", userIdParameter, transTypeParameter, stDateParameter, edDateParameter).ToList();

                return dailyBalances;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<T> spSS_GetTimeAndEffortDataForWeb<T>(TimeAideWindowContext dbContext, int userId, string userName, DateTime effortDate /*, int deptId, int subDepId,int empTypeId*/) where T : EmployeeTimeAndEffort
        {
            try
            {
                var userIdParameter = new SqlParameter("@userId", userId);
                var userNameParameter = new SqlParameter("@userName", userName??string.Empty);
                var yearParameter = new SqlParameter("@year", effortDate.Year);
                var monthParameter = new SqlParameter("@month", effortDate.Month);
                //var deptIdParameter = new SqlParameter("@deptId", deptId);
                //var subDepIdParameter = new SqlParameter("@subDepId", subDepId);
                //var employeeTypeIdParameter = new SqlParameter("@employeeTypeId", empTypeId);
                //var companyIdParameter = new SqlParameter("@companyId", SessionHelper.SelectedCompanyId);
                var timeAndEffortData = dbContext.Database.SqlQuery<T>("dbo.spSS_GetTimeAndEffortDataForWeb @userId,@userName,@year,@month ", userIdParameter, userNameParameter, yearParameter, monthParameter).ToList();

                return timeAndEffortData;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static TimeAide.Data.tPunchDate GetToday(int? employeeId)
        {
            tReportWeek model = null;
            var todayDte = DateTime.Today;
            //string efConnStr = GetConnectionStting();
            //TimeAideWindowContext timeAideWindowContext = new TimeAideWindowContext(efConnStr);
            var timeAideWindowContext = DataHelper.GetSelectedClientTAWinEFContext();
            var q = timeAideWindowContext.tReportWeeks.Where(w => w.e_id == employeeId &&
                                                        todayDte >= w.DTStartDate && todayDte <= w.DTEndDate);
            model = q.FirstOrDefault();
            if (model != null)
            {
                var q2 = timeAideWindowContext.tPunchDates.Where(w => w.e_id == model.e_id && w.nWeekID == model.nWeekID && w.DTPunchDate == todayDte);
                return q2.FirstOrDefault();
            }
            return null;
        }

        private static string GetConnectionStting()
        {
            TimeAideContext db = new TimeAideContext();
            var client = db.Client.Find(SessionHelper.SelectedClientId);
            var connStrDb = string.Format("data source={0};initial catalog={1};User ID={2};Password={3};MultipleActiveResultSets=True;App=EntityFramework", client.DBServerName, client.DBName, client.DBUser, client.DBPassword);
            var efConnStr = ConvertToEFConnectionString(connStrDb);
            return efConnStr;
        }

        public static List<T> PunchData<T>(int? employeeId)
        {
            //string efConnStr = GetConnectionStting();
            //TimeAideWindowContext timeAideWindowContext = new TimeAideWindowContext(efConnStr);
            //        tPunchData
            var timeAideWindowContext = DataHelper.GetSelectedClientTAWinEFContext();
            //EmployeeID of Web: e_id
            //[DTPunchDate]: Date of Punch
            //[DTPunchDateTime]: Time of Punch
            //Order In, Out, In, Out
            string query = "select * from tPunchData where  [DTPunchDate] = '" + DateTime.Now.ToShortDateString() + "' And e_id='" + employeeId + "' ";
            
            var punchDateList = timeAideWindowContext.Database
               .SqlQuery<T>(query.ToString())
               .ToList<T>();
            return punchDateList;

        }
    }

}


//  
//  