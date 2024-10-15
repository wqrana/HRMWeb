namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeCompensation")]
    public partial class EmployeeCompensation: BaseUserObjects
    {
        [Display(Name = "EmployeeCompensationId")]
        [Column("EmployeeCompensationId")]
        public override int Id { get; set; }

        public int CompanyCompensationId { get; set; }

        public bool Enabled { get; set; }
        [Display(Name = "Money Amount")]
        public decimal MoneyAmount { get; set; }

        public int? GLAccountId { get; set; }

        public int PeriodEntryId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public virtual CompanyCompensation CompanyCompensation { get; set; }

        public virtual CompensationPeriodEntry PeriodEntry { get; set; }

        public virtual GLAccount GLAccount { get; set; }
    }
}
