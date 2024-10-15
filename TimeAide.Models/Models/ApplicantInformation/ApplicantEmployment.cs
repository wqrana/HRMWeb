
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantEmployment")]
    public partial class ApplicantEmployment : BaseApplicantObjects
    {
        [Display(Name = "Applicant Application Id")]
        [Column("ApplicantEmploymentId")]
        public override int Id { get; set; }
        public int? ApplicantCompanyId { get; set; }
        public int? ApplicantPositionId { get; set; }
        public int? ApplicantExitTypeId { get; set; }
        public string CompanyTelephone { get; set; }
        public string CompanyAddress { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public bool? IsCurrentEmployment { get; set; }
        public decimal? Rate { get; set; }
        public int? RateFrequencyId { get; set; }
        public string SuperviorName { get; set; }
        public string ExitReason { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string PositionName { get; set; }
        public virtual ApplicantCompany ApplicantCompany { get; set; }

        public virtual ApplicantPosition ApplicantPosition { get; set; }

        public virtual ApplicantExitType ApplicantExitType { get; set; }

        public virtual RateFrequency RateFrequency { get; set; }       

    }
}
