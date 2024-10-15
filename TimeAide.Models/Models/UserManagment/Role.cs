namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("Role")]
    public partial class Role: BaseEntity
    {
        public Role()
        {
            RoleInterfaceControlPrivilege = new HashSet<RoleInterfaceControlPrivilege>();
            RoleFormPrivilege = new HashSet<RoleFormPrivilege>();
            UserInformationRole = new HashSet<UserInformationRole>();
        }
        [Column("RoleId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public int? RoleTypeId { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual ICollection<RoleInterfaceControlPrivilege> RoleInterfaceControlPrivilege { get; set; }
        public virtual ICollection<RoleFormPrivilege> RoleFormPrivilege { get; set; }
        public virtual ICollection<UserInformationRole> UserInformationRole { get; set; }
    }
}
