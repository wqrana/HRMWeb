namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WithholdingTaxType")]
    public partial class WithholdingTaxType : BaseGlobalEntity
    {
        [Display(Name = "WithholdingTaxTypeId")]
        [Column("WithholdingTaxTypeId")]
        public override int Id { get; set; }

        [Required]
        public string WithholdingTaxTypeName { get; set; }

        public string WithholdingTaxTypeDescription { get; set; }

    }
}
