namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeType")]
    public partial class EmployeeType : BaseCompanyObjects
    {
        
        public EmployeeType()
        {
            UserInformations = new HashSet<UserInformation>();
            EmploymentHistory = new HashSet<EmploymentHistory>();
            SupervisorEmployeeType = new HashSet<SupervisorEmployeeType>();
        }

        [Display(Name = "Employee Type Id")]
        [Column("EmployeeTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Employee Type")]
        public string EmployeeTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmployeeTypeDescription { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }
        public virtual ICollection<SupervisorEmployeeType> SupervisorEmployeeType { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.UserInformations.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            list = list.Union(this.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct());
            list = list.Union(this.SupervisorEmployeeType.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct());
            return list.ToList();
        }
    }
}
