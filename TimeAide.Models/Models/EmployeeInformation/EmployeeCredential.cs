namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeCredential")]
    public partial class EmployeeCredential : BaseUserObjects
    {
        public EmployeeCredential()
        {
            NotificationLog = new HashSet<NotificationLog>();
            SelfServiceEmployeeCredential = new HashSet<SelfServiceEmployeeCredential>();
        }
        [Display(Name = "EmployeeCredentialId")]
        [Column("EmployeeCredentialId")]
        public override int Id { get; set; }

        [Display(Name = "Credential")]
        public String EmployeeCredentialName { get; set; }

        [StringLength(200)]
        [Display(Name = "Credential Description")]
        public string EmployeeCredentialDescription { get; set; }

        [Required]
        [Display(Name = "Issue Date")]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Note")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Title")]
        public int? CredentialTypeId { get; set; }

        [Display(Name = "Document Name")]
        [StringLength(500)]
        public string DocumentName { get; set; }

        [Display(Name = "Document Path")]
        [StringLength(500)]
        public string DocumentPath { get; set; }

        [Display(Name = "Document")]
        [Column(TypeName = "image")]
        public byte[] DocumentFile { get; set; }

        [Display(Name = "Credential")]
        public int CredentialId { get; set; }

        [Display(Name = "Expiration Date Required")]
        public int? ExpirationDateRequired { get; set; }


        public virtual CredentialType CredentialType { get; set; }

        public virtual Credential Credential { get; set; }

        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<NotificationLog> NotificationLog { get; set; }
        public virtual ICollection<SelfServiceEmployeeCredential> SelfServiceEmployeeCredential { get; set; }
        public bool IsRequired
        {
            get;
            set;
            
        }
        [NotMapped]
        public string SefServiceRemarks { get; set; }
        [NotMapped]
        public bool IsExpired
        {
            get
            {
                return NotificationLog.Count > 0 || (ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now);
            }
        }
    }
}
