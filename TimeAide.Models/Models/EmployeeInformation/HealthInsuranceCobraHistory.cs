using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAide.Web.Models
{
    [Table("HealthInsuranceCobraHistory")]
    public class HealthInsuranceCobraHistory : BaseEntity
    {
        [Column("HealthInsuranceCobraHistoryId")]
        public override int Id { get; set; }
        public int? EmployeeHealthInsuranceId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? CobraPaymentStatusId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public virtual CobraPaymentStatus CobraPaymentStatus { get; set; }
        public virtual EmployeeHealthInsurance EmployeeHealthInsurance { get; set; }

    }
}
