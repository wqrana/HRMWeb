using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
   public class UserFilterViewModel
    {
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string SearchText { get; set; }       
        public int? DepartmentId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? EmploymentTypeId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? SupervisorId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNo { get; set; }
        public int ViewTypeId { get; set; }
        public int? TAEApprovalTypeId { get; set; }
        public int? TAEEmpApprovalTypeId { get; set; }

    }
}
