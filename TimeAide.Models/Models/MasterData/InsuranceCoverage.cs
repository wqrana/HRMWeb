using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("InsuranceCoverage")]
   public class InsuranceCoverage : BaseEntity
    {
        public InsuranceCoverage()
        {
            EmployeeDentalInsurance = new HashSet<EmployeeDentalInsurance>();
            EmployeeHealthInsurance = new HashSet<EmployeeHealthInsurance>();
        }

        [Column("InsuranceCoverageId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Insurance Status")]
        public string InsuranceCoverageName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeeDentalInsurance> EmployeeDentalInsurance { get; set; }

        
        public virtual ICollection<EmployeeHealthInsurance> EmployeeHealthInsurance { get; set; }
    }
}
