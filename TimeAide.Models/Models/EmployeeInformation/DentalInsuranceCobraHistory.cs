
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAide.Web.Models
{
    [Table("DentalInsuranceCobraHistory")]
    public class DentalInsuranceCobraHistory : BaseEntity
    {
        [Column("DentalInsuranceCobraHistoryId")]
        public override int Id { get; set; }
        public int? EmployeeDentalInsuranceId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? CobraPaymentStatusId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public virtual CobraPaymentStatus CobraPaymentStatus { get; set; }
        public virtual EmployeeDentalInsurance EmployeeDentalInsurance { get; set; }

    }
}
