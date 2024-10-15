namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Withholding401KType")]
    public partial class Withholding401KType : BaseEntity
    {
        
        public Withholding401KType()
        {
        }

        [Display(Name = "Withholding401KTypeId")]
        [Column("Withholding401KTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Withholding401KType Name")]
        public string Withholding401KTypeName { get; set; }


        [StringLength(150)]
        [Display(Name = "Description")]
        public string Withholding401KTypeDescription { get; set; }

    }
}
