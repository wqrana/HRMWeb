using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.SendNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            NotificationScheduleServiceManager manager = new NotificationScheduleServiceManager();

            try
            {
                if (args.Length==0)
                {
                    manager.MessageLogging("Database Name needed:  ");
                    manager.ErrorNotification("", manager.LogFileName);
                    return;
                }
                manager.DatabaseName = args[0];
                
                if(!manager.TimeAideContext.Database.Exists())
                {
                    manager.MessageLogging("Database does not Exists:  " + args[0]);
                    manager.ErrorNotification(args[0], manager.LogFileName);
                    return;
                }
                DateTime schedule = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
                manager.MessageLogging("Processing Notifications for date: " + DateTime.Now);
                //NotificationServiceEventManager.AddNotificationServiceEvent(null, null, 1, "Processing Notifications for date: " + DateTime.Now);
                schedule = schedule.AddHours(24);
                manager.GenerateLog(manager);
                //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(null, null, 4, "Executed TimeAide.NotificationService");
                manager.MessageLogging("Executed TimeAide.NotificationService");
            }
            catch (Exception ex)
            {
                manager.ErrorLogging(ex);
                manager.ErrorNotification(args[0], manager.LogFileName);
            }
        }


    }
}
