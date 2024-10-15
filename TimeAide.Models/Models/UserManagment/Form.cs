namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Form")]
    public partial class Form : BaseGlobalEntity
    {
        public Form()
        {
            RoleFormPrivileges = new HashSet<RoleFormPrivilege>();
            RoleTypeFormPrivilege = new HashSet<RoleTypeFormPrivilege>();
            InterfaceControlForms = new HashSet<InterfaceControlForm>();
            UserMenus = new HashSet<UserMenu>();
        }
        [Column("FormId")]
        public override int Id { get; set; }

        public int ModuleId { get; set; }

        [StringLength(100)]
        public string FormName { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [StringLength(100)]
        public string LabelPlural { get; set; }
        public int? ParentFormId { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Module Module { get; set; }

        public virtual bool IsSuperUserOnlyForm { get; set; }

        public virtual bool IsTab { get; set; }
        //public virtual UserInformation UserInformation { get; set; }

        public virtual Client Client { get; set; }

        public virtual Client Client1 { get; set; }
        public virtual ICollection<RoleFormPrivilege> RoleFormPrivileges { get; set; }
        public virtual ICollection<RoleTypeFormPrivilege> RoleTypeFormPrivilege { get; set; }
        public virtual ICollection<InterfaceControlForm> InterfaceControlForms { get; set; }
        public virtual ICollection<UserMenu> UserMenus { get; set; }
    }
}
