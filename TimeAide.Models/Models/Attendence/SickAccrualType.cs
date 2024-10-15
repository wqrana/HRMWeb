namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("SickAccrualType")]
    public partial class SickAccrualType : BaseGlobalEntity
    {
        
        public SickAccrualType()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Sick Accrual Type Id")]
        [Column("SickAccrualTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Accrual Type Name")]
        public string SickAccrualTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string SickAccrualTypeDescription { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
