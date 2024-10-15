using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("InsuranceStatus")]
    public class InsuranceStatus : BaseEntity
    {
        public InsuranceStatus()
        {
            EmployeeDentalInsurance = new HashSet<EmployeeDentalInsurance>();
            EmployeeHealthInsurance = new HashSet<EmployeeHealthInsurance>();
        }

        [Column("InsuranceStatusId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Insurance Status")]
        public string InsuranceStatusName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeeDentalInsurance> EmployeeDentalInsurance { get; set; }

        
        public virtual ICollection<EmployeeHealthInsurance> EmployeeHealthInsurance { get; set; }
    }
}
