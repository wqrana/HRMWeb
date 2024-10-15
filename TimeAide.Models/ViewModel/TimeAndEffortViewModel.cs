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
    public class EmployeeTimeAndEffortViewModel
    {
       public int EmployeeId { get; set; }
       public string TAEId { get; set; }
       public string EmployeeName { get; set; }
       public int EffortMonth { get; set; }
       public int EffortYear { get; set; }
       public bool IsSupervisorApproved { get;set; }
       public bool IsEmployeeApproved { get; set; }
       public IList<string> EffortTypeRange { get; set; }
       public IList<EmployeeTimeAndEffortDetailViewModel> TimeAndEffortDetail { get; set; }
    }

    public class EmployeeTimeAndEffortDetailViewModel
    {
       public DateTime EffortDate { get; set; }
       public IList<EffortInfoViewModel> EffortInfo { get; set; }
    }

    public class EffortInfoViewModel
    {
        public string EffortTypeName { get; set; }  
        public decimal EffortHrs { get; set; }
    }
    public class TimeAndEffortApprovalViewModel
    {
        public string TAEIds { get; set; }
        public int EmployeeId { get; set; }       
        public string EmployeeName { get; set; }
        public string ActionType { get; set; } //I-Invidual, A-All
        //public string ApprovalType { get;set; } //A-approved, U-unapproved
        public int ApprovalTypeId { get; set; } //1- approved, 0- unapproved                                      //
    }
}
