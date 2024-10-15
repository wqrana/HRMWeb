namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Privilege")]
    public partial class Privilege: BaseEntity
    {
        
        public Privilege()
        {
            InterfaceControlForms = new HashSet<InterfaceControlForm>();
            RoleInterfaceControlPrivileges = new HashSet<RoleInterfaceControlPrivilege>();
            RoleFormPrivileges = new HashSet<RoleFormPrivilege>();
            RoleTypeFormPrivileges = new HashSet<RoleTypeFormPrivilege>();
        }

        [Column("PrivilegeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Privilege Name")]
        public string PrivilegeName { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<InterfaceControlForm> InterfaceControlForms { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<RoleInterfaceControlPrivilege> RoleInterfaceControlPrivileges { get; set; }
        public virtual ICollection<RoleFormPrivilege> RoleFormPrivileges { get; set; }
        public virtual ICollection<RoleTypeFormPrivilege> RoleTypeFormPrivileges { get; set; }
    }
}
