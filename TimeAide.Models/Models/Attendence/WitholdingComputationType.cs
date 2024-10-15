namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WitholdingComputationType")]
    public partial class WitholdingComputationType : BaseGlobalEntity
    {
        [Display(Name = "WitholdingComputationTypeId")]
        [Column("WitholdingComputationTypeId")]
        public override int Id { get; set; }

        [Required]
        public string WitholdingComputationTypeName { get; set; }

        public string WitholdingComputationTypeDescription { get; set; }
    }
}
