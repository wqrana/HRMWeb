 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAccrualBalance")]
    public partial class EmployeeAccrualBalance : BaseUserObjects
    {
        [Column("EmployeeAccrualBalanceId")]
        public override int Id { get; set; }

        public int AccrualTypeId { get; set; }

    
        public decimal? AccruedHours { get; set; }

        public DateTime BalanceStartDate { get; set; }

        public virtual AccrualType AccrualType { get; set; }


            }
}
