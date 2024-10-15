using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.ViewModel
{
    public class WebSchedule
    {

    }
   public class WebScheduleViewModel
    {
        public int UserInformationId { get; set; }
        public bool? IsRotatingSchedule { get; set; }
        public IEnumerable<EmployeeRotatingSchedule> EmployeeRotatingSchedule { get; set; }
        public IEnumerable<EmployeeFutureSchedule> EmployeeFutureSchedule { get; set; }
    }
}
