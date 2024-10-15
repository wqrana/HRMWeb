using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    [Table("NotificationScheduleDetail")]
    public partial class NotificationScheduleDetail : BaseEntity
    {
        public NotificationScheduleDetail()
        {
            NotificationLog = new HashSet<NotificationLog>();
            NotificationScheduleEmployeeGroup = new HashSet<NotificationScheduleEmployeeGroup>();

        }

        [Column("NotificationScheduleDetailId")]
        public override int Id { get; set; }
        public int DaysBefore { get; set; }
        public int NotificationScheduleId { get; set; }
        public virtual NotificationSchedule NotificationSchedule { get; set; }

        public virtual ICollection<NotificationLog> NotificationLog { get; set; }
        public int NotificationMessageId { get; set; }
        public virtual NotificationMessage NotificationMessage { get; set; }
        public virtual bool ExcludeUser { get; set; }

        public virtual bool ExcludeSupervisor { get; set; }

        public virtual ICollection<NotificationScheduleEmployeeGroup> NotificationScheduleEmployeeGroup
        {
            get;
            set;
        }

        [NotMapped]
        public int DaysBeforePrevious
        {
            get
            {
                if (NotificationSchedule == null || NotificationSchedule.NotificationScheduleDetail.Count == 0)
                    return 0;
                var detail = NotificationSchedule.NotificationScheduleDetail.Where(d => d.Id != Id && d.DaysBefore < DaysBefore).OrderByDescending(d => d.DaysBefore).FirstOrDefault();
                if (detail != null)
                    return detail.DaysBefore;
                return 0;
            }
        }

        [NotMapped]
        public string SelectedEmployeeGroupId { get; set; }

        [NotMapped]
        public string SelectedNotificationEmployeeGroupNames
        {
            get
            {
                if (NotificationScheduleEmployeeGroup != null && NotificationScheduleEmployeeGroup.Count > 0)
                    return String.Join(",", NotificationScheduleEmployeeGroup.Where(w => w.DataEntryStatus == 1).Select(c => c.EmployeeGroup.EmployeeGroupName.ToString()));
                return "";
            }

        }

    }
}
