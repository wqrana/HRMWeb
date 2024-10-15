namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("AuditLogDetail")]
    public partial class AuditLogDetail : BaseCompanyObjects
    {
        
        public AuditLogDetail()
        {
        }
        [Column("AuditLogDetailId")]
        public override int Id { get; set; }
        [Required]
        public int AuditLogId { get; set; }
        public virtual AuditLog AuditLog { get; set; }
        [StringLength(150)]
        [Display(Name = "ColumnName")]
        public string ColumnName { get; set; }

        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
    }
}
