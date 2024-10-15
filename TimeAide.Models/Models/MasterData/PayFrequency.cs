namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PayFrequency")]
    public partial class PayFrequency : BaseEntity
    {
        public PayFrequency()
        {
            PayInformationHistory = new HashSet<PayInformationHistory>();
        }

        [Column("PayFrequencyId")]
        [Display(Name = "Id")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Pay Frequency")]
        public string PayFrequencyName { get; set; }

        [StringLength(200)]
        [Display(Name = "Pay Frequency Description")]
        public string PayFrequencyDescription { get; set; }

        [Display(Name = "Hourly Multiplier")]
        public decimal? HourlyMultiplier { get; set; }

        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }
    }
}
