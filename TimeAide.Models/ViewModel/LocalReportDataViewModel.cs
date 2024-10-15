using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
    public class EmployeeAttendanceSchViewModel
    {
        public int? SchId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string sWeekID { get; set; }
        public string sNote { get; set; }
        public DateTime? dStartDate { get; set; }
        public DateTime? dEndDate { get; set; }
        public double? dblPeriodHours { get; set; }
        public int? nPayPeriodType { get; set; }
        public int? nPayWeekNum { get; set; }

        public DateTime? dPunchDate { get; set; }
        public DateTime? dPunchIn1 { get; set; }
        public DateTime? dPunchOut1 { get; set; }
        public DateTime? dPunchIn2 { get; set; }
        public DateTime? dPunchOut2 { get; set; }
        public int? nWorkDayType { get; set; }
        public string sWorkDayTypeName { get; set; }
        public double? dblDayHours { get; set; }
        public int? nPunchNum { get; set; }
        public string sNoteDetail { get; set; }

    }
}
