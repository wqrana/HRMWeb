namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyCompensation")]
    public partial class CompanyCompensation : BaseCompanyObjects
    {
        public CompanyCompensation()
        {
            CompensationTransaction = new HashSet<CompensationTransaction>();
            //CompanyCompensationPRPayExport = new HashSet<CompanyCompensationPRPayExport>();
            CompanyWithholdingCompensationExclusion = new HashSet<CompanyWithholdingCompensationExclusion>();
            CompanyContributionCompensationExclusion = new HashSet<CompanyContributionCompensationExclusion>();
        }
        [Display(Name = "CompanyCompensationId")]
        [Column("CompanyCompensationId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Compensation Name")]
        public string CompensationName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int CompensationTypeId { get; set; }

        public int ComputationTypeId { get; set; }

        public bool IsEnabled { get; set; }

        public int ImportTypeId { get; set; }

        [Required]
        public int GLAccountId { get; set; }

        public bool IsCovidCompensation { get; set; }

        public bool IsFICASSCCExempt { get; set; }

        public int? ReportOrder { get; set; }

        public int GLLookupField { get; set; }

        public virtual CompensationType CompensationType { get; set; }

        public virtual CompensationImportType ImportType { get; set; }
        public virtual CompensationComputationType ComputationType { get; set; }
        public virtual GLAccount GLAccount { get; set; }
        public virtual CompanyCompensationPRPayExport CompanyCompensationPRPayExport { get; set; }
        [NotMapped]
        public string SelectedTransactions { get; set; }
        public virtual ICollection<CompensationTransaction> CompensationTransaction { get; set; }
        public virtual ICollection<CompanyWithholdingCompensationExclusion> CompanyWithholdingCompensationExclusion { get; set; }
        public virtual ICollection<CompanyContributionCompensationExclusion> CompanyContributionCompensationExclusion { get; set; }
    }
}
