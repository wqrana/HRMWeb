namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("RoleInterfaceControlPrivilege")]
    public partial class RoleInterfaceControlPrivilege: BaseEntity
    {
        [Key]
        [Column("RoleInterfacePrivilegeId")]
        public override int Id { get; set; }

        public int? RoleId { get; set; }

        public int? PrivilegeId { get; set; }

        public int? InterfaceControlFormId { get; set; }

        

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual InterfaceControlForm InterfaceControlForm { get; set; }

        public virtual Client Client { get; set; }

        public virtual Privilege Privilege { get; set; }

        public virtual Role Role { get; set; }

        //public virtual UserInformation UserInformation { get; set; }

        //public virtual UserInformation UserInformation1 { get; set; }
    }
}
