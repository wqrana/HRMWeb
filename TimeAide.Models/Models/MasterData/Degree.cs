using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("Degree")]
    public class Degree : BaseEntity
    {
        public Degree()
        {
            EmployeeEducation = new HashSet<EmployeeEducation>();
        }

        [Column("DegreeId")]
        public override int Id { get; set ; }

        [Required]
        [Display(Name ="Degree Name")]
        public string DegreeName { get; set; }
        [Display(Name = "Degree Description")]
        public string DegreeDescription { get; set; }
        public virtual ICollection<EmployeeEducation> EmployeeEducation { get; set; }
    }
}
