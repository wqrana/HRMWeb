namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("Department")]
    public partial class Department : BaseCompanyObjects
    {
        
        public Department()
        {
            SubDepartments = new HashSet<SubDepartment>();
            UserInformations = new HashSet<UserInformation>();
            SupervisorDepartment = new HashSet<SupervisorDepartment>();
            EmploymentHistory = new HashSet<EmploymentHistory>();
        }

        [Display(Name = "Department ID")]
        [Column("DepartmentId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string DepartmentDescription { get; set; }

        //public bool Enabled { get; set; }

        [Display(Name = "CFSE Assignment")] 
        public bool USECFSEAssignment { get; set; }

        [Display(Name = "CFSE Code")]
        [RequireCondition("USECFSEAssignment")]
        public int? CFSECodeId { get; set; }

        [Display(Name = "CFSE Percent")]
        [Range(typeof(decimal), "0", "100")]
        [RequireCondition("USECFSEAssignment")]
        public decimal? CFSECompanyPercent { get; set; }

        [Display(Name = "CFSE Code")]
        public int? JobCertificationSigneeId { get; set; }
        public int? JobCertificationTemplateId { get; set; }
        public virtual JobCertificationSignee JobCertificationSignee { get; set; }
        public virtual JobCertificationTemplate JobCertificationTemplate { get; set; }
        public virtual CFSECode CFSECode { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<SubDepartment> SubDepartments { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<SupervisorDepartment> SupervisorDepartment { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            var list = this.SubDepartments.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            list = list.Union(this.UserInformations.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct());
            list = list.Union(this.SupervisorDepartment.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct());
            list = list.Union(this.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct());
            return list.ToList();
        }
    }
}
