using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("InsuranceType")]
    public class InsuranceType : BaseEntity
    {
        public InsuranceType()
        {
            EmployeeDentalInsurance = new HashSet<EmployeeDentalInsurance>();
            EmployeeHealthInsurance = new HashSet<EmployeeHealthInsurance>();
        }

        [Column("InsuranceTypeId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Insurance Type")]
        public string InsuranceTypeName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeeDentalInsurance> EmployeeDentalInsurance { get; set; }

        
        public virtual ICollection<EmployeeHealthInsurance> EmployeeHealthInsurance { get; set; }
    }
}
