using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    public class ClosingNotificationType : BaseCompanyObjects
    {
        public ClosingNotificationType()
        {

        }

        [Display(Name = "Id")]
        [Column("ClosingNotificationTypeId")]
        public override int Id { get; set; }
        [Display(Name = "Name")]
        public string ClosingNotificationTypeName { get; set; }
        [Display(Name = "Description")]
        public string ClosingNotificationTypeDescription { get; set; }
    }

    public enum ClosingNotificationTypes
    { 
        Employees=1,
        EmployeeAndApprovers=2,
        All=3
    }
}
