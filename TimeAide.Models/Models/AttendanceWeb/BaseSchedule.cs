using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("BaseSchedule")]
    public class BaseSchedule : BaseCompanyObjects
    {
        [Column("BaseScheduleId")]
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoOfPunch { get; set; }        
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }        
        [NotMapped]
        public double TotalScheduleHours { get
            {
                double totalHrs = 0.0;
                if (BaseScheduleDayInfos.Count == 7)
                {
                   foreach(var day in BaseScheduleDayInfos)
                    {
                        switch (day.DayOfWeek)
                        {
                            case 1:
                                if (IsSunday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 2:
                                if (IsMonday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 3:
                                if (IsTuesday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 4:
                                if (IsWednesday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 5:
                                if (IsThursday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 6:
                                if (IsFriday)
                                    totalHrs += day.ShiftHours;
                                break;
                            case 7:
                                if (IsSaturday)
                                    totalHrs += day.ShiftHours;
                                break;
                        }
                   }
                 }
                return Math.Round(totalHrs,2);
            }
        }
        public virtual ICollection<BaseScheduleDayInfo> BaseScheduleDayInfos { get; set; }
    }

    [Table("BaseScheduleDayInfo")]
    public class BaseScheduleDayInfo : BaseEntity
    {
        [Column("BaseScheduleDayInfoId")]
        public override int Id { get; set; }       
        public int BaseScheduleId { get; set; }      
        public int DayOfWeek { get; set; }     
        public int NoOfPunch { get; set; }
        public bool IsRight { get; set; }
        public DateTime? TimeIn1 { get; set; }
        public DateTime? TimeOut1 { get; set; }
        public DateTime? TimeIn2 { get; set; }
        public DateTime? TimeOut2 { get; set; }

        [NotMapped]
        public double ShiftHours
        {
            get {
                var punch2Min = 0.0;
                var punch4Min = 0.0;
                DateTime defaultPMPeakDatetime = DateTime.Parse(TimeIn1.Value.ToShortDateString()).AddDays(1).AddMinutes(-1);
                DateTime defaultPMLeastDatetime = DateTime.Parse(TimeIn1.Value.ToShortDateString()).AddHours(12);
                DateTime defaultAMLeastDatetime = DateTime.Parse(TimeIn1.Value.ToShortDateString());
                DateTime defaultAMPeakDatetime = DateTime.Parse(TimeIn1.Value.ToShortDateString()).AddHours(12).AddMinutes(-1);
                if (NoOfPunch == 2)
                {
                    punch2Min = GetPunchMinutes(TimeIn1.Value, TimeOut1.Value);
                    ////punch2Min= Math.Abs(TimeOut1.Value.Subtract(TimeIn1.Value).TotalMinutes);
                    //if ((TimeIn1.Value >= defaultPMLeastDatetime && TimeOut1.Value <= defaultPMPeakDatetime)
                    //    && (TimeOut1.Value >= defaultAMLeastDatetime && TimeOut1.Value <= defaultAMPeakDatetime))
                    //{
                    //    var part1 = Math.Abs(TimeIn1.Value.Subtract(defaultPMPeakDatetime).TotalMinutes) + 1;
                    //    var part2 = Math.Abs(TimeOut1.Value.Subtract(defaultAMLeastDatetime).TotalMinutes);
                    //    punch2Min = part1 + part2;
                    //}
                    //else
                    //{
                    //    punch2Min = Math.Abs(TimeIn1.Value.Subtract(TimeOut1.Value).TotalMinutes);
                    //}
                }
                else if (NoOfPunch == 4)
                {

                    //punch2Min = Math.Abs(TimeIn1.Value.Subtract(TimeOut1.Value).TotalMinutes);
                    punch2Min = GetPunchMinutes(TimeIn1.Value, TimeOut1.Value);
                    //punch4Min = Math.Abs(TimeIn2.Value.Subtract(TimeOut2.Value).TotalMinutes);
                    punch4Min = GetPunchMinutes(TimeIn2.Value, TimeOut2.Value);
                }
                var dayHours = Math.Round(((punch2Min + punch4Min) / 60), 2);
                return dayHours;
            }
        }
        private double GetPunchMinutes(DateTime timeIn, DateTime timeOut)
        {
            var punchMin = 0.0;
            DateTime defaultPMPeakDatetime = DateTime.Parse(timeIn.ToShortDateString()).AddDays(1).AddMinutes(-1);
            DateTime defaultPMLeastDatetime = DateTime.Parse(timeIn.ToShortDateString()).AddHours(12);
            DateTime defaultAMLeastDatetime = DateTime.Parse(timeIn.ToShortDateString());
            DateTime defaultAMPeakDatetime = DateTime.Parse(timeIn.ToShortDateString()).AddHours(12).AddMinutes(-1);
            if ((timeIn >= defaultPMLeastDatetime && timeIn <= defaultPMPeakDatetime)
                       && (timeOut >= defaultAMLeastDatetime && timeOut <= defaultAMPeakDatetime))
            {
                var part1 = Math.Abs(timeIn.Subtract(defaultPMPeakDatetime).TotalMinutes) + 1;
                var part2 = Math.Abs(timeOut.Subtract(defaultAMLeastDatetime).TotalMinutes);
                punchMin = part1 + part2;
            }
            else
            {
                punchMin = Math.Abs(timeIn.Subtract(timeOut).TotalMinutes);
            }

            return punchMin;
        }
        public virtual BaseSchedule BaseSchedule { get; set; }

    }

    
}


