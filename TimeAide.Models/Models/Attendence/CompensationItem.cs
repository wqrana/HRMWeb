namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationItem")]
    public partial class CompensationItem : BaseCompanyObjects
    {
        //public int CompensationItemId { get; set; }
        [Display(Name = "CompensationItemId")]
        [Column("CompensationItemId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CompensationName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int CompensationTypeId { get; set; }

        public int ComputationTypeId { get; set; }

        public bool IsEnabled { get; set; }

        public int ImportTypeId { get; set; }
        

        [Required]
        [StringLength(50)]
        public string GLAccount { get; set; }

        public bool IsCovidCompensation { get; set; }

        public bool IsFICASSCCExempt { get; set; }

        public int? ReportOrder { get; set; }

        public int GLLookupField { get; set; }

        public virtual CompensationType CompensationType { get; set; }
        public virtual CompensationImportType ImportType { get; set; }
        public virtual CompensationComputationType ComputationType { get; set; }
    }
}
