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

    [Table("NotificationSchedule")]
    public partial class NotificationSchedule : BaseCompanyObjects
    {
        
        public NotificationSchedule()
        {
            NotificationScheduleDetail = new HashSet<NotificationScheduleDetail>();
            Document = new HashSet<Document>();
            Credential = new HashSet<Credential>();
            CustomField = new HashSet<CustomField>();
        }
        [Display(Name = "Notification Schedule")]
        [Column("NotificationScheduleId")]
        public override int Id { get; set; }

        [Display(Name = "Notification Schedule")]
        [Required]
        [StringLength(50)]
        public string NotificationScheduleName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string NotificationScheduleDescription { get; set; }

        
        public virtual ICollection<NotificationScheduleDetail> NotificationScheduleDetail { get; set; }

        
        public virtual ICollection<Document> Document { get; set; }
        public virtual ICollection<Credential> Credential { get; set; }
        public virtual ICollection<CustomField> CustomField { get; set; }
    }
}
