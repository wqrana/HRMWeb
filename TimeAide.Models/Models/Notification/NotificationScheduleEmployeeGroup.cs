using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    [Table("NotificationScheduleEmployeeGroup")]
    public partial class NotificationScheduleEmployeeGroup : BaseEntity
    {
        public NotificationScheduleEmployeeGroup()
        {
        }

        [Column("NotificationScheduleEmployeeGroupId")]
        public override int Id { get; set; }

        public int NotificationScheduleDetailId { get; set; }
        public virtual NotificationScheduleDetail NotificationScheduleDetail { get; set; }
        public int EmployeeGroupId { get; set; }
        public virtual EmployeeGroup EmployeeGroup { get; set; }
    }
}
