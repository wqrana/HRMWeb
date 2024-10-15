using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;


namespace TimeAide.Models.ViewModel
{
    public class UserInformationViewModel
    {
        public UserInformationViewModel()
        {
            ListOfEmails = new Dictionary<string,string>();
        }
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string ShortFullName { get; set; }
        public string SSNEnd { get; set; }
        public int? GenderId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? SubDepartmentId { get; set; }
        public string SubDepartmentName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? EmployeeTypeID { get; set; }
        public string EmployeeTypeName { get; set; }
        public string PictureFilePath { get; set; }
        public string ResumeFilePath { get; set; }
        public int? EmployeeStatusId { get; set; }
        public DateTime? EmployeeStatusDate { get; set; }
        public string EmployeeStatusName { get; set; }
        public int CompanyId { get; set; }
        public int? OldCompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ClientId { get; set; }
        public string LoginEmail { get; set; }
        public string NotificationEmail { get; set; }
        public int? EmploymentHistoryId { get; set; }
        public int? PayInformationHistoryId { get; set; }
        public int? EmploymentId { get; set; }
        public Dictionary<string,string> ListOfEmails { get; set; }
        public string EmployeeGroups { get; set; }
        public string UserRole { get; set; }
        public string EncryptedId
        {
            get
            {
                var strId = Id.ToString();
                return Common.Helpers.Encryption.EncryptURLParm(strId);

            }
        }
        public string EmpStatusBgClass { get; set; }
        public string LastName { get; set; }

    }
    public class AuditLogListViewModel
    {
        public int AuditLogId { get; set; }
        public int ReferenceId { get; set; }
        public int? AuditLogDetailId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? RefUserId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string RecordType { get; set; }
        public string ActionType { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remarks { get; set; }
        public int SupervisorId { get; set; }
        public int SupervisorEmployeeId { get; set; }
        public string SupervisorName { get; set; }
    }
}
