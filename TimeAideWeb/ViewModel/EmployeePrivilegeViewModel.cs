using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.ViewModel
{
    public class EmployeePrivilegeViewModel
    {
        public EmployeePrivilegeViewModel()
        {
            SupervisorCompany = new List<Company>();
        }

        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int RoleTypeId { get; set; }

        public int UserInformationId { get; set; }
        public int? EmployeeStatusId { get; set; }
        [StringLength(50)]
        //[Required]
        [EmailAddress(ErrorMessage = "Invalid Login Email Address")]
        public string LoginEmail { get; set; }
        [StringLength(50)]
        
        [EmailAddress(ErrorMessage = "Invalid Notification Email Address")]
        public string NotificationEmail { get; set; }
        public string SelectedClientId { get; set; }
        public List<Company> SupervisorCompany { get; set; }
        public string SelectedEmployeeGroupId { get; set; }
        public string SelectedCompanyId { get; set; }
        public string SelectedDepartmentId { get; set; }
        public string SelectedSubDepartmentId { get; set; }
        public string _SelectedEmployeeTypeId;
        public string SelectedEmployeeTypeId
        {
            get
            {
                return _SelectedEmployeeTypeId;
            }

            set
            {
                _SelectedEmployeeTypeId = value;
            }
        }

        public int EmployeeTypeId { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public MultiSelectList SupervisedUsers { get; set; }
        public List<UserInformation> SupervisedEmployee { get; set; }
        public List<string> SupervisedUserId { get; set; }
        public List<UserInformation> EmployeeSupervisor { get; set; }
        public UserInformationActivation UserInformationActivation { get; set; }

        public bool LockAccount { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ViewType { get; set; }
        public bool? HasAllCompany { get; set; }

    }
}