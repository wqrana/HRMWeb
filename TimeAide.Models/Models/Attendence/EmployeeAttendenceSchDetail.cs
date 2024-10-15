using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
   public class EmployeeAttendenceSchDetail: BaseCompanyObjects
    {
        public int? UserInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? OldCompanyId { get; set; }       
        public int? nUserID { get; set; }
        public string sUserName { get; set; }
        public string sWeekID { get; set; }
        public string sNote { get; set; }
        public DateTime? dPunchDate { get; set; }
        public DateTime? dPunchIn1 { get; set; }
        public DateTime? dPunchOut1 { get; set; }
        public DateTime? dPunchIn2 { get; set; }
        public DateTime? dPunchOut2 { get; set; }
        public DateTime? dPunchIn3 { get; set; }
        public DateTime? dPunchOut3 { get; set; }
        public DateTime? dPunchIn4 { get; set; }
        public DateTime? dPunchOut4 { get; set; }
        public int? nWorkDayType { get; set; }
        public string sWorkDayTypeName { get; set; }
        public double? dblDayHours { get; set; }
        public double? dblTotalPeriodHours { get; set; }
        public int? nPunchNum { get; set; }
        public int? nPayWeekNum { get; set; }
        public int? nJobCodeID { get; set; }
        public int? nSchedModPeriodSummId { get; set; }
        public int? nScheduleId { get; set; }
    }
}
