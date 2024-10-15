namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public  class WebScheduledEditor : BaseEntity
    {

    }

    [Table("EmployeeWebScheduledPeriod")]
    public partial class EmployeeWebScheduledPeriod : BaseUserObjects
    {
        [Column("EmployeeWebScheduledPeriodId")]
        public override int Id { get; set; }
        public int? EmployeeWebTimeSheetId { get; set; }
        public string Note { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public double? PeriodHours { get; set; }
        public int? PayFrequencyId { get; set; }
        public int? PayWeekNumber { get; set; }
       
        public virtual PayFrequency PayFrequency { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual EmployeeWebTimeSheet EmployeeWebTimeSheet { get; set; }
    }

    [Table("EmployeeWebScheduledPeriodDetail")]
    public partial class EmployeeWebScheduledPeriodDetail : BaseUserObjects
    {
        [Column("EmployeeWebScheduledPeriodDetailId")]
        public override int Id { get; set; }
        public int? EmployeeWebScheduledPeriodId { get; set; }
        public string Note { get; set; }
        public int NoOfPunch { get; set; }
        public DateTime PunchDate { get; set; }
        public DateTime? TimeIn1 { get; set; }
        public DateTime? TimeOut1 { get; set; }
        public DateTime? TimeIn2 { get; set; }
        public DateTime? TimeOut2 { get; set; }
        public int? WorkDayTypeId { get; set; }
        public double? DayHours { get; set; }       
        public int? PayWeekNumber { get; set; }

        public virtual EmployeeWebScheduledPeriod EmployeeWebScheduledPeriod { get; set; }
        public virtual UserInformation UserInformation { get; set; }
    }
    
}
