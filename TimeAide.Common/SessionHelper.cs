using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TimeAide.Common.Helpers
{
    public class SessionHelper
    {

        // private constructor
        private SessionHelper()
        {
            //Property1 = "default value";
        }
        public static bool IsExternalSession { get; set; }
        public static int ExternalSessionClientId { get; set; }
        public static int ExternalSessionCompanyId  {get;   set; }
        public static int ExternalSessionLoginId { get; set; }
        // Gets the current session.
        public static SessionHelper Current
        {
            get
            {
                SessionHelper session =
                  (SessionHelper)HttpContext.Current.Session["__SessionHelper__"];
                if (session == null)
                {
                    session = new SessionHelper();
                    HttpContext.Current.Session["__SessionHelper__"] = session;
                }
                return session;
            }
        }

        public static HttpContext CurretnHttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }
        // **** add your session properties here, e.g like this:
        public static int ClientId
        {
            get
            {
                if (HttpContext.Current.Session["ClientId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["ClientId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["ClientId"] = value;
            }
        }

        public static int SelectedClientId
        {
            get
            {
                if (IsExternalSession)
                    return ExternalSessionClientId;

                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["SelectedClientId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["SelectedClientId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["SelectedClientId"] = value;
            }
        }
        public static int CompanyId
        {
            get
            {
                if (HttpContext.Current.Session["CompanyId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["CompanyId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["CompanyId"] = value;
            }
        }

        public static int SelectedCompanyId
        {
            get
            {
                if (IsExternalSession)
                    return ExternalSessionCompanyId;

                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["SelectedCompanyId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["SelectedCompanyId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["SelectedCompanyId"] = value;
            }
        }

        public static int SelectedCompanyId_Old
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["SelectedCompanyId_Old"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["SelectedCompanyId_Old"].ToString());
            }
            set
            {
                HttpContext.Current.Session["SelectedCompanyId_Old"] = value;
            }
        }
        public static string SelectedCompanyName
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["SelectedCompanyName"] == null)
                    return "";
                return HttpContext.Current.Session["SelectedCompanyName"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SelectedCompanyName"] = value;
            }
        }
        public static int SelectedUserInformationId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["SelectedUserInformationId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["SelectedUserInformationId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["SelectedUserInformationId"] = value;
            }
        }
        public static int LoginId
        {
            get
            {
                if (IsExternalSession)
                    return ExternalSessionLoginId;

                if (HttpContext.Current == null || HttpContext.Current.Session==null || HttpContext.Current.Session["LoginId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["LoginId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["LoginId"] = value;
            }
        }

        public static int LoginEmployeeId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["LoginEmployeeId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["LoginEmployeeId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["LoginEmployeeId"] = value;
            }
        }
        public static string LoginEmployeeName
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["LoginEmployeeName"] == null)
                    return "";
                return (HttpContext.Current.Session["LoginEmployeeName"].ToString());
            }
            set
            {
                HttpContext.Current.Session["LoginEmployeeName"] = value;
            }
        }
        
        public static int UserSessionLogId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["UserSessionLogId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["UserSessionLogId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["UserSessionLogId"] = value;
            }
        }

        public static int UserSessionLogDetailId
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["UserSessionLogDetailId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["UserSessionLogDetailId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["UserSessionLogDetailId"] = value;
            }
        }



        public static string UserProfilePicture
        {
            get
            {
                if (HttpContext.Current.Session["UserProfilePicture"] == null)
                    return "";
                return HttpContext.Current.Session["UserProfilePicture"].ToString();
            }
            set
            {
                HttpContext.Current.Session["UserProfilePicture"] = value;
            }
        }
        public static string ActivationCode
        {
            get
            {
                if (HttpContext.Current.Session["ActivationCode"] == null)
                    return "";
                return HttpContext.Current.Session["ActivationCode"].ToString();
            }
            set
            {
                HttpContext.Current.Session["ActivationCode"] = value;
            }
        }

        public static string LoginEmail
        {
            get
            {
                if (HttpContext.Current.Session["LoginEmail"] == null)
                    return "";
                return HttpContext.Current.Session["LoginEmail"].ToString();
            }
            set
            {
                HttpContext.Current.Session["LoginEmail"] = value;
            }
        }
        public static string NotificationEmail
        {
            get
            {
                if (HttpContext.Current.Session["NotificationEmail"] == null)
                    return "";
                return HttpContext.Current.Session["NotificationEmail"].ToString();
            }
            set
            {
                HttpContext.Current.Session["NotificationEmail"] = value;
            }
        }

        public static string PreviousLoginEmail
        {
            get
            {
                if (HttpContext.Current.Session["PreviousLoginEmail"] == null)
                    return "";
                return HttpContext.Current.Session["PreviousLoginEmail"].ToString();
            }
            set
            {
                HttpContext.Current.Session["PreviousLoginEmail"] = value;
            }
        }
        public static string SupervisorCompany
        {
            get
            {
                if (HttpContext.Current.Session["SupervisorCompany"] == null)
                    return "";
                return HttpContext.Current.Session["SupervisorCompany"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SupervisorCompany"] = value;
            }
        }

        public static string SupervisorEmployeeType
        {
            get
            {
                if (HttpContext.Current.Session["SupervisorEmployeeType"] == null)
                    return "";
                return HttpContext.Current.Session["SupervisorEmployeeType"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SupervisorEmployeeType"] = value;
            }
        }

        public static string SupervisorSubDepartment
        {
            get
            {
                if (HttpContext.Current.Session["SupervisorSubDepartment"] == null)
                    return "";
                return HttpContext.Current.Session["SupervisorSubDepartment"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SupervisorSubDepartment"] = value;
            }
        }

        public static string SupervisorDepartment
        {
            get
            {
                if (HttpContext.Current.Session["SupervisorDepartment"] == null)
                    return "";
                return HttpContext.Current.Session["SupervisorDepartment"].ToString();
            }
            set
            {
                HttpContext.Current.Session["SupervisorDepartment"] = value;
            }
        }

        public static int RoleId
        {
            get
            {
                if (HttpContext.Current.Session["RoleId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["RoleId"] = value;
            }
        }

        public static int RoleTypeId
        {
            get
            {
                if (HttpContext.Current.Session["RoleTypeId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["RoleTypeId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["RoleTypeId"] = value;
            }
        }

        public static bool IsTimeAideWindowClient
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["IsTimeAideWindowClient"] == null)
                    return false;
                return Convert.ToBoolean(HttpContext.Current.Session["IsTimeAideWindowClient"].ToString());
            }
            set
            {
                HttpContext.Current.Session["IsTimeAideWindowClient"] = value;
            }
        }
        public static UserFilterInSession UserFilterInSession
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["UserFilterInSession"] == null)
                    return new UserFilterInSession();
                return (UserFilterInSession)HttpContext.Current.Session["UserFilterInSession"];
            }
            set
            {
                HttpContext.Current.Session["UserFilterInSession"] = value;
            }
        }
        public static ApplicantFilterInSession ApplicantFilterInSession
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["ApplicantFilterInSession"] == null)
                    return new ApplicantFilterInSession();
                return (ApplicantFilterInSession)HttpContext.Current.Session["ApplicantFilterInSession"];
            }
            set
            {
                HttpContext.Current.Session["ApplicantFilterInSession"] = value;
            }
        }
        public static string GetIPAddress()
        {
            var IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IPAddress))
            {
                IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return IPAddress;

            //string IPv6Address1 = "2400:adc1: 1ea: 2300::2";
            //string IPv6Address = "2400:adc1: 1ea: 2300:a844: d6c: ea1e: a560";
            //string TemporaryIPv6Address1 = "2400:adc1: 1ea: 2300:3cbc: 21e9:514d:a179";
            //string TemporaryIPv6Address = "2400:adc1: 1ea: 2300:a582: f2d2: f79d: b664";
            //string LinklocalIPv6Address = "fe80::a844:d6c: ea1e: a560 % 22";
            string IPv4Address = "134.201.250.155";
            //string SubnetMask = "255.255.255.0";
            //string DefaultGateway1 = "fe80::1 % 22";
            //string DefaultGateway = "192.168.18.1";
            //return IPv4Address;
        }
        public static NotificationFilterViewModel NotificationFilterViewModel
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["NotificationFilterViewModel"] == null)
                    return new NotificationFilterViewModel();
                return (NotificationFilterViewModel)HttpContext.Current.Session["NotificationFilterViewModel"];
            }
            set
            {
                HttpContext.Current.Session["NotificationFilterViewModel"] = value;
            }
        }
        public static string ConvertIpAddressToIpNumber(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return "";

            string[] ipAddressParts = ipAddress.Split('.');
            if(ipAddressParts.Length<4)
                return "";
            //Let assume the IP Address is A.B.C.D.
            string a = (256 * 256 * 256 * Convert.ToInt32(ipAddressParts[0]) +
                       (256 * 256 * Convert.ToInt32(ipAddressParts[1])) +
                       (256 * Convert.ToInt32(ipAddressParts[2])) +
                       Convert.ToInt32(ipAddressParts[2])).ToString();
            //IP Number, X = A x(256 * 256 * 256) + B x(256 * 256) + C x 256 + D



            return a;
        }

        public static string GetDeviceName()
        {
            return HttpContext.Current.Request.UserAgent;
        }
        public static string GetBrowserInformation()
        {
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            //string s = "Browser Capabilities\n"
            string s = "{"
           + "\"Type\":\"" + browser.Type +"\","
           + "\"Name\":\"" + browser.Browser +"\","
           + "\"Version\":\"" + browser.Version +"\","
           + "\"MajorVersion\":\"" + browser.MajorVersion +"\","
           + "\"MinorVersion\":\"" + browser.MinorVersion +"\","
           + "\"Platform\":\"" + browser.Platform +"\","
           + "\"IsBeta\":\"" + browser.Beta +"\","
           + "\"IsCrawler\":\"" + browser.Crawler +"\","
           + "\"IsAOL\":\"" + browser.AOL +"\","
           + "\"IsWin16\":\"" + browser.Win16 +"\","
           + "\"IsWin32\":\"" + browser.Win32 +"\","
           + "\"SupportsFrames\":\"" + browser.Frames +"\","
           + "\"SupportsTables\":\"" + browser.Tables +"\","
           + "\"SupportsCookies\":\"" + browser.Cookies +"\","
           + "\"SupportsVBScript\":\"" + browser.VBScript +"\","
           + "\"SupportsJavaScript\":\"" +
               browser.EcmaScriptVersion.ToString() +"\","
           + "\"SupportsJavaApplets\":\"" + browser.JavaApplets +"\","
           + "\"SupportsActiveXControls\":\"" + browser.ActiveXControls
                 +"\","
           + "\"SupportsJavaScriptVersion\":\"" +
               browser["JavaScriptVersion"] +"\"";
            s += "}";
            return s;
        }

        //public static string GetUserBrowser(string userAgent)
        //{
        //    // get a parser with the embedded regex patterns
        //    var uaParser = Parser.GetDefault();
        //    ClientInfo c = uaParser.Parse(userAgent);
        //    return c.UserAgent.Family;
        //}


        public static string GetApplicationUrl()
        {
            var appUrl = ConfigurationManager.AppSettings["AppUrl"];
            if (string.IsNullOrEmpty(appUrl))
            {
                var request = HttpContext.Current.Request;
                var baseUrl = request.Url.Scheme + "://" + request.Url.Authority;
                appUrl = baseUrl;
            }
            if (!appUrl.EndsWith("/") && !appUrl.EndsWith("\\"))
                appUrl = appUrl + "/";
            return appUrl;
        }
        //public static string GetUserOS(string userAgent)
        //{
        //    // get a parser with the embedded regex patterns
        //    var uaParser = Parser.GetDefault();
        //    ClientInfo c = uaParser.Parse(userAgent);
        //    return c.OS.Family;
        //}
        public static string CurrentBowser
        {

            get
            {
                return System.Web.HttpContext.Current.Request.Browser.Browser;
            }
        }
    }
    public static class JobPortalSession
    {
        public static int JobPortalClientId { get; set; }

        public static int JobPortalCompanyId { get; set; }

        public static string JobPortalEntityName { get; set; }

    }
    public class UserFilterInSession
    {
        public int? DepartmentId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? EmploymentTypeId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? SupervisorId { get; set; }

        public bool IsFilterApllied
        {
            get
            {
                if(DepartmentId==null&& SubDepartmentId==null&&
                   PositionId==null&& EmploymentTypeId==null&& EmployeeTypeId==null
                   && EmployeeStatusId==null && SupervisorId == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    public class ApplicantFilterInSession
    {
        public string ApplicantName { get; set; }
        public int? PositionId { get; set; }
        public int? ApplicantStatusId { get; set; }
        public int? LocationId { get; set; }
       
        public IEnumerable<dynamic> QACriteriaFilters { get; set; }

        public bool IsFilterApllied
        {
            get
            {
                if (PositionId == null && ApplicantStatusId == null &&
                   PositionId == null && LocationId == null && QACriteriaFilters == null)                  
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
    public class NotificationFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? WorkflowTriggerTypeId { get; set; }
        public int? NotificationTypeId { get; set; }
        public int? WorkflowStatusId { get; set; }
        public int? NotificationStatusId { get; set; }
        public string SelectedWorkflowStatusId { get; set; }
    }
    public class EmailBlastEmployeesFilterViewModel
    {
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? EmployeeStatusId { get; set; }
    }
}
