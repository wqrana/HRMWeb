using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Data
{
    public partial class viewUserCompensationDaily
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int nUserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dEffectiveDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string sAccrualType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? dBalanceStartDate { get; set; }

        public decimal? dblAccruedHours { get; set; }

        public decimal? dblAccrualDailyHours { get; set; }

        public decimal? dblStartBalance { get; set; }

        public decimal? dblAccruedBalance { get; set; }

        public decimal? dblTakenBalance { get; set; }
        public decimal? dblAvailableBalance { get; set; }

        [StringLength(50)]
        public string sAccrualDays { get; set; }
      
    }
}
