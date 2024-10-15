 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeePerformance")]
    public partial class EmployeePerformance : BaseUserObjects
    {
        [Column("EmployeePerformanceId")]
        public override int Id { get; set; }
        public int? SupervisorId { get; set; }
        public int? ActionTakenId { get; set; }
        public int? PerformanceDescriptionId { get; set; }
        public int? PerformanceResultId { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ReviewSummary { get; set; }
        public string ReviewNote { get; set; }
        public string DocName { get; set; }
        public string DocFilePath { get; set; }
       
        public virtual ActionTaken ActionTaken { get; set; }
        public virtual PerformanceDescription PerformanceDescription { get; set; }
        public virtual PerformanceResult PerformanceResult { get; set; }
        [ForeignKey("SupervisorId")]
        public virtual UserInformation Supervisor { get; set; }
        public virtual UserInformation UserInformation { get; set; }


    }
}
