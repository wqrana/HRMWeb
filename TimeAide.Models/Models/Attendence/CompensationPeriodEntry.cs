namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationPeriodEntry")]
    public partial class CompensationPeriodEntry:BaseGlobalEntity
    {
        [Display(Name = "CompensationPeriodEntryId")]
        [Column("CompensationPeriodEntryId")]
        public override int Id { get; set; }

        [Required]
        public string CompensationPeriodEntryName { get; set; }

        public string CompensationPeriodEntryDescription { get; set; }

    }
}
