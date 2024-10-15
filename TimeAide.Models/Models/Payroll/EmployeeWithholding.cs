namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeWithholding")]
    public partial class EmployeeWithholding: BaseUserObjects
    {
        [Display(Name = "EmployeeWithholdingId")]
        [Column("EmployeeWithholdingId")]
        public override int Id { get; set; }

        [Required]
        public int CompanyWithholdingId { get; set; }

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

        public bool Apply401kPlan { get; set; }

        public int PeriodEntryId { get; set; }

        public virtual PeriodEntry PeriodEntry { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public virtual CompanyWithholding CompanyWithholding { get; set; }
        public bool Enabled { get; set; }
        [Display(Name = "Money Amount")]
        public decimal MoneyAmount { get; set; }

        [Display(Name = "Loan Amount")]
        public decimal? LoanAmount { get; set; }

        [Display(Name = "Loan Number")]
        public int? LoanNumber { get; set; }

        [Display(Name = "Remaining Balance")]
        public decimal? RemainingBalance { get; set; }

    }
}
