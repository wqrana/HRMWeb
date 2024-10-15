
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AccrualRuleWorkedHoursTier")]
    public class AccrualRuleWorkedHoursTier : BaseCompanyObjects
    {
        [Column("AccrualRuleWorkedHoursTierId")]
        public override int Id { get; set; }
        public int AccrualRuleId { get; set; }
        public int AccrualRuleTierId { get; set; }
        public int TierNo { get; set; }
        public string TierDescription { get; set; }
        public double TierWorkedHoursMin { get; set; }
        public double? TierWorkedHoursMax { get; set; }
        public double? AccrualHours { get; set; }
        public virtual AccrualRuleTier AccrualRuleTier { get; set; }

    }
}


