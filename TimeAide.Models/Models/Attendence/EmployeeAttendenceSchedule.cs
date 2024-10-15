using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    public class WorkDayType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }
    }
    public class PunchNumType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class AttendenceScheduleMasterDDV
    {
        private static List<WorkDayType> _workDayTypes = new List<WorkDayType>() { new WorkDayType {Id=1,Name="Work Day",ShortName="WorkDay"},
                                                                  new WorkDayType {Id=0,Name="UnScheduled",ShortName="UnSched"} , new WorkDayType {Id=2,Name="UnSched (License)",ShortName="UnSched(Lnse)"}};
        private static List<PunchNumType> _punchNumTypes = new List<PunchNumType>() {
                                                                    new PunchNumType {Id=0,Name="0"},
                                                                    new PunchNumType {Id=1,Name="1"},
                                                                    new PunchNumType {Id=2,Name="2"},
                                                                    new PunchNumType {Id=4,Name="4"}};

        public static List<WorkDayType> WorkDayTypes { get
            {
                return _workDayTypes;
            } }

        public static List<PunchNumType> PunchNumTypes
        {
            get
            {
                return _punchNumTypes;
            }
        }

       
    }
    public  class EmployeeAttendenceSchedule : BaseCompanyObjects
    {
        public int? UserInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? OldCompanyId { get; set; }       
        public int? nUserID { get; set; }
        public string sUserName { get; set; }
        public string sWeekID { get; set; }
        public string sNote { get; set; }
        public DateTime? dStartDate { get; set; }
        public DateTime? dEndDate { get; set; }
        public double? dblPeriodHours { get; set; }            
        public int? nPayPeriodType { get; set; }
        public int? nPayWeekNum { get; set; }
        public int? nScheduleId { get; set; }

    }
}
