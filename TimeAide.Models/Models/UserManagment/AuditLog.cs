namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("AuditLog")]
    public partial class AuditLog : BaseCompanyObjects
    {

        public AuditLog()
        {
            AuditLogDetail = new HashSet<AuditLogDetail>();
        }
        [Column("AuditLogId")]
        public override int Id { get; set; }
        public int ReferenceId { get; set; }
        public long? ReferenceId1 { get; set; }
        public int? UserInformationId { get; set; }
        public int? RefUserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual UserInformation RefUserInformation { get; set; }
        public int? UserSessionLogDetailId { get; set; }
        public virtual UserSessionLogDetail UserSessionLogDetail { get; set; }
        [StringLength(150)]
        [Display(Name = "TableName")]
        public string TableName { get; set; }
        public string Remarks { get; set; }
        [StringLength(150)]
        [Display(Name = "ActionType")]
        public string ActionType { get; set; }
        public virtual ICollection<AuditLogDetail> AuditLogDetail { get; set; }
        public object ReferenceObject { get; set; }
        [NotMapped]
        public virtual UserInformation CreatedByUser { get; set; }
        [NotMapped]
        public int RefEmployeeId
        {
            get
            {
                if (RefUserInformation != null)
                {
                    return RefUserInformation.Id;
                }
                return 0;
            }
        }
        public string RefEmployeeName
        {
            get
            {
                if (RefUserInformation != null)
                {
                    return RefUserInformation.ShortFullName;
                }
                return "";
            }
        }
        public override List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
    }
}
