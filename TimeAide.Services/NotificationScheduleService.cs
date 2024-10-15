using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;


using System.Net;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using TimeAide.Services.Helpers;
using TimeAide.Common.Helpers;
using System.IO;
using TimeAide.Models.ViewModel;

namespace TimeAide.Services
{
    //public class NotificationServiceEventManager
    //{
    //    //public static void AddNotificationServiceEvent(int? clientId, int? companyId, int eventTypeId, string eventMessage)
    //    //{
    //    //    TimeAideContext db = new TimeAideContext();
    //    //    NotificationServiceEvent notificationServiceEvent = new NotificationServiceEvent();
    //    //    if (clientId.HasValue)
    //    //        notificationServiceEvent.ClientId = clientId;
    //    //    if (companyId.HasValue)
    //    //        notificationServiceEvent.CompanyId = companyId;
    //    //    notificationServiceEvent.EventDate = DateTime.Now;
    //    //    notificationServiceEvent.EventTypeId = eventTypeId;
    //    //    notificationServiceEvent.EventMessage = eventMessage;
    //    //    db.NotificationServiceEvent.Add(notificationServiceEvent);
    //    //    db.SaveChanges();
    //    //}
    //}
    public class NotificationScheduleServiceManager
    {
        public void GenerateLog(NotificationScheduleServiceManager manager)
        {
            DateTime testdate = DateTime.Now;
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["TestDate"].ToString()))
            {
                MessageLogging("Application is executing with test date from configuration.");
                testdate = Convert.ToDateTime(ConfigurationManager.AppSettings["TestDate"].ToString());
            }
            MessageLogging("GenerateLog");
            GenerateScheduleNotificationLog(testdate, 1);
            GenerateExpirationNotificationLog(testdate, 1);
            manager.SendNotifications();
        }

        public void GenerateExpirationNotificationLog(DateTime createLogDate, int createdBy)
        {
            MessageLogging("GenerateExpirationNotificationLog");
            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_CreateDefaultExpirationNotificationLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CreateLogDate", createLogDate));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", createdBy));
                cmd.ExecuteNonQuery();
            }
        }

        public void GenerateScheduleNotificationLog(DateTime createLogDate, int createdBy)
        {
            MessageLogging("GenerateScheduleNotificationLog");
            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {

                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_CreateNotificationLog", conn);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CreateLogDate", createLogDate));
                cmd.Parameters.Add(new SqlParameter("@CreatedBy", createdBy));
                cmd.ExecuteNonQuery();
            }
        }
        public void SendNotifications()
        {
            MessageLogging("SendNotifications");
            var logItems = db.NotificationLog.Where(l => l.DeliveryStatusId == (int)DeliveryStatus.PendingSend).ToList();

            foreach (var eachLog in logItems)
            {
                Dictionary<string, string> toEmails = new Dictionary<string, string>();
                List<string> ccEmailBuilder = new List<string>();
                if (eachLog.EmployeeDocumentId.HasValue)
                {
                }
                string message = eachLog.NotificationScheduleDetail.NotificationMessage.Message;
                if (eachLog.NotificationScheduleDetail.NotificationScheduleEmployeeGroup.Count > 0)
                {
                    foreach (var eachScheduleRole in eachLog.NotificationScheduleDetail.NotificationScheduleEmployeeGroup)
                    {
                        foreach (var eachRole in eachScheduleRole.EmployeeGroup.UserEmployeeGroup)
                        {
                            if (eachRole.UserInformation.UserContactInformations.Count > 0 && eachRole.UserInformation.EmployeeStatusId==1) //check active user
                            {
                                var contact = eachRole.UserInformation.UserContactInformations.FirstOrDefault();
                                //Change the notification email from login to notification email
                                if (!String.IsNullOrEmpty(contact.NotificationEmail) && !toEmails.ContainsKey(contact.NotificationEmail))
                                {
                                    ccEmailBuilder.Add(contact.NotificationEmail);
                                    toEmails.Add(contact.NotificationEmail, contact.NotificationEmail);
                                }
                                //else
                                //{
                                //    //Need to discuss if notification email is not set,should we use the workemail.
                                //    if (!string.IsNullOrEmpty(contact.WorkEmail) && !toEmails.ContainsKey(contact.WorkEmail))
                                //    {
                                //        ccEmailBuilder.Add(contact.WorkEmail);
                                //        toEmails.Add(contact.WorkEmail, contact.WorkEmail);
                                //    }
                                //}
                                //ccEmailBuilder.Add(",");
                            }
                        }
                    }
                    //if (ccEmailBuilder.Length > 0)
                    //    ccEmailBuilder.Length--;
                }

                eachLog.DeliveryStatusId = (int)DeliveryStatus.Sent;
                var userInformation = eachLog.UserInformation;

                var AppUrl = ConfigurationManager.AppSettings["AppUrl_" + DatabaseName];
                //string url ="<a href='"//NotificationLog//ReadNotification//2268" title="View Record"> 262</a>
                string url = "<a href='" + AppUrl + "NotificationLog/ReadNotificationEmail/" + eachLog.Id + "'>Please click to review the notification.</a>";

                message = UtilityHelper.ReplaceMessagePlaceholders(eachLog.UserInformation, eachLog, message, url, "", db);
                //string toEmail = "";

                if (userInformation.UserContactInformations.Count > 0 && !eachLog.NotificationScheduleDetail.ExcludeSupervisor)
                {
                    var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(userInformation.Id, userInformation.ClientId??0);
                    foreach (var eachSupervisor in userInformationList)
                    {
                        if (eachSupervisor.EmployeeStatusId == 1)
                        { // active employee
                          //Send to Notification email
                            if (!string.IsNullOrEmpty(eachSupervisor.NotificationEmail) && !toEmails.ContainsKey(eachSupervisor.NotificationEmail))
                            {
                                //if (toEmails.Count > 0)
                                //    ccEmailBuilder.Add(",");
                                ccEmailBuilder.Add(eachSupervisor.NotificationEmail);
                                toEmails.Add(eachSupervisor.NotificationEmail, eachSupervisor.FirstLastName);
                            }
                        }
                    }
                }
                if (userInformation.UserContactInformations.Count > 0 && !eachLog.NotificationScheduleDetail.ExcludeUser)
                {
                    if (userInformation.EmployeeStatusId == 1) //active user
                    {
                        if (!string.IsNullOrEmpty(userInformation.UserContactInformations.FirstOrDefault().NotificationEmail) && !toEmails.ContainsKey(userInformation.UserContactInformations.FirstOrDefault().NotificationEmail))
                        {
                            //if (toEmails.Count > 0)
                            //    ccEmailBuilder.Add(",");
                            ccEmailBuilder.Add(userInformation.UserContactInformations.FirstOrDefault().NotificationEmail);
                            toEmails.Add(userInformation.UserContactInformations.FirstOrDefault().NotificationEmail, userInformation.UserContactInformations.FirstOrDefault().UserInformation.FirstLastName);
                        }
                    }
                    //else if (!string.IsNullOrEmpty(userInformation.UserContactInformations.FirstOrDefault().WorkEmail) && !toEmails.ContainsKey(userInformation.UserContactInformations.FirstOrDefault().WorkEmail))
                    //{
                    //    //if (toEmails.Count > 0)
                    //    //    ccEmailBuilder.Add(",");
                    //    ccEmailBuilder.Add(userInformation.UserContactInformations.FirstOrDefault().WorkEmail);
                    //    toEmails.Add(userInformation.UserContactInformations.FirstOrDefault().WorkEmail, userInformation.UserContactInformations.FirstOrDefault().UserInformation.FirstLastName);
                    //}
                }
                //if (string.IsNullOrEmpty(toEmail))
                //{
                //    toEmail = ccEmail;
                //    ccEmail = "";
                //}
                string toEmail = string.Join(",", ccEmailBuilder);
                if (string.IsNullOrEmpty(toEmail) || toEmail.Length < 2)
                    continue;
                NotificationLogEmail logEmail = new NotificationLogEmail();

                logEmail.ClientId = eachLog.ClientId;
                logEmail.CompanyId = eachLog.CompanyId;
                logEmail.CreatedBy = eachLog.CreatedBy;
                logEmail.SenderAddress = ConfigurationManager.AppSettings["FromMail"].ToString();
                logEmail.ToAddress = toEmail;
                logEmail.CcAddress = "";
                logEmail.BccAddress = "";
                logEmail.NotificationLog = eachLog;
                db.NotificationLogEmail.Add(logEmail);
          
                var mailSetting = UtilityHelper.GetSenderEmailConfiguration(eachLog.CompanyId.Value, eachLog.ClientId.Value, db);

                UtilityHelper.SendEmailWithSenderSetting(mailSetting, toEmails, "", "", null, message, "Notification alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));
                db.SaveChanges();

            }

        }
        public void ErrorNotification(string targetDatabase, string fileName)
        {
            try
            {
                Dictionary<string, string> toEmails = new Dictionary<string, string>();
                //toEmails.Add("info@identechinc.com", "info@identechinc.com");
                toEmails.Add("waqar.q@allshorestaffing.com", "waqar.q@allshorestaffing.com");
                Dictionary<string, string> attachments = new Dictionary<string, string>();
                attachments.Add("ErrorLogFile", fileName);
                string message = "Hi,<br/><br/><br/>There is an error in Notification application, details can be found in file " + fileName + " Target database is " + targetDatabase + ". Same file is also attached.<br/><br/><br/>  Thanks,<br/> TimesAide Notifications";

                var mailSetting = new SenderEmailConfiguration
                {
                    ProviderAccount = ConfigurationManager.AppSettings["ProviderAccount"].ToString(),
                    SenderName = ConfigurationManager.AppSettings["SenderName"].ToString(),
                    FromEmail = ConfigurationManager.AppSettings["FromMail"].ToString(),
                    HostName = ConfigurationManager.AppSettings["Host"].ToString(),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"].ToString()),
                    EnableSsl = true,
                    Password = ConfigurationManager.AppSettings["Password"].ToString()
                };
                UtilityHelper.SendEmailWithSenderSetting(mailSetting, toEmails, "", "", attachments, message, "Notification app , error alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));
                //UtilityHelper.SendEmail(toEmails, "", "", null, "There is an error in Notification application, details can be found in file" + fileName + " Target database is " + targetDatabase, "Notification app , error alert on " + DateTime.Now.ToString("dd-MMM-yyyy"));
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
            }
        }
        public static List<NotificationLog> GetNotificationList(string emailAddress, Boolean unread)
        {

            TimeAideContext db = new TimeAideContext();
            var logItems = db.GetAllByCompany<NotificationLogEmail>(SessionHelper.CompanyId, SessionHelper.ClientId).Where(l => !string.IsNullOrEmpty(emailAddress) && (l.ToAddress.ToLower().Contains(emailAddress.ToLower()) || l.CcAddress.ToLower().Contains(emailAddress.ToLower()))).Select(l => l.NotificationLog).Distinct().ToList();
            if (unread)
            {

                logItems = (from item in logItems
                            where !item.NotificationLogMessageReadBy.Any(i => i.ReadById == SessionHelper.LoginId)
                            select item).ToList();
            }
            return logItems.ToList();
        }
        public string DatabaseName
        {
            get;
            set;
        }
        private TimeAideContext db;
        private SqlConnectionStringBuilder connection;
        public TimeAideContext TimeAideContext
        {
            get
            {
                if (db == null)
                {
                    db = new TimeAideContext();
                    connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString)
                    { InitialCatalog = DatabaseName }; // you can add other parameters.
                    try
                    {
                        db = new TimeAideContext(connection.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        MessageLogging("Invalid Database Name:  " + DatabaseName);
                        ErrorNotification(DatabaseName, LogFileName);
                    }
                }
                return db;
            }
        }
        public string LogFileName
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "ErrorLog", "NotificationLog_" + (DatabaseName ?? "") + DateTime.Now.ToString("yyyyMMdd") + ".txt");

                //if (string.IsNullOrEmpty(DatabaseName))
                //    return @"ErrorLog\NotificationLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                //else
                //    return @"ErrorLog\NotificationLog_" + DatabaseName + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
        }
        public void ErrorLogging(Exception exception)
        {
            string stackTrace = exception.StackTrace;
            StringBuilder errorMessage = new StringBuilder();


            CreateFile();
            while (exception != null)
            {
                errorMessage.Append(exception.Message);
                exception = exception.InnerException;
            }
            using (StreamWriter sw = File.AppendText(LogFileName))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + errorMessage.ToString());
                sw.WriteLine("Stack Trace: " + stackTrace);
                sw.WriteLine("===========End============= " + DateTime.Now);

            }
        }
        public void MessageLogging(string message)
        {
            CreateFile();
            using (StreamWriter sw = File.AppendText(LogFileName))
            {
                sw.WriteLine("=============Message Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Message: " + message);
                sw.WriteLine("===========End============= " + DateTime.Now);

            }
        }

        private void CreateFile()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "ErrorLog")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "ErrorLog"));
            if (!File.Exists(LogFileName))
                File.Create(LogFileName).Dispose();
        }
    }
}
