 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeTimeOffRequest")]
    public partial class EmployeeTimeOffRequest : ChangeRequestBase
    {
        [Column("EmployeeTimeOffRequestId")]
        public override int Id { get; set; }
        [NotMapped]
        public int? EmployeeId { get; set; }
        public string AccrualType { get; set; }
        public string TransType { get; set; }
        public bool? IsSingleDay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DayHours { get; set; }
        public decimal? WorkingDays { get; set; }
        [NotMapped]
        public decimal? Balance { get; set; }
        [NotMapped]
        public decimal? RequestedTime { get {
                return (DayHours ?? 0) * (WorkingDays ?? 0);
            }          
        }
        [NotMapped]
        public decimal? RemainingTime {
            get;set;
        }
        public string RequestNote { get; set; }
        [NotMapped]
        public bool CanTakeAction { get; set; }

        [NotMapped]
        public bool CanRequiredDocument { get; set; }
    }
}
