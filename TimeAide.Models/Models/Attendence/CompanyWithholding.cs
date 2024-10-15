namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyWithholding")]
    public partial class CompanyWithholding:BaseCompanyObjects
    {
        [Display(Name = "CompanyWithholdingId")]
        [Column("CompanyWithholdingId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string WithHoldingName { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public int WitholdingPrePostTypeId { get; set; }
        public virtual WitholdingPrePostType WitholdingPrePostType { get; set; }

        public int WitholdingComputationTypeId { get; set; }
        public virtual WitholdingComputationType WitholdingComputationType { get; set; }

        public int WithholdingTaxTypeId { get; set; }
        public virtual WithholdingTaxType WithholdingTaxType { get; set; }

        public decimal EmployeeWithholdingPercentage { get; set; }

        public decimal EmployeeWithholdingAmount { get; set; }

        public decimal CompanyWithholdingPercent { get; set; }

        public decimal CompanyWithholdingAmount { get; set; }

        public decimal MaximumSalaryLimit { get; set; }

        public decimal MinimumSalaryLimit { get; set; }

        public bool IsDeleted { get; set; }

        public int intReportOrder { get; set; }

        public int GLAccountId { get; set; }
        public virtual GLAccount GLAccount { get; set; }

        public int GLLookupFieldId { get; set; }

        public bool IsLoan { get; set; }

        public bool Is401kPlan { get; set; }

        public virtual CompanyWithholdingPRPayExport CompanyWithholdingPRPayExport { get; set; }
        public virtual CompanyWithholdingLoan CompanyWithholdingLoan { get; set; }
        public virtual CompanyWithholding401K CompanyWithholding401K { get; set; }

        [NotMapped]
        public string SelectedCompensations { get; set; }
    }
}
