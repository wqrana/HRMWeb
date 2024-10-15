using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAide.Web.Models
{
    [Table("EmployeeNotificationType")]
    public partial class EmployeeNotificationType : BaseGlobalEntity
    {

        public EmployeeNotificationType()
        {
            EmployeeNotification = new HashSet<EmployeeNotification>();
        }

        [Display(Name = "Notification Setup Id")]
        [Column("EmployeeNotificationTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Notification")]
        public string EmployeeNotificationTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmployeeNotificationTypeDescription { get; set; }


        public virtual ICollection<EmployeeNotification> EmployeeNotification { get; set; }
    }
    public enum EmployeeNotificationTypes
    {
        ProbationPeriodNotification = 1,
    }
}
