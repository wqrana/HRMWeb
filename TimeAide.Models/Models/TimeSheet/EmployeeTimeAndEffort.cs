using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeAide.Web.Models
{
    public enum TimeAndEffortApproval
    {
        Unapproved,
        Approved
    }
    public class EmployeeTimeAndEffort : BaseCompanyObjects
    {   
        public string TAEId { get; set; }
        public int UserId { get; set; }
        public string UserName {get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsSupervisorApproved { get; set;}
        public string strSupervisorEntry { get; set; }
        public DateTime? dtSupervisorDateTime { get; set; }
        public bool IsEmployeeApproved { get;set; }
        public string strEmployeeEntry { get; set; }
        public DateTime? dtEmployeeDateTime { get; set; }
        public DateTime PunchDate { get; set; }
        public string CompensationName { get; set; }       
        public decimal EffortHours { get; set; } 

    }
}
