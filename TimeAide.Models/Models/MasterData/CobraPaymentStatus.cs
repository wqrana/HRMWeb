using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("CobraPaymentStatus")]
    public class CobraPaymentStatus :BaseEntity
    {
        public CobraPaymentStatus()
        {
            DentalInsuranceCobraHistory = new HashSet<DentalInsuranceCobraHistory>();
            HealthInsuranceCobraHistory = new HashSet<HealthInsuranceCobraHistory>();
        }

        [Column("CobraPaymentStatusId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Payment Status")]
        public string CobraPaymentStatusName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public virtual ICollection<DentalInsuranceCobraHistory> DentalInsuranceCobraHistory { get; set; }

        
        public virtual ICollection<HealthInsuranceCobraHistory> HealthInsuranceCobraHistory { get; set; }
    }
}
