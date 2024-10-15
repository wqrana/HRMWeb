namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("PayRateMultiplier")]
    public partial class PayRateMultiplier : BaseCompanyObjects
    {
        
        public PayRateMultiplier()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Pay Rate Multiplier Id")]
        [Column("PayRateMultiplierId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Multiplier")]
        public string PayRateMultiplierName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string PayRateMultiplierDescription { get; set; }

        [Display(Name = "Rate Multiplier Value")]
        public Decimal PayRateMultiplierValue { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
