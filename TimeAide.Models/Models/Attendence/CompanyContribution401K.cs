namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CompanyContribution401K : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("CompanyContribution")]
        [Display(Name = "CompanyContribution401KId")]
        [Column("CompanyContribution401KId")]
        public override int Id { get; set; }
        public string PlanDescription { get; set; }
        public decimal EmployerPeriodMax { get; set; }
        public decimal EmployerMatchPercentage { get; set; }
        public decimal EmployerPercentageLimitType { get; set; }
        public bool Is401K1165eStTaxExempted { get; set; }
        public int Withholding401KTypeId { get; set; }
        public int CompanyContributionId { get; set; }
        public virtual CompanyContribution CompanyContribution { get; set; }
    }
}
