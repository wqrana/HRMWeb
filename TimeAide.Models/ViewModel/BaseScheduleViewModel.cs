using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.ViewModel
{
    public class BaseScheduleViewModel
    {
        public int Id { get; set; }
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
        public bool IsAllCompanies { get; set; }
        public IEnumerable<BaseScheduleDayInfo> BaseScheduleDaysInfo { get; set; }
    }
}
