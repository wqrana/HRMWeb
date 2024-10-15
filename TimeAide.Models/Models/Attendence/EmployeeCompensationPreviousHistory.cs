namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeCompensationPreviousHistory")]
    public partial class EmployeeCompensationPreviousHistory: BaseEntity
    {
        [Display(Name = "EmployeeCompensationPreviousHistoryId")]
        [Column("EmployeeCompensationPreviousHistoryId")]
        public override int Id { get; set; }

        public DateTime PayDate { get; set; }

        public decimal PeriodWages { get; set; }

        public decimal PeriodComissions { get; set; }

        public decimal PeriodHours { get; set; }
    }
}
