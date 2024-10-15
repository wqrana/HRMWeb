namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeNotification")]
    public partial class EmployeeNotification : BaseCompanyObjects
    {
        
        public EmployeeNotification()
        {
        }

        [Display(Name = "Notification Id")]
        [Column("EmployeeNotificationId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Notification")]
        public string EmployeeNotificationName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmployeeNotificationDescription { get; set; }
        public int EmployeeNotificationTypeId { get; set; }
        public virtual EmployeeNotificationType EmployeeNotificationType { get; set; }

        public int NotificationScheduleId { get; set; }
        public virtual NotificationSchedule NotificationSchedule { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
    }
}
