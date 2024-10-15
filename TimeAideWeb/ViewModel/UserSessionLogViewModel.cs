using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Web;
using TimeAide.Web.Models;

namespace TimeAide.Web.ViewModel
{
    public class UserSessionLogViewModel
    {
        public string FullName { get; set; }
        public int EmployeeId { get; set; }
        public List<UserSessionLog> UserSessionLog { get; set; }
    }
    public class AuditLogViewModel
    {
        public string FullName { get; set; }
        public int EmployeeId { get; set; }
        public List<AuditLog> AuditLog { get; set; }
    }
    public class AuditLogFilterViewModel
    {
        public DateTime? FromDate { get; set;}
        public DateTime? ToDate { get; set;}
        public int? EmployeeId { get; set; }
        public int SupervisorId { get; set; }
        public string EmployeeName { get; set; }
        public string ActionType { get; set; }
        public string RecordType { get; set; }
        

    }

}