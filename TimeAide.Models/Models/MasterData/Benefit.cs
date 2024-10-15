using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("Benefit")]
    public class Benefit : BaseEntity
    {
        public Benefit()
        {
            EmployeeBenefitEnlisted = new HashSet<EmployeeBenefitEnlisted>();
            EmployeeBenefitHistory = new HashSet<EmployeeBenefitHistory>();
        }

        [Column("BenefitId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Benefit Name")]
        public string BenefitName { get; set; }
        [Display(Name = "Benefit Description")]
        public string BenefitDescription { get; set; }
        public virtual ICollection<EmployeeBenefitEnlisted> EmployeeBenefitEnlisted { get; set; }
        public virtual ICollection<EmployeeBenefitHistory> EmployeeBenefitHistory { get; set; }
    }
}
