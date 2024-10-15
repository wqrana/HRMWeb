 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAccrualRule")]
    public partial class EmployeeAccrualRule : BaseUserObjects
    {
        [Column("EmployeeAccrualRuleId")]
        public override int Id { get; set; }
        
        public int AccrualTypeId { get; set; }

        public int? AccrualRuleId { get; set; }

        public decimal? AccrualDailyHours { get; set; }

        public DateTime StartOfRuleDate { get; set; }

        public virtual AccrualType AccrualType { get; set; }

        public virtual AccrualRule AccrualRule { get; set; }
    }
}
