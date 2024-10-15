using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("CobraStatus")]
    public class CobraStatus : BaseEntity
    {
        public CobraStatus()
        {
            EmployeeDentalInsurance = new HashSet<EmployeeDentalInsurance>();
            EmployeeHealthInsurance = new HashSet<EmployeeHealthInsurance>();
        }
        [Column("CobraStatusId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Cobra Status")]
        public string CobraStatusName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<EmployeeDentalInsurance> EmployeeDentalInsurance { get; set; }

        
        public virtual ICollection<EmployeeHealthInsurance> EmployeeHealthInsurance { get; set; }
    }
}
