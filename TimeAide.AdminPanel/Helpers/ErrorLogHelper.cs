using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TimeAide.Common.Helpers;

namespace TimeAide.AdminPanel.Helpers
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

        public static void InsertLog(ErrorLogType logType, string message)
        {


            string logMessage = message;
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
        
        }

    }
