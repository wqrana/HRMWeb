namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeGroupType")]
    public partial class EmployeeGroupType : BaseGlobalEntity
    {

        public EmployeeGroupType()
        {
            EmployeeGroup = new HashSet<EmployeeGroup>();
        }

        [Display(Name = "Employee Group Id")]
        [Column("EmployeeGroupTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Employee Group")]
        public string EmployeeGroupTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmployeeGroupTypeDescription { get; set; }


        public virtual ICollection<EmployeeGroup> EmployeeGroup { get; set; }
    }

    public enum EmployeeGroupTypes
    {

        Employee = 1,
        Supervisor = 2,
        HumanResource = 3,
        Management = 4,
        CompanyPortalAdmin = 5
    }
}
