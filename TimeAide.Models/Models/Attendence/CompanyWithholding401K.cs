namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CompanyWithholding401K : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("CompanyWithholding")]
        [Display(Name = "CompanyWithholding401KId")]
        [Column("CompanyWithholding401KId")]
        public override int Id { get; set; }
        public decimal EEMaxYearlyAmount { get; set; }
        public decimal ERMaxYearlyAmount { get; set; }
        public bool Is401K1165eStTaxExempted { get; set; }
        public int Withholding401KTypeId { get; set; }
        public virtual Withholding401KType Withholding401KType { get; set; }
        public int CompanyWithholdingId { get; set; }
        public virtual CompanyWithholding CompanyWithholding { get; set; }
    }
}
