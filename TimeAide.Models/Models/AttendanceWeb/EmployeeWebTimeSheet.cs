using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.Models.AttendanceWeb
{
    class EmployeeWebTimeSheet
    {
    }
}

namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    [Table("EmployeeWebTimeSheet")]
    public partial class EmployeeWebTimeSheet : BaseUserObjects
    {
        [Column("EmployeeWebTimeSheetId")]
        public override int Id { get; set; }
        public int? PayWeekNumber { get; set; }       
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? RegularHours { get; set; }
        public double? MealHours { get; set; }
        public double? SingleOTHours { get; set; }
        public double? DoubleOTHours { get; set; }
        public double? OtherHours { get; set; }
        public string HoursSummary { get; set; }
        public int? PayFrequencyId { get; set; }
        public int? ReviewStatusId { get; set; }
        public int? ReviewSupervisorId { get; set;}
        public bool? IsLocked { get; set; }      
        public int? BaseScheduleId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? CompanyId { get; set; }
        public virtual PayFrequency PayFrequency { get; set; }
        public virtual UserInformation UserInformation { get; set; }
    }
}