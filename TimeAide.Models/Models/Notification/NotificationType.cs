using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    
    [Table("NotificationType")]
    public partial class NotificationType : BaseCompanyObjects
    {
        public NotificationType()
        {
        }
        [Display(Name = "Notification Type")]
        [Column("NotificationTypeId")]
        public override int Id { get; set; }
        
        [Display(Name = "Notification Type Name")]
        [Required]
        [StringLength(100)]
        public string NotificationTypeName { get; set; }

    }

    public enum NotificationTypes
    {
        Document = 1,
        Credential = 2,
        CustomField = 3,
        ProbationPeriod = 4,
        Performance = 5,
        TrainingExpiration = 6,
        HealthInsurance = 7,
        DentalInsurance = 8,
        ActionHistory = 9,
        Benefits = 10,
        Birthday = 11,
        WorkAnniversary = 12,
        EmailBlast = 13,
    }
}
