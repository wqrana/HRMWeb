using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Models.Models;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Services.Helpers
{
    public static class UtilityHelper
    {
        //(SenderEmailConfiguration senderEmailConfiguration)//
        public static void ForgotPasswordEmail(string toEmailAddress, string toName, string emailSubject, string emailBody, string activationCode)
        {
            if (String.IsNullOrEmpty(emailSubject))
            {
                emailSubject = "Reset Password";
            }

            string body = "Hi  " + toName + " ,<br/><br/><br/> We received a request to reset your password, please follow link in the email to reset password, <a href='" + SessionHelper.GetApplicationUrl() + "Account/ResetPassword?activationCode=" + activationCode + "'>Please click here to reset password.</a>    <br/><br/><br/> Thanks,<br/> TimesAide";
            if (!String.IsNullOrEmpty(emailBody))
            {
                emailBody = emailBody.Replace("[LastName]", toName);
                emailBody = emailBody.Replace("[URL]", "<a href='" + SessionHelper.GetApplicationUrl() + "Account/ResetPassword?activationCode=" + activationCode + "'>click here</a>");
                body = emailBody;
            }
            Dictionary<string, string> toEmailAddresses = new Dictionary<string, string>();
            toEmailAddresses.Add(toEmailAddress, toName);
            SendEmailWithSenderSetting(toEmailAddresses, null, null, null, emailBody, emailSubject);
        }
        public static void SetSessionVariables(UserInformation userInformation, string loginEmail)
        {
            SessionHelper.LoginId = userInformation.Id;
            SessionHelper.LoginEmployeeId = userInformation.EmployeeId ?? 0;
            SessionHelper.LoginEmployeeName = userInformation.ShortFullName;
            SessionHelper.SelectedClientId = SessionHelper.ClientId = userInformation.ClientId ?? 0;
            SessionHelper.SelectedCompanyId = SessionHelper.CompanyId = userInformation.CompanyId ?? 0;
            SessionHelper.IsTimeAideWindowClient = (DataHelper.GetSelectedClientTAWinEFContext() == null) ? false : true;
            var company = (new TimeAideContext()).Find<Company>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
            if (company != null)
                SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
            var userContactInfo = userInformation.UserContactInformations.FirstOrDefault();
            if (userContactInfo != null)
            {
                SessionHelper.NotificationEmail = userContactInfo.NotificationEmail;
            }

            SessionHelper.LoginEmail = loginEmail;
            SessionHelper.UserProfilePicture = string.IsNullOrEmpty(userInformation.PictureFilePath) ? "/images/no-profile-image.jpg" : userInformation.PictureFilePath;
            TimeAideContext db = new TimeAideContext();
            SessionHelper.SupervisorCompany = String.Join(",", db.SupervisorCompany.Where(w => w.DataEntryStatus == 1 && w.Company.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == userInformation.Id).Select(c => c.Company.Id.ToString()));
            SessionHelper.SupervisorEmployeeType = String.Join(",", db.SupervisorEmployeeType.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == userInformation.Id).Select(c => c.EmployeeType.Id.ToString()));
            SessionHelper.SupervisorSubDepartment = String.Join(",", db.SupervisorSubDepartment.Where(w => w.DataEntryStatus == 1 && w.SubDepartment.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == userInformation.Id).Select(c => c.SubDepartment.Id.ToString()));
            SessionHelper.SupervisorDepartment = String.Join(",", db.SupervisorDepartment.Where(w => w.DataEntryStatus == 1 && w.Department.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == userInformation.Id).Select(c => c.Department.Id.ToString()));
            if (userInformation.UserInformationRole.Count > 0)
            {
                var userInformationRole = userInformation.UserInformationRole.FirstOrDefault();
                SessionHelper.RoleId = userInformationRole.RoleId;
                SessionHelper.RoleTypeId = userInformationRole.Role.RoleTypeId ?? 0;
            }

        }

        public static DeviceMetaData DeviceValidate(UserInformation userinformation)
        {

            DeviceMetaData devicemetadata   = new DeviceMetaData();

            var iPAddress=TimeAide.Common.Helpers.SecurityAlert.ExtractIp();

            IPAddress address = IPAddress.Parse(iPAddress);
           
            if (IPAddress.IsLoopback(address))
            {
                return null;
            }
            //string ipNumber = SessionHelper.ConvertIpAddressToIpNumber(IPAddress);
            var location=TimeAide.Common.Helpers.SecurityAlert.GetIpLocation(iPAddress);

            if (string.IsNullOrEmpty(location))
            {
                location = "unknown";
            }
            var devicedetail = TimeAide.Common.Helpers.SecurityAlert.GetDeviceDetails();
            TimeAideContext db = new TimeAideContext();


             var devicemetadatalist= db.GetAllByUser<DeviceMetaData>(userinformation.Id, userinformation.ClientId ?? 0);


            foreach (var devicedata in devicemetadatalist)
            {
                if(devicedata.DeviceDetails.ToLower()==devicedetail.ToLower()
                    && devicedata.Location.ToLower() == location.ToLower())
                {

                    if(devicedata.IsVerified == true)
                    {
                        devicedata.LastLoggedIn = DateTime.Now;
                        db.SaveChanges();
                        return null;
                    }
                    else
                    {
                        devicedata.LastLoggedIn = DateTime.Now;
                        db.SaveChanges();
                        return devicedata;
                    }

                  
                }

            }

            devicemetadata.DeviceDetails = devicedetail;
            devicemetadata.Location = location;
            devicemetadata.UserInformationId = userinformation.Id;
            devicemetadata.ClientId = userinformation.ClientId;
            devicemetadata.LastLoggedIn = DateTime.Now;
            devicemetadata.IPaddress = iPAddress;

            db.DeviceMetaData.Add(devicemetadata);
            db.SaveChanges();
         
            return devicemetadata;

        }
        public static UserSessionLog LogUserActivity()
        {
            UserSessionLog userSessionLog = new UserSessionLog();
            userSessionLog.IPAddress = SessionHelper.GetIPAddress();
            string ipNumber = SessionHelper.ConvertIpAddressToIpNumber(userSessionLog.IPAddress);
            TimeAideContext db = new TimeAideContext();
            //var abc = db.CountryIPAddress.Where(ip => ip.StartIPNumber <= ipNumber && ip.EndIPNumber >= ipNumber);

            //string key = "2ffc484db8cfcd2248468e1f1aaa53e1";
            //string ip = "134.201.250.155";
            //var targetUri = new Uri("http://api.ipstack.com/"+ip+"?access_key="+ key);
            //var webRequest = (HttpWebRequest)WebRequest.Create(targetUri);
            //var webRequestResponse = webRequest.GetResponse();

            userSessionLog.BrowserInformation = SessionHelper.GetBrowserInformation();
            userSessionLog.DeviceInformation = SessionHelper.GetDeviceName();
            userSessionLog.OsInformation = "";
            userSessionLog.Location = "";
            userSessionLog.UserInformationId = SessionHelper.LoginId;

            db.UserSessionLog.Add(userSessionLog);
            db.SaveChanges();
            userSessionLog.UserInformationId = SessionHelper.LoginId;
            SessionHelper.UserSessionLogId = userSessionLog.Id;
            UserSessionLogDetail("Account", "Login");
            return userSessionLog;
        }
        public static UserSessionLogDetail UserSessionLogDetail(string controllerName, string actionName)
        {
            UserSessionLogDetail userSessionLogDetail = new UserSessionLogDetail();
            userSessionLogDetail.UserSessionLogId = SessionHelper.UserSessionLogId;
            userSessionLogDetail.ControllerName = controllerName;
            userSessionLogDetail.ActionName = actionName;
            TimeAideContext db = new TimeAideContext();
            db.UserSessionLogDetail.Add(userSessionLogDetail);
            db.SaveChanges();
            SessionHelper.UserSessionLogDetailId = userSessionLogDetail.Id;
            return userSessionLogDetail;
        }
        public static string Pluralize(string word)
        {
            var pluralword = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(System.Globalization.CultureInfo.GetCultureInfo("en-us")).Pluralize(word);
            return pluralword;
        }

        public static void SendSecurityAlertEmail(int SecurityCode,Dictionary<string, string> toEmailAddress,DeviceMetaData devicemetadata)
        {
            string emailBody = "";
            string emailSubject = "";
            string loginEmail = toEmailAddress.First().Key;
            
        

            emailSubject = "Security Alert";
            emailBody = "Hi  " + toEmailAddress.First().Value + " ,<br/><br/> We noticed a new sign-in to your Account.please use the below security Code for the sign-in,this code will expre in 24 hours for security reason.<br/><h1 style='margin-left:100px'>" + SecurityCode + "</h1><br/> Please find the below details of the new sign-in<br/><br/> &nbsp;&nbsp;&nbsp; DeviceDetail:<b>" + devicemetadata.DeviceDetails+ "</b>&nbsp;&nbsp;Estimated Location:<b>" + devicemetadata.Location+"</b>&nbsp;&nbsp;IP Address:<b>" + devicemetadata.IPaddress+"</b><br/><br/>Thanks,<br/> TimesAide";

            SendEmail(toEmailAddress, "", "", null, emailBody, emailSubject);
        }
        public static void SendRegistrationEmail(Dictionary<string, string> toEmailAddress, string activationCode, TimeAideContext db, int userInformationId, string password, string emailType)
        {
            //string subject = "Please Complete Registration process";
            //var AppUrl = ConfigurationManager.AppSettings["AppUrl"];
            string emailBody = "";
            string emailSubject = "";
            string loginEmail = toEmailAddress.First().Key;
            var emailTemplate = db.EmailTemplate.FirstOrDefault(u => u.EmailType.EmailTypeName == emailType && u.ClientId == SessionHelper.SelectedClientId);
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userInformationId);

            if (emailTemplate != null)
            {
                string url = "<br/><br/><br/> <a href='" + SessionHelper.GetApplicationUrl() + "Account/Register?activationCode=" + activationCode + "'>Please follow the link to ";
                if (emailType != "Registration")
                    url += " login.</a> It is recmonded to change password on first login.";
                else
                {
                    url += " complete registration</a>";
                    password = "";
                }
                emailBody = ReplaceMessagePlaceholders(userInformation, null, emailTemplate.EmailBody, url, password,null, loginEmail);
                emailSubject = emailTemplate.EmailSubject;
            }
            else
            {
                if (emailType != "Registration")
                {
                    emailSubject = "Welcome to Time Aide Web";
                    emailBody = "Hi  " + toEmailAddress.First().Value + " ,<br/><br/><br/> <a href='" + SessionHelper.GetApplicationUrl() + "Account/Login'>Please follow the link to login application,</a><br/><br/><br/> Your temporary password is:  " + password + ". It is recmonded to change password on first login.<br/><br/><br/>  Thanks,<br/> TimesAide";

                }
                else
                {
                    emailSubject = "Please Complete Registration process";
                    emailBody = "Hi  " + toEmailAddress.First().Value + " ,<br/><br/><br/> <a href='" + SessionHelper.GetApplicationUrl() + "Account/Register?activationCode=" + activationCode + "'>Please follow the link to complete registration</a>    <br/><br/><br/> Thanks,<br/> TimesAide";
                }
            }
            SendEmail(toEmailAddress, "", "", null, emailBody, emailSubject);

        }

        public static string ReplaceMessagePlaceholders(UserInformation userInformation, NotificationLog eachLog, string emailBody, string uRL, string password,TimeAideContext db = null,string loginEmail="")
        {
            var employment = EmploymentService.GetActiveEmployment(userInformation.Id, db);
            EmploymentHistory employmentHistory=null;
            if (employment != null)
            {
                employmentHistory = EmploymentHistoryService.GetActiveEmploymentHistory(userInformation.Id, employment.Id,db);
            }
            emailBody = emailBody.Replace("[FirstName]", userInformation.FirstName);
            emailBody = emailBody.Replace("[EmployeeId]", userInformation.EmployeeId.ToString());
            emailBody = emailBody.Replace("[LastName]", userInformation.FirstName);
            emailBody = emailBody.Replace("[MiddleInitial]", userInformation.MiddleInitial);
            emailBody = emailBody.Replace("[FirstLastName]", userInformation.FirstLastName);
            emailBody = emailBody.Replace("[SecondLastName]", userInformation.SecondLastName);
            emailBody = emailBody.Replace("[ShortFullName]", userInformation.ShortFullName);
            emailBody = emailBody.Replace("[CompanyName]", userInformation.Company.CompanyName);
            if (employmentHistory != null)
            {
                if (employment.EmploymentStatus != null)
                    emailBody = emailBody.Replace("[EmploymentStatus]", employment.EmploymentStatus.EmploymentStatusName);
                if (employmentHistory.Department != null)
                    emailBody = emailBody.Replace("[Department]", employmentHistory.Department.DepartmentName);
                if (employmentHistory.SubDepartment != null)
                    emailBody = emailBody.Replace("[SubDepartment]", employmentHistory.SubDepartment.SubDepartmentName);
                if (employmentHistory.EmployeeType != null)
                    emailBody = emailBody.Replace("[EmployeeType]", employmentHistory.EmployeeType.EmployeeTypeName);
                if (employmentHistory.Position != null)
                    emailBody = emailBody.Replace("[PositionId]", employmentHistory.Position.PositionName);
            }
            if (emailBody.Contains("[LoginUserName]"))
            {
                db = new TimeAideContext();
                //var loginUser = db.UserInformation.FirstOrDefault(u => u.Id == SessionHelper.LoginId);
               // var userLogin = db.UserContactInformation.Where(w => w.UserInformationId == userInformation.Id && w.LoginEmail!=null).FirstOrDefault();
               // var userLoginEmail = userLogin == null ? "" : userLogin.LoginEmail;
                emailBody = emailBody.Replace("[LoginUserName]", loginEmail);
            }
            //if (userInformation.EmploymentStatusId != null)
            //    emailBody = emailBody.Replace("[EmploymentStatus]", userInformation.EmploymentStatusId.Value.ToString());

            if (eachLog != null)
            {
                emailBody = emailBody.Replace("[DaysBefore]", eachLog.ExpiringDays.ToString() + " day(s)");
                if (emailBody.Contains("[RecordName]") && !String.IsNullOrEmpty(eachLog.RecordName))
                    emailBody = emailBody.Replace("[RecordName]", eachLog.RecordName);
            }
            if (!string.IsNullOrEmpty(uRL))
                emailBody = emailBody.Replace("[URL]", uRL);
            if (!string.IsNullOrEmpty(password))
            {
                if (emailBody.Contains("[Password]"))
                    emailBody = emailBody.Replace("[Password]", password);
                else
                    emailBody += " Your temporary password is: " + password;
            }

            emailBody = emailBody.Replace("\\n", "<br\\>");
            return emailBody;
        }

        public static void SendEmailByWorkflow(int userInformationId, WorkflowTriggerRequestDetail workflowTriggerRequestDetail, int changeRequestId, TimeAideContext db)
        {
            WorkflowTriggerRequestDetailEmail logEmail = new WorkflowTriggerRequestDetailEmail();
            string toEmail = "";
            Dictionary<string, string> toEmails = new Dictionary<string, string>();
            // toEmails.Add("wqrana@gmail.com", "wqrana@gmail.com");
            if (workflowTriggerRequestDetail.WorkflowLevel.WorkflowLevelTypeId == 1)
            {
                var userInformationList = db.SP_GetEmployeeSupervisors<UserInformationViewModel>(userInformationId, SessionHelper.SelectedClientId)
                                                                .Where(w=>w.EmployeeStatusId==1); //active employee only
                foreach (var each in userInformationList)
                {
                    if (!string.IsNullOrEmpty(each.NotificationEmail) && !toEmails.ContainsKey(each.NotificationEmail))
                    {
                        toEmail += toEmail == "" ? each.NotificationEmail : "," + each.NotificationEmail;
                        toEmails.Add(each.NotificationEmail, each.NotificationEmail);
                    }
                }
            }
            if (workflowTriggerRequestDetail.WorkflowLevel.WorkflowLevelGroup.Count > 0)
            {
                var userInformationList = db.SP_GetUserInformationByWorkflowLevelGroupId<UserInformationViewModel>(workflowTriggerRequestDetail.WorkflowLevel.Id, SessionHelper.SelectedClientId)
                                                                    .Where(w => w.EmployeeStatusId == 1); //active employee only;
                foreach (var each in userInformationList)
                {
                    if (!string.IsNullOrEmpty(each.NotificationEmail) && !toEmails.ContainsKey(each.NotificationEmail))
                    {
                        toEmail += toEmail == "" ? each.NotificationEmail : "," + each.NotificationEmail;
                        toEmails.Add(each.NotificationEmail, each.NotificationEmail);
                    }
                }
            }

            logEmail.SenderAddress = ConfigurationManager.AppSettings["FromMail"].ToString();
            logEmail.ToAddress = toEmail;
            logEmail.CcAddress = "";
            logEmail.BccAddress = "";
            logEmail.WorkflowTriggerRequestDetail = workflowTriggerRequestDetail;
            db.WorkflowTriggerRequestDetailEmail.Add(logEmail);
            db.SaveChanges();

            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userInformationId);

            var notificationMessage = db.NotificationMessage.FirstOrDefault(m => m.Id == workflowTriggerRequestDetail.WorkflowLevel.NotificationMessageId);

            string url = "<a href='" + SessionHelper.GetApplicationUrl() + TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(workflowTriggerRequestDetail, true, true) + "'>Please click to review the request.</a>  <br/><br/><br/> Thanks,<br/> TimeAide";
            string message = UtilityHelper.ReplaceMessagePlaceholders(userInformation, null, notificationMessage.Message, url, "");

            TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "New Request Notification - " + DateTime.Now.ToString("dd-MMM-yyyy"));

        }
        public static void SendEmailByWorkflowReminderNotification(int userInformationId, Workflow workflow, WorkflowTriggerRequestDetail workflowTriggerRequestDetail, int changeRequestId, TimeAideContext db)
        {
            // WorkflowTriggerRequestDetailEmail logEmail = new WorkflowTriggerRequestDetailEmail();
            string toEmail = "timeaideweb@gmail.com";
            Dictionary<string, string> toEmails = new Dictionary<string, string>();
            // toEmails.Add("wqrana@gmail.com", "wqrana@gmail.com");
            var RequesstReminderEmail = db.WorkflowTriggerRequestDetailEmail.Where(w => w.WorkflowTriggerRequestDetailId == workflowTriggerRequestDetail.Id).FirstOrDefault();
            if (RequesstReminderEmail != null)
            {
                if (!string.IsNullOrEmpty(RequesstReminderEmail.ToAddress))
                {
                    toEmail += "," + RequesstReminderEmail.ToAddress;
                }
            }
            foreach (var email in toEmail.Split(','))
            {
                if (!toEmails.ContainsKey(email))
                    toEmails.Add(email, email);
            }
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userInformationId);
            var notificationMessage = db.NotificationMessage.FirstOrDefault(m => m.Id == workflow.ReminderNotificationMessageId);
            string reminderMessage = notificationMessage == null ? "Reminder Notification [URL]" : notificationMessage.Message;
            string url = "<a href='" + SessionHelper.GetApplicationUrl() + TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(workflowTriggerRequestDetail, true, true) + "'>Please click to review the request.</a>  <br/><br/><br/> Thanks,<br/> TimeAide";
            string message = UtilityHelper.ReplaceMessagePlaceholders(userInformation, null, reminderMessage, url, "");

            TimeAide.Services.Helpers.UtilityHelper.SendEmail(toEmails, "", "", null, message, "Request Reminder Notification");

        }
        public static void SendEmailByWorkflowAutoCancelNotification(int userInformationId, int changeRequestId, Workflow workflow, WorkflowTriggerRequestDetail workflowTriggerRequestDetail, Dictionary<string, string> toEmails, TimeAideContext db)
        {
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userInformationId);
            var messageObj = db.NotificationMessage.FirstOrDefault(m => m.Id == workflow.CancelNotificationMessageId);
            var CancelmessageTemplate = messageObj == null ? "Cancel Request Notification [URL]" : messageObj.Message;
            string url = "<a href='" + SessionHelper.GetApplicationUrl() + TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(workflowTriggerRequestDetail, true, true) + "'>Please click to review the request.</a>  <br/><br/><br/> Thanks,<br/> TimeAide";
            string message = UtilityHelper.ReplaceMessagePlaceholders(userInformation, null, CancelmessageTemplate, url, "");

            UtilityHelper.SendEmail(toEmails, "", "", null, message, "Request Cancellation Notification");

        }
        public static void SendEmailByWorkflowClosingNotification(int userInformationId, int changeRequestId, Workflow workflow, WorkflowTriggerRequestDetail workflowTriggerRequestDetail, Dictionary<string, string> toEmails, TimeAideContext db)
        {
            var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userInformationId);
            string message1 = db.NotificationMessage.FirstOrDefault(m => m.Id == workflow.ClosingNotificationMessageId).Message;
            string url = "<a href='" + SessionHelper.GetApplicationUrl() + TimeAide.Services.WorkflowService.GetChangeRequestApprovalUrl(workflowTriggerRequestDetail, false, true) + "'>Please click to review the request.</a>  <br/><br/><br/> Thanks,<br/> TimeAide";
            string message = UtilityHelper.ReplaceMessagePlaceholders(userInformation, null, message1, url, "");
            UtilityHelper.SendEmail(toEmails, "", "", null, message, "Request Closing Notification- " + DateTime.Now.ToString("dd-MMM-yyyy"));

        }


        public static void SendEmail(Dictionary<string, string> toEmailAddress, string cc, string bcc, Dictionary<string, string> attachments, string emailBody, string emailSubject)
        {
            SendEmailWithSenderSetting(toEmailAddress, cc, bcc, attachments, emailBody, emailSubject);
        }
        public static void TestEmail(SenderEmailConfiguration senderEmailConfiguration)
        {
            Dictionary<string, string> toEmailAddress = new Dictionary<string, string>();
            toEmailAddress.Add(senderEmailConfiguration.ToEmailForTest, senderEmailConfiguration.ToEmailForTest);
            SendEmailWithSenderSetting(senderEmailConfiguration, toEmailAddress, null, null, null, "It is email to test sender config", "Test Email");
        }
        private static void SendEmailWithSenderSetting(Dictionary<string, string> toEmailAddress, string cc, string bcc, Dictionary<string, string> attachments, string emailBody, string emailSubject)
        {
            SenderEmailConfiguration senderEmailConfiguration = GetSenderEmailConfiguration(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
            SendEmailWithSenderSetting(senderEmailConfiguration, toEmailAddress, cc, bcc, attachments, emailBody, emailSubject);
        }
        public static void SendEmailWithSenderSetting(SenderEmailConfiguration senderEmailConfiguration, Dictionary<string, string> toEmailAddress, string cc, string bcc, Dictionary<string, string> attachments, string emailBody, string emailSubject)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            //mailMessage.Sender = new MailAddress(senderEmailConfiguration.ProviderAccount);
            mailMessage.From = new MailAddress(senderEmailConfiguration.FromEmail, senderEmailConfiguration.SenderName);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = emailSubject;
            mailMessage.Body = emailBody;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mailMessage.ReplyToList.Add(senderEmailConfiguration.FromEmail);

            if (toEmailAddress != null)
            {
                foreach (var ToEMailId in toEmailAddress)
                {
                    if (!string.IsNullOrEmpty(ToEMailId.Key))
                        mailMessage.To.Add(new System.Net.Mail.MailAddress(ToEMailId.Key, ToEMailId.Value ?? ToEMailId.Key));
                }
            }
            if (!String.IsNullOrEmpty(cc))
            {
                string[] CCId = cc.Split(',');
                foreach (string CCEmail in CCId)
                {
                    mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
                }
            }
            if (!String.IsNullOrEmpty(bcc))
            {
                string[] bccid = bcc.Split(',');
                foreach (string bccEmailId in bccid)
                {
                    mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
                }
            }
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    Attachment newAttachment = new Attachment(attachment.Value);
                    mailMessage.Attachments.Add(newAttachment);
                }
            }

            smtp.Host = senderEmailConfiguration.HostName;
            smtp.Port = senderEmailConfiguration.Port;
            smtp.EnableSsl = senderEmailConfiguration.EnableSsl;
            smtp.UseDefaultCredentials = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(senderEmailConfiguration.ProviderAccount, senderEmailConfiguration.Password);
            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SenderEmailConfiguration GetSenderEmailConfiguration(int compayId, int clientId, TimeAideContext db = null)
        {
            if (db == null)
                db = new TimeAideContext();
            var senderEmails = db.GetAllByCompany<SenderEmailConfiguration>(compayId, clientId);
            var mailSetting = senderEmails.FirstOrDefault(s => s.CompanyId.HasValue);
            if (mailSetting == null)
                mailSetting = senderEmails.FirstOrDefault(s => !s.CompanyId.HasValue);
            if (mailSetting == null)
                mailSetting = db.SenderEmailConfiguration.FirstOrDefault(s => s.ClientId == 1);
            if (mailSetting == null)
            {
                int port;
                if (!int.TryParse(ConfigurationManager.AppSettings["PortNumber"].ToString(), out port))
                    port = 587;
                bool enableSsl;
                if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableSsl"].ToString(), out enableSsl))
                    enableSsl = true;
                mailSetting = new SenderEmailConfiguration
                {
                    ProviderAccount = ConfigurationManager.AppSettings["ProviderAccount"].ToString(),
                    SenderName = ConfigurationManager.AppSettings["SenderName"].ToString(),
                    FromEmail = ConfigurationManager.AppSettings["FromMail"].ToString(),
                    HostName = ConfigurationManager.AppSettings["Host"].ToString(),
                    Port = port,
                    EnableSsl = enableSsl,
                    Password = ConfigurationManager.AppSettings["Password"].ToString()
                };
            }
            return mailSetting;
        }
    }
}