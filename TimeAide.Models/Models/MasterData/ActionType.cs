using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ActionType")]
    public partial class ActionType : BaseEntity
    {
        public ActionType()
        {
            EmployeeAction = new HashSet<EmployeeAction>();
        }
        [Column("ActionTypeId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Action Type")]
        public string ActionTypeName { get; set; }
        [Display(Name = "Description")]
        public string ActionTypeDescription { get; set; }
        public virtual ICollection<EmployeeAction> EmployeeAction { get; set; }
    }
}
