namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("CompensationAccrualType")]
    public partial class CompensationAccrualType : BaseGlobalEntity
    {
        
        public CompensationAccrualType()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Accrual Type Id")]
        [Column("CompensationAccrualTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Accrual Type Name")]
        public string CompensationAccrualTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string CompensationAccrualTypeDescription { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
