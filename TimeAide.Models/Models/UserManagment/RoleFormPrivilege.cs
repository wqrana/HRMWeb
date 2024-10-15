namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("RoleFormPrivilege")]
    public partial class RoleFormPrivilege: BaseEntity
    {
        [Key]
        [Column("RolePrivilegeId")]
        public override int Id { get; set; }

        public int? RoleId { get; set; }

        public int? PrivilegeId { get; set; }

        public int? FormId { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Form Form { get; set; }

        public virtual Client Client { get; set; }

        public virtual Privilege Privilege { get; set; }

        public virtual Role Role { get; set; }

        
    }
}
