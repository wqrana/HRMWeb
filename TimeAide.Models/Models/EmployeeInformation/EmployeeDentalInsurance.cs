
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{

    [Table("EmployeeDentalInsurance")]
    public class EmployeeDentalInsurance : BaseUserObjects
    {
        public EmployeeDentalInsurance()
        {
            DentalInsuranceCobraHistory = new HashSet<DentalInsuranceCobraHistory>();
        }

        [Column("EmployeeDentalInsuranceId")]
        public override int Id { get; set; }
        public bool? IsEnlisted { get; set; }
        public string GroupId { get; set; }
        public int? InsuranceCoverageId { get; set; }
        public int? InsuranceTypeId { get; set; }
        public int? InsuranceStatusId { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public int? CobraStatusId { get; set; }
        public DateTime? LeyCobraStartDate { get; set; }
        public DateTime? LeyCobraExpiryDate { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal CompanyContribution { get; set; }
        public decimal OtherContribution { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal PCORIFee { get; set; }
        public decimal? InsurancePremium { get; set; }

        public virtual InsuranceType InsuranceType { get; set; }
        public virtual InsuranceStatus InsuranceStatus { get; set; }
        public virtual InsuranceCoverage InsuranceCoverage { get; set; }
        public virtual CobraStatus CobraStatus { get; set; }
        public virtual ICollection<DentalInsuranceCobraHistory> DentalInsuranceCobraHistory { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
