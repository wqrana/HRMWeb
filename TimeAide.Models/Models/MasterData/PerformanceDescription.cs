using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("PerformanceDescription")]
    public partial class PerformanceDescription : BaseEntity
    {
        public PerformanceDescription()
        {
            EmployeePerformance = new HashSet<EmployeePerformance>();
        }

        [Column("PerformanceDescriptionId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Description Name")]
        public string PerformanceDescriptionName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeePerformance> EmployeePerformance { get; set; }

    }
}
