using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;

namespace TimeAide.Web.Helpers
{
    public enum ErrorLogType
    {
        Error,
        Warning,
        Info
        
    }
    public class ErrorLogHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void InsertLog(ErrorLogType logType, string message, ControllerContext controllerContext)
        {
            string actionName = controllerContext.RouteData.Values["action"].ToString();
            string controllerName = controllerContext.RouteData.Values["controller"].ToString();
            var idVal = controllerContext.RouteData.Values["id"];
            string idValStr = idVal == null ? "" : idVal.ToString();
            string loginDetail = string.Format("ClientId:{0};CompanyId:{1};UserId:{2}", SessionHelper.ClientId, SessionHelper.CompanyId, SessionHelper.LoginId);
            string logMessage = String.Format("Route Info:{0}/{1}/{2}, Session Info:{3}, Issue:{4}", controllerName, actionName, idValStr, loginDetail, message);
            if (!string.IsNullOrEmpty(logMessage))
            {
                switch (logType)
                {
                    case ErrorLogType.Error:
                         logger.Error(logMessage);
                        break;
                    case ErrorLogType.Warning:
                        logger.Warn(logMessage);
                        break;
                    case ErrorLogType.Info:
                        logger.Info(logMessage);
                        break;
                }
               
            }
        }
        public static void InsertLog(ErrorLogType logType, Exception exception, ControllerContext controllerContext)
        {
            string requestType= controllerContext.HttpContext.Request.HttpMethod;
            string actionName = controllerContext.RouteData.Values["action"].ToString();
            string controllerName = controllerContext.RouteData.Values["controller"].ToString();
            int clientId = controllerName=="JobPortal"?JobPortalSession.JobPortalClientId:SessionHelper.SelectedClientId;
            int companyId = controllerName == "JobPortal" ? JobPortalSession.JobPortalCompanyId:SessionHelper.SelectedCompanyId;
            int loginId = controllerName == "JobPortal" ? 0 : SessionHelper.LoginId;
            var idVal = controllerContext.RouteData.Values["id"];
            var form = controllerContext.HttpContext.Request.Form;
            var queryString = controllerContext.HttpContext.Request.QueryString;
            StringBuilder formStr = new StringBuilder();
            if (form != null)
            {
                foreach (var key in form.AllKeys)
                {
                    formStr.Append(String.Format("Key:{0},Value:{1}|", key, form[key]));
                }
            }
            string idValStr = idVal == null ? "" : idVal.ToString();
            string queryStringStr = queryString == null ? "" : queryString.ToString();
            string loginDetail = string.Format("ClientId:{0}|CompanyId:{1}|UserId:{2}", clientId, companyId, loginId);           
            string logMessage = String.Format("Route Info:({0}){1}/{2}/{3};{6}, Session Info:{5}, POST Values:[{4}]", requestType, controllerName, actionName, idValStr, formStr.ToString(), loginDetail, queryStringStr);
            if (exception!=null)
            {
                if (logType == ErrorLogType.Error)
                {
                    logger.Error( exception, logMessage);

                }
            }
        }

    }
}