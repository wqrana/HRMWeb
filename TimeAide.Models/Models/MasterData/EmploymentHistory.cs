namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using TimeAide.Common.Helpers;

    [Table("EmploymentHistory")]
    public partial class EmploymentHistory : BaseUserObjects
    {
        public EmploymentHistory()
        {
            StartDate = DateTime.Now;
            EmploymentHistoryAuthorizer = new HashSet<EmploymentHistoryAuthorizer>();
            CompanyId = SessionHelper.SelectedCompanyId;
        }

        [Display(Name = "Employment History Id")]
        [Column("EmploymentHistoryId")]
        public override int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        [Display(Name = "Position")]
        public int? PositionId { get; set; }

        [Display(Name = "Employee Type")]
        public int? EmployeeTypeId { get; set; }

        [Display(Name = "Employment Type")]
        public int? EmploymentTypeId { get; set; }

        [StringLength(200)]
        [Display(Name = "Change Reason")]
        public string ChangeReason { get; set; }

        public int? LocationId { get; set; }
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "SubDepartment")]
        public int? SubDepartmentId { get; set; }
        [Required]
        public int? CompanyId { get; set; }

        public int? SupervisorId { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Closed By")]
        public int? ClosedBy { get; set; }

        //public virtual UserInformation AuthorizeBy { get; set; }

        //[Column(TypeName = "image")]
        //public byte[] Document { get; set; }

        public int EmploymentId { get; set; }
        public virtual Employment Employment { get; set; }
        public virtual Company Company { get; set; }
        public virtual Department Department { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual Position Position { get; set; }
        public virtual SubDepartment SubDepartment { get; set; }
        //public virtual UserInformation Supervisor { get; set; }
        [NotMapped]
        public string SelectedSupervisorId { get; set; }
        [NotMapped]
        public string SelectedSupervisorByName
        {
            get
            {
                if (UserInformation != null && UserInformation.EmployeeUserSupervisor.Count > 0)
                    return String.Join(", ", UserInformation.EmployeeUserSupervisor.Where(w => w.DataEntryStatus == 1).Select(c => c.SupervisorUser.ShortFullName.ToString()).Distinct());
                return "";
            }

        }
        [NotMapped]
        public string ClosedByName { get; set; }
        public virtual ICollection<EmploymentHistoryAuthorizer> EmploymentHistoryAuthorizer { get; set; }
        [NotMapped]
        public string SelectedAuthorizById { get; set; }

        [NotMapped]
        public string SelectedAuthorizByName
        {
            get
            {
                if (EmploymentHistoryAuthorizer != null && EmploymentHistoryAuthorizer.Count > 0)
                    return String.Join(",", EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.AuthorizeBy.ShortFullName.ToString()));
                return "";
            }

        }
        [NotMapped]
        public string SelectedAuthorizByIds
        {
            get
            {
                if (EmploymentHistoryAuthorizer != null && EmploymentHistoryAuthorizer.Count > 0)
                    return String.Join(",", EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1).Select(c => c.AuthorizeBy.Id.ToString()));
                return "";
            }

        }
        [NotMapped]
        public string Description
        {
            get
            {
                StringBuilder description = new StringBuilder();
                if (Company != null)
                {
                    description.Append(Company.CompanyName + " - ");
                }
                if (Department != null)
                {
                    description.Append(Department.DepartmentName + " - ");
                }
                if (Position != null)
                {
                    description.Append(Position.PositionName + " - ");
                }
                return description.ToString();

            }
        }

        public virtual Location Location { get; set; }

        [NotMapped]
        public bool CanAddNew
        {
            get
            {
                if (Id == 0)
                    return true;
                if (UserInformation == null)
                    return false;
                var model = UserInformation.EmploymentHistory.Where(W => W.DataEntryStatus == 1).OrderByDescending(e => e.StartDate).FirstOrDefault();
                // if (model == null || model.Id == 0 || model.EmploymentHistoryAuthorizer.Count() > 0)
                if (model == null || model.Id == 0 || model.EndDate!= null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [NotMapped]
        public bool CanEdit
        {
            get
            {
                //if (Id!=0 && !(this.EmploymentHistoryAuthorizer.Count() > 0))
                //if(Id != 0 && EndDate==null)
                if (Id != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [NotMapped]
        public bool CanEditClosing
        {
            get
            {
                //if (Id!=0 && !(this.EmploymentHistoryAuthorizer.Count() > 0))
                if (Id != 0 )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    
}
