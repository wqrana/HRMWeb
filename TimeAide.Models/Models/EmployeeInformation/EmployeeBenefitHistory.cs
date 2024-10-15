    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAide.Web.Models
{
    [Table("EmployeeBenefitHistory")]
    public partial class EmployeeBenefitHistory : BaseUserObjects
    {
        [Column("EmployeeBenefitHistoryId")]
        public override int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? Amount { get; set; }
        public int? BenefitId { get; set; }
        public int? PayFrequencyId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set;}
        public decimal? EmployeeContribution { get; set; }
        public decimal? CompanyContribution { get; set; }
        public decimal? OtherContribution { get; set; }
        public decimal? TotalContribution { get; set; }     
        public virtual Benefit Benefit { get; set; }
        public virtual PayFrequency PayFrequency { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
