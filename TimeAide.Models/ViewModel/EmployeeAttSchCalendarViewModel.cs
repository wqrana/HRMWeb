using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Web.Models;
namespace TimeAide.Models.ViewModel
{
   public class EmployeeAttSchCalendarViewModel
    {
        public DateTime CalendarStartDate { get; set; }
        public DateTime CalendarEndDate { get; set; }
        public IList<DateTime> CalendarDateRange { get; set; }
        public IList<EmployeeAttSchCalendarDetailViewModel> CalendarDetail { get; set; }
    }

    public class EmployeeAttSchCalendarDetailViewModel
    {
        public EmployeeAttendenceSchedule EmployeeSchedule { get; set; }
        public IList<EmployeeAttendenceSchDetail> EmployeeScheduleDetail { get; set; }
    }
}
