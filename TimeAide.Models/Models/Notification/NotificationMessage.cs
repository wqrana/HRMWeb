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
    
    [Table("NotificationMessage")]
    public partial class NotificationMessage : BaseCompanyObjects
    {
        
        public NotificationMessage()
        {
            WorkflowLevel = new HashSet<WorkflowLevel>();
            Workflow = new HashSet<Workflow>();
        }
        [Display(Name = "Serial #")]
        [Column("NotificationMessageId")]
        public override int Id { get; set; }
        
        [Display(Name = "Notification Message")]
        [Required]
        [StringLength(50)]
        public string NotificationMessageName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string NotificationMessageDescription { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }
        public virtual ICollection<WorkflowLevel> WorkflowLevel { get; set; }
        public virtual ICollection<Workflow> Workflow { get; set; }
    }
}
