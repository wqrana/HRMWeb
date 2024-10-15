namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationComputationType")]
    public partial class CompensationComputationType : BaseGlobalEntity
    {
        [Display(Name = "CompensationComputationTypeId")]
        [Column("CompensationComputationTypeId")]
        public override int Id { get; set; }

        [Required]
        public string CompensationComputationTypeName { get; set; }

        public string CompensationComputationTypeDescription { get; set; }

    }
}
