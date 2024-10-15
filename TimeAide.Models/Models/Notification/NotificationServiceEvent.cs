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
    [Table("NotificationServiceEvent")]
    public class NotificationServiceEvent : BaseCompanyObjects
    {

        
        public NotificationServiceEvent()
        {
        }
        [Display(Name = "Notification Service Event Id")]
        [Column("NotificationServiceEventId")]
        public override int Id { get; set; }
        public int EventTypeId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventMessage { get; set; }


    }
}
