using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("PerformanceResult")]
    public partial class PerformanceResult :BaseEntity
    {
        public PerformanceResult()
        {
            EmployeePerformance = new HashSet<EmployeePerformance>();
        }

        [Column("PerformanceResultId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Result Name")]
        public string PerformanceResultName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeePerformance> EmployeePerformance { get; set; }
    }
}
