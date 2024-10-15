namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InterfaceControl")]
    public partial class InterfaceControl: BaseEntity
    {
        public InterfaceControl()
        {
            InterfaceControlForms = new HashSet<InterfaceControlForm>();
            UserMenu = new HashSet<UserMenu>();
        }
        [Column("InterfaceControlId")]
        public override int Id { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<InterfaceControlForm> InterfaceControlForms { get; set; }
        public virtual ICollection<UserMenu> UserMenu { get; set; }
    }
}
