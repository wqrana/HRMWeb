namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("VacationAccrualType")]
    public partial class VacationAccrualType : BaseGlobalEntity
    {
        
        public VacationAccrualType()
        {
            TransactionConfiguration = new HashSet<TransactionConfiguration>();
        }

        [Display(Name = "Accrual Type Id")]
        [Column("VacationAccrualTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Accrual Type Name")]
        public string VacationAccrualTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string VacationAccrualTypeDescription { get; set; }
        public virtual ICollection<TransactionConfiguration> TransactionConfiguration { get; set; }

    }
}
