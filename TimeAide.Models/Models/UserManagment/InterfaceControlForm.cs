namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InterfaceControlForm")]
    public partial class InterfaceControlForm: BaseEntity
    {
        
        public InterfaceControlForm()
        {
            RoleInterfaceControlPrivileges = new HashSet<RoleInterfaceControlPrivilege>();
        }
        [Column("InterfaceControlFormId")]
        public override int Id { get; set; }

        public int? InterfaceControlId { get; set; }

        public int? ModuleId { get; set; }

        public int? FormId { get; set; }

        public int? PrivilegeId { get; set; }

       

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Form Form { get; set; }

        public virtual InterfaceControl InterfaceControl { get; set; }

        public virtual Module Module { get; set; }

        public virtual Client Client { get; set; }


        
        public virtual ICollection<RoleInterfaceControlPrivilege> RoleInterfaceControlPrivileges { get; set; }

        public virtual Privilege Privilege { get; set; }
    }
}
