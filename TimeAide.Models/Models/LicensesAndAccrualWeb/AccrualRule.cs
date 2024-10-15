using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AccrualRule")]
    public class AccrualRule : BaseCompanyObjects
    {
        [Column("AccrualRuleId")]
        public override int Id { get; set; }
        public string AccrualRuleName { get; set; }
        public string AccrualRuleDescription { get; set; }
        public int? ReferenceTypeId { get; set; }
        public DateTime? BeginningOfYearDate { get; set; }
        public int? AccrualTypeId { get; set; }
        public string AccrualTypeUnavailable { get; set; }
        public int? AccrualPeriodTypeId { get; set; }
        public int? AccumulationTypeId { get; set; }
        public double? AccumulationMultiplier { get; set; }
        public bool UseYearsWorked { get; set; }
        [NotMapped]
        public new bool IsAllCompanies { get; set; }
        public virtual AccrualType AccrualType { get; set; }

    }
}


