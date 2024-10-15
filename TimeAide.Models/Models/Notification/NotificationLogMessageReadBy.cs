using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("NotificationLogMessageReadBy")]
    public class NotificationLogMessageReadBy : BaseCompanyObjects
    {


        public NotificationLogMessageReadBy()
        {
        }
        [Display(Name = "Message Read By Id")]
        [Column("NotificationLogMessageReadById")]
        public override int Id { get; set; }
        public int? NotificationLogId { get; set; }
        public virtual NotificationLog NotificationLog { get; set; }
        public int? WorkflowTriggerRequestId { get; set; }
        public virtual WorkflowTriggerRequest WorkflowTriggerRequest { get; set; }
        public int? ReadById { get; set; }
        public virtual UserInformation ReadBy { get; set; }
    }
}
