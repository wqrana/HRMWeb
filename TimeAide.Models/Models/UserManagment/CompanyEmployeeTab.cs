namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("CompanyEmployeeTab")]
    public partial class CompanyEmployeeTab : BaseEntity
    {
        [Key]
        [Column("CompanyEmployeeTabId")]
        public override int Id { get; set; }

        public int FormId { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual Form Form { get; set; }

        public virtual Client Client { get; set; }

    }
}
