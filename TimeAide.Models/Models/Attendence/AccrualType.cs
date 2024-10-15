namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("AccrualType")]
    public partial class AccrualType : BaseCompanyObjects
    {
        
        public AccrualType()
        {
        }

        [Display(Name = "Accrual Type Id")]
        [Column("AccrualTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Accrual Type Name")]
        public string AccrualTypeName { get; set; }


        [StringLength(150)]
        [Display(Name = "Description")]
        public string AccrualTypeDescription { get; set; }

        public bool? IsUsedBySystem { get; set; }

    }
}
