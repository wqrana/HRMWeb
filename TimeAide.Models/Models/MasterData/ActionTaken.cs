using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ActionTaken")]
    public partial  class ActionTaken : BaseEntity
    {
        public ActionTaken()
        {
            EmployeePerformance = new HashSet<EmployeePerformance>();
        } 

        [Column("ActionTakenId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Action Name")]
        [StringLength(150)]
        public string ActionTakenName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string ActionTakenDescription { get; set; }
        public virtual ICollection<EmployeePerformance> EmployeePerformance { get; set; }
    }
}
