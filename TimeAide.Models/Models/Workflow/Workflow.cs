using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class Workflow : BaseCompanyObjects
    {
        public Workflow()
        {
            WorkflowLevel = new HashSet<WorkflowLevel>();
            WorkflowTrigger = new HashSet<WorkflowTrigger>();
        }
        [Display(Name = "Worflow Id")]
        [Column("WorkflowId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Workflow Name")]
        public string WorkflowName { get; set; }
        [Display(Name = "Is Zero Level")]
        public Boolean IsZeroLevel { get; set; }
        [Display(Name = "Workflow Description")]
        public string WorkflowDescription { get; set; }
        [Display(Name = "Number of Levels")]
        [NotMapped]
        public int NumberOfLevels
        {
            get
            {
                if (WorkflowLevel != null && WorkflowLevel.Where(w => w.DataEntryStatus == 1).Count() > 0)
                    return WorkflowLevel.Where(w => w.DataEntryStatus == 1).Count();
                return 0;
            }
        }
        [Display(Name = "Closing Notification Type Id")]
        public virtual int ClosingNotificationId { get; set; }
        public ClosingNotificationType ClosingNotification { get; set; }
        public virtual ICollection<WorkflowLevel> WorkflowLevel { get; set; }
        public virtual ICollection<WorkflowTrigger> WorkflowTrigger { get; set; }
        [Required]
        public int? ClosingNotificationMessageId { get; set; }
        // [ForeignKey("ClosingNotificationMessageId")]
        public virtual NotificationMessage ClosingNotificationMessage { get; set; }
        public int? ReminderNotificationMessageId { get; set; }
        //  [ForeignKey("ReminderNotificationMessageId")]
        public virtual NotificationMessage ReminderNotificationMessage { get; set; }
        public int? CancelNotificationMessageId { get; set; }
        //  [ForeignKey("CancelNotificationMessageId")]
        public virtual NotificationMessage CancelNotificationMessage { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.WorkflowTrigger.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct().ToList();
            if (this.ClosingNotificationMessage != null && this.ClosingNotificationMessage.CompanyId.HasValue)
                list.Add(this.ClosingNotificationMessage.CompanyId);
            if (this.ReminderNotificationMessage != null && this.ReminderNotificationMessage.CompanyId.HasValue)
                list.Add(this.ReminderNotificationMessage.CompanyId);
            if (this.CancelNotificationMessage != null && this.CancelNotificationMessage.CompanyId.HasValue)
                list.Add(this.CancelNotificationMessage.CompanyId);
            return list.ToList();
        }

    }
}
