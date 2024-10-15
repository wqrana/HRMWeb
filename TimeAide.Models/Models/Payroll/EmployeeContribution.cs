namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeContribution")]
    public partial class EmployeeContribution: BaseUserObjects
    {
        [Display(Name = "EmployeeContributionId")]
        [Column("EmployeeContributionId")]
        public override int Id { get; set; }

        public int CompanyContributionId { get; set; }

        public bool Enabled { get; set; }
        [Display(Name = "Money Amount")]
        public decimal MoneyAmount { get; set; }

        public int? GLContributionAccountId { get; set; }
        public int? GLContributionPayableAccountId { get; set; }
        public int PeriodEntryId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public virtual CompanyContribution CompanyContribution { get; set; }
        public virtual CompensationPeriodEntry PeriodEntry { get; set; }
        public virtual GLAccount GLContributionAccount { get; set; }
        public virtual GLAccount GLContributionPayableAccount { get; set; }
    }
}
