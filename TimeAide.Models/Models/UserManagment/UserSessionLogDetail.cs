namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("UserSessionLogDetail")]
    public partial class UserSessionLogDetail : BaseCompanyObjects
    {
        
        public UserSessionLogDetail()
        {
            AuditLog = new HashSet<AuditLog>();
        }
        [Column("UserSessionLogDetailId")]
        public override int Id { get; set; }
        [Required]
        public int UserSessionLogId { get; set; }
        public virtual UserSessionLog UserSessionLog { get; set; }
        [StringLength(150)]
        [Display(Name = "IPAddress")]
        public string ControllerName { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }
        public string ActionName { get; set; }
        public virtual ICollection<AuditLog> AuditLog { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
    }
}
