namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyWithholdingPRPayExport")]
    public partial class CompanyWithholdingPRPayExport : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("CompanyWithholding")]
        [Display(Name = "CompanyWithholdingPRPayExportId")]
        [Column("CompanyWithholdingPRPayExportId")]
        public override int Id { get; set; }


        public int CompanyWithholdingId { get; set; }
        public virtual CompanyWithholding CompanyWithholding { get; set; }

        public bool CustomDeduction1 { get; set; }

        public bool CustomDeduction2 { get; set; }

        public bool CustomDeduction3 { get; set; }

        public bool CustomDeduction4 { get; set; }

        public bool CustomDeduction5 { get; set; }

        public bool IsWithholding { get; set; }

        public bool IsFICA { get; set; }

        public bool IsSocialSecurity { get; set; }

        public bool IsMedicare { get; set; }

        public bool IsDisability { get; set; }

        public bool IsChauffeurInsurance { get; set; }

        public bool IsOtherDeduction { get; set; }

        public bool IsHealthCoverageContribution { get; set; }

        public bool Charitable_Contribution { get; set; }

        public bool IsMoneySaving { get; set; }

        public bool IsMedicarePlus { get; set; }

        public bool IsCoda401K { get; set; }

        public bool IsCodaExemptSalary { get; set; }

        public bool IsMedicalInsurance { get; set; }
    }
}
