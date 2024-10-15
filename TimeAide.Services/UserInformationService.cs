using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class UserInformationService
    {
        public static string MaskSsn(string ssn, int digitsToShow = 4, char maskCharacter = '*')
        {
            if (String.IsNullOrWhiteSpace(ssn)) return String.Empty;
            ssn = Encryption.Decrypt(ssn);
            //if (ssn.Length != 9) return String.Empty;
            if (string.IsNullOrEmpty(ssn))
                return "";
            const int ssnLength = 9;
            const string separator = "-";
            int maskLength = ssnLength - digitsToShow;
            if (ssn.Length < maskLength)
                return "Invalid SSN";
            // truncate and convert to number
            int output = Int32.Parse(ssn.Replace(separator, String.Empty).Substring(maskLength, digitsToShow));

            string format = String.Empty;
            for (int i = 0; i < maskLength; i++) format += maskCharacter;
            for (int i = 0; i < digitsToShow; i++) format += "0";

            format = format.Insert(3, separator).Insert(6, separator);
            format = "{0:" + format + "}";

            return String.Format(format, output);
        }

        public static UserContactInformation GetActiveUserContactInformation(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == userInformationId && u.DataEntryStatus == 1);
        }
        public static UserInformation GetUserInformation(int userInformationId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.UserInformation.Where(w => w.Id == userInformationId).FirstOrDefault();
        }
        public static UserInformationViewModel GetUserInformationViewModel(int employeeId)
        {
            TimeAideContext db = new TimeAideContext();
            return db.SP_UserInformation<UserInformationViewModel>(employeeId, "", 0, 1, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).FirstOrDefault();
        }
        public static void AddUserRegistrationLog(string viewType, TimeAideContext context, UserInformation userInformation, string accountActivityType)
        {
            string action = "";
            if (viewType == "Account")
                action = "Registration Email";
            else
                action = "Manual User Registration";
            AddUserRegistrationLog("Account Registration", accountActivityType, userInformation.Id, userInformation, action, context);
        }

        public static void AddUserRegistrationLog(string tableName, string accountActivityType, int referenceId, UserInformation referenceObject, string action, TimeAideContext context)
        {
            var log = new AuditLog()
            {
                ActionType = accountActivityType,
                TableName = tableName,
                ReferenceId = referenceId,
                ReferenceObject = referenceObject,
                UserInformationId = SessionHelper.LoginId,

            };

            var logDetail = new AuditLogDetail()
            {
                AuditLog = log,
                OldValue = "",
                ColumnName = action
            };

            if (accountActivityType == "Activation Link clicked")
            {
                logDetail.ClientId = log.ClientId = referenceObject.ClientId;
                var activeEmployment = TimeAide.Services.EmploymentService.GetActiveEmployment(referenceId);
                EmploymentHistory activeEmploymentHistory = null;
                if (activeEmployment != null)
                {
                    activeEmploymentHistory = TimeAide.Services.EmploymentHistoryService.GetActiveEmploymentHistory(referenceId, activeEmployment.Id);
                    if (activeEmploymentHistory != null)
                    {
                        logDetail.CompanyId = log.CompanyId = activeEmploymentHistory.CompanyId;
                    }
                }
            }
            log.AuditLogDetail.Add(logDetail);
            context.AuditLog.Add(log);
        }

        //public static void AddUserRegistrationLog(string viewType, TimeAideContext context, UserContactInformation contact, UserInformation userInformation, string accountActivityType)
        //{
        //    var log = new AuditLog()
        //    {
        //        ActionType = accountActivityType,
        //        TableName = "Account Registration",
        //        ReferenceId = userInformation.Id,
        //        ReferenceObject = userInformation,
        //        UserInformationId = SessionHelper.LoginId,

        //    };

        //    var logDetail = new AuditLogDetail()
        //    {
        //        AuditLog = log,
        //        OldValue = "",
        //    };

        //    if (viewType == "Account")
        //        logDetail.ColumnName = "Registration Email";
        //    else
        //        logDetail.ColumnName = "Manual User Registration";
        //    log.AuditLogDetail.Add(logDetail);
        //    context.AuditLog.Add(log);
        //}
        public static string GetTimesheetApprovalStatus(string status)
        {
            if (status == "0")
                return "Unreviewed";
            else if (status == "1")
                return "Reviewed";
            else if (status == "2")
                return "Approved";
            else
               return "";


        }
        public static string GetFormattedNumber(String contactNumber)
        {
            if (!string.IsNullOrEmpty(contactNumber) && contactNumber.Length == 10)
                return String.Format("{0:(000) 000-0000}", Convert.ToInt64(contactNumber));
            else
                return contactNumber;
        }
    }
}
