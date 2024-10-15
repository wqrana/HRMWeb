using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.NotificationService
{
    public partial class NotificationScheduleService : ServiceBase
    {
        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(null, null, 1, "ServiceTimer_Ticked");

            if (DateTime.Now > schedule)
            {
                TimeAideContext db = new TimeAideContext();
                schedule = schedule.AddHours(24);
                foreach (var eachClient in db.GetAll<Client>(1))
                {
                    //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(eachClient.ClientId, null, 1, "Processing client " + eachClient.ClientName);
                    foreach (var eachCompany in db.GetAllByCompany<Company>(1, 1))
                    {
                        //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(eachClient.ClientId, eachCompany.Id, 2, "Processing company " + eachCompany.CompanyName + "/" + eachClient.ClientName);
                        //TimeAide.Services.NotificationScheduleService.AddNotificationServiceEvent();
                        //TimeAide.Services.NotificationServiceEventManager scheduleService = new TimeAide.Services.NotificationServiceEventManager();
                        //scheduleService.GetDocumentsNotificationAletrs(eachClient.Id, eachCompany.Id);
                        //scheduleService.GetCredentialsNotificationAletrs(eachClient.Id, eachCompany.Id);
                    }

                    //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(eachClient.ClientId, null, 4, "Executed TimeAide.NotificationService");
                }

            }
        }
        DateTime schedule;
        protected override void OnStart(string[] args)
        {
            schedule = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 27, 0);
            timer1.AutoReset = true;
            timer1.Enabled = true;
            //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(null, null, 1, "Daily TimeAide.NotificationService started");
        }

        protected override void OnStop()
        {
            timer1.AutoReset = false;
            timer1.Enabled = false;
            //TimeAide.Services.NotificationServiceEventManager.AddNotificationServiceEvent(null, null, 1, "Daily TimeAide.NotificationService stopped");
        }

        System.Timers.Timer timer1;
        public NotificationScheduleService()
        {
            InitializeComponent();
            timer1 = new System.Timers.Timer();
            double inter = (double)1000;
            timer1.Interval = inter;
            timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
        }

    }
}
