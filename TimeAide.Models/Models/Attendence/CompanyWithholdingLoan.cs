namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyWithholdingLoan")]
    public partial class CompanyWithholdingLoan : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("CompanyWithholding")]
        [Display(Name = "CompanyWithholdingLoanId")]
        [Column("CompanyWithholdingLoanId")]
        public override int Id { get; set; }

        public int CompanyWithholdingId { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanDescription { get; set; }

        public decimal LoanAmount { get; set; }

        //public decimal LoanPaymentAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public int LoanPeriodLengthId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public virtual CompanyWithholding CompanyWithholding { get; set; }
    }
}
