namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("RoleType")]
    public partial class RoleType: BaseGlobalEntity
    {
        public RoleType()
        {
            Role = new HashSet<Role>();
            RoleTypeFormPrivilege = new HashSet<RoleTypeFormPrivilege>();
        }

        [Column("RoleTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleTypeName { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<RoleTypeFormPrivilege> RoleTypeFormPrivilege { get; set; }

        

    }
}
