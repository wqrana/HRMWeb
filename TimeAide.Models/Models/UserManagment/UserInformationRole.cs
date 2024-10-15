namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("UserInformationRole")]
    public partial class UserInformationRole: BaseEntity
    {
        [Key]
        [Column("UserRoleID")]
        public override int Id { get; set; }

        public int RoleId { get; set; }

        public int UserInformationId { get; set; }

        public int? CompanyId { get; set; }

        public virtual Role Role { get; set; }

        public virtual UserInformation UserInformation { get; set; }
    }
}
