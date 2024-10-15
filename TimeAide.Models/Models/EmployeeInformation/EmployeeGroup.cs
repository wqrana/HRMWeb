namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeGroup")]
    public partial class EmployeeGroup : BaseCompanyObjects
    {
        
        public EmployeeGroup()
        {
            UserEmployeeGroup = new HashSet<UserEmployeeGroup>();
            WorkflowLevelGroup = new HashSet<WorkflowLevelGroup>();


        }

        [Display(Name = "Employee Group Id")]
        [Column("EmployeeGroupId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Employee Group")]
        public string EmployeeGroupName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmployeeGroupDescription { get; set; }
        public int EmployeeGroupTypeId { get; set; }
        public virtual EmployeeGroupType EmployeeGroupType { get; set; }

        public virtual ICollection<UserEmployeeGroup> UserEmployeeGroup { get; set; }
        public virtual ICollection<WorkflowLevelGroup> WorkflowLevelGroup { get; set; }
        public virtual ICollection<NotificationScheduleEmployeeGroup> NotificationScheduleEmployeeGroup { get; set; }


        public override List<int?> GetRefferredCompanies()
        {
            var list = this.UserEmployeeGroup.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            list = list.Union(this.WorkflowLevelGroup.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct());
            return list.ToList();
        }
    }
}
