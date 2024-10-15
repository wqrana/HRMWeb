namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyCompensationPRPayExport")]
    public partial class CompanyCompensationPRPayExport : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("CompanyCompensation")]
        [Display(Name = "CompanyCompensationPRPayExportId")]
        [Column("CompanyCompensationPRPayExportId")]
        public override int Id { get; set; }
        public int? CompanyCompensationId { get; set; }
        public virtual CompanyCompensation CompanyCompensation { get; set; }

        public bool CustomIncome1 { get; set; }

        public bool CustomIncome2 { get; set; }

        public bool CustomIncome3 { get; set; }

        public bool CustomIncome4 { get; set; }

        public bool CustomIncome5 { get; set; }

        public bool NonTaxable1 { get; set; }

        public bool NonTaxable2 { get; set; }

        public bool NonTaxable3 { get; set; }

        public bool NonTaxable4 { get; set; }

        public bool NonTaxable5 { get; set; }

        public bool Wages { get; set; }

        public bool Commissions { get; set; }

        public bool Allowances { get; set; }

        public bool Tips { get; set; }

        public bool Income401K { get; set; }

        public bool OtherRetirement { get; set; }

        public bool Cafeteria { get; set; }

        public bool Reimbursements { get; set; }

        public bool CODA401K { get; set; }

        public bool ExemptSalaries { get; set; }
    }
}
