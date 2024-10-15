namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyContribution")]
    public partial class CompanyContribution : BaseCompanyObjects
    {
        [Display(Name = "CompanyContributionId")]
        [Column("CompanyContributionId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyContributionName { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public int WitholdingPrePostTypeId { get; set; }
        public virtual WitholdingPrePostType WitholdingPrePostType { get; set; }

        public int WitholdingComputationTypeId { get; set; }
        public virtual WitholdingComputationType WitholdingComputationType { get; set; }

        public int? ContributionTaxTypeId { get; set; }

        public decimal EmployeeContributionPercentage { get; set; }

        public decimal EmployeeContributionAmount { get; set; }

        public decimal CompanyContributionPercent { get; set; }

        public decimal CompanyContributionAmount { get; set; }

        public decimal MaximumSalaryLimit { get; set; }

        public decimal MinimumSalaryLimit { get; set; }

        public bool IsDeleted { get; set; }

        public int intReportOrder { get; set; }

        public int GLAccountId { get; set; }
        public virtual GLAccount GLAccount { get; set; }


        public int? GLContributionAccountId { get; set; }
        public virtual GLAccount GLContributionAccount { get; set; }
        public int? GLContributionPayableAccountId { get; set; }
        public virtual GLAccount GLContributionPayableAccount { get; set; }
        public int GLLookupFieldId { get; set; }

        //public bool IsLoan { get; set; }

        public bool Is401kPlan { get; set; }

        public virtual CompanyContributionPRPayExport CompanyContributionPRPayExport { get; set; }
        //public virtual CompanyContributionLoan CompanyContributionLoan { get; set; }
        public virtual CompanyContribution401K CompanyContribution401K { get; set; }

        [NotMapped]
        public string SelectedCompensations { get; set; }
    }
}
