using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AccrualRuleTier")]
    public class AccrualRuleTier : BaseCompanyObjects
    {
        [Column("AccrualRuleTierId")]
        public override int Id { get; set; }
        public int AccrualRuleId { get; set; }
        public int TierNo { get; set; }
        public string TierDescription { get; set; }
        public double? YearsWorkedFrom { get; set; }
        public double? YearsWorkedTo { get; set; }
        public bool WaitingPeriodType { get; set; }
        public int? WaitingPeriodLength { get; set; }
        public int? AllowedMaxHoursTypeId { get; set; }
        public double? AllowedMaxHours { get; set; }
        public string AccrualTypeExcess { get; set; }

        public int? ResetAccruedHoursTypeId { get; set; }
        public double? ResetHours { get; set; }
        public DateTime? ResetDate { get; set; }

        public bool MinWorkedHoursType { get; set; }
        public double? AccrualHours { get; set; }
  
        public virtual AccrualRule AccrualRule { get; set; }

        public virtual ICollection<AccrualRuleWorkedHoursTier> AccrualRuleWorkedHoursTiers { get; set; }
    }
}


