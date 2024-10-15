namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("UserMenu")]
    public partial class UserMenu: BaseEntity
    {
        [Key]
        [Column("LinkId")]
        public override int Id { get; set; }

        public int? ParentLinkId { get; set; }

        public int? FormId { get; set; }

        public int? UserInterfaceId { get; set; }

        public int? Weight { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Alt { get; set; }

        [StringLength(100)]
        public string Anchor { get; set; }

        [StringLength(100)]
        public string AnchorClass { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual Form Form { get; set; }

        public int? InterfaceControlId { get; set; }

        public virtual InterfaceControl InterfaceControl { get; set; }

        public virtual Client Client { get; set; }

    }
}
