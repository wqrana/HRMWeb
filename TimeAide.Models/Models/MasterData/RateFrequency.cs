namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RateFrequency")]
    public partial class RateFrequency : BaseEntity
    {
        public RateFrequency()
        {
            PayInformationHistory = new HashSet<PayInformationHistory>();
            PayScale = new HashSet<PayScale>();
        }

        [Column("RateFrequencyId")]
        [Display(Name = "Rate Frequency Id")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Rate Frequency Name")]
        public string RateFrequencyName { get; set; }

        [Display(Name = "Rate Frequency Description")]
        [StringLength(200)]
        public string RateFrequencyDescription { get; set; }

        [Display(Name = "Hourly Multiplier")]
        [Range(typeof(decimal), "0", "10000.00")]
        public decimal? HourlyMultiplier { get; set; }

        
        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }

        
        public virtual ICollection<PayScale> PayScale { get; set; }
        

    }
}
