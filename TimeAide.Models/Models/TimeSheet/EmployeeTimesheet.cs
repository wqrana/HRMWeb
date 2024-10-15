using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeAide.Web.Models
{
     
    public class EmployeeTimesheet : BaseCompanyObjects
    {
        public int? UserInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? OldCompanyId { get; set; }
        public int? nUserID { get; set; }
        public string sUserName { get; set; }
        public string sCompanyName { get; set; }
        public string sDeptName { get; set; }
        public string sEmployeeTypeName { get; set; }
        public string sWeekID { get; set; }
        public string sHoursSummary { get; set; }
        public DateTime? dStartDate { get; set; }
        public DateTime? dEndDate { get; set; }       
        public int? nPayWeekNum { get; set; }
        public string sPayRuleName { get; set; }
        public DateTime? dPunchDate { get; set; }
        public double? dblPunchHours { get; set; }
        public string sPunchSummary { get; set; }
        public string sExpections { get; set; }
        public string sDaySchedule { get; set; }
        public string sPunchHoursSummary { get; set; }
    }
    public class EmployeeTimesheetEditor : BaseCompanyObjects
    {
    

    }
}
