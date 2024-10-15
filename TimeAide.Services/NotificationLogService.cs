using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class NotificationLogService
    {
        public static void CreateNotificationLog()
        {
            TimeAideContext db = new TimeAideContext();
            NotificationLog notificationLog = new NotificationLog();
        }

        public static string GetNotifiationMessage(int messageId)
        {
            TimeAideContext db = new TimeAideContext();
            var level = db.NotificationMessage.FirstOrDefault(n => n.Id == messageId);
            if (level != null && !string.IsNullOrEmpty(level.Message))
                return level.Message;
            return "";
        }
    }
}
