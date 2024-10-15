namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SelfServiceEmployeeCredential")]
    public partial class SelfServiceEmployeeCredential : ChangeRequestBase
    {
        public SelfServiceEmployeeCredential()
        {
        }
        [Display(Name = "Employee Document Id")]
        [Column("SelfServiceEmployeeCredentialId")]
        public override int Id { get; set; }

        [Display(Name = "EmployeeCredentialId")]
        [Column("EmployeeCredentialId")]
        public int? EmployeeCredentialId { get; set; }

        [Display(Name = "Credential")]
        public String EmployeeCredentialName { get; set; }

        [StringLength(200)]
        [Display(Name = "Credential Description")]
        public string EmployeeCredentialDescription { get; set; }

        //[Required]
        [Display(Name = "Issue Date")]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Note")]
        public string Note { get; set; }
               
        [Display(Name = "Credential Type")]
        [Required]
        public int? CredentialTypeId { get; set; }

        [Display(Name = "Document Name")]
        [StringLength(500)]
        public string DocumentName { get; set; }

        [StringLength(250)]
        public string OriginalDocumentName { get; set; }

        [Display(Name = "Document Path")]
        [StringLength(500)]
        public string DocumentPath { get; set; }

        [Display(Name = "Document")]
        [Column(TypeName = "image")]
        public byte[] DocumentFile { get; set; }
       
        [Display(Name = "Credential")]
        [Required]
        public int CredentialId { get; set; }

        [Display(Name = "Expiration Date Required")]
        public int? ExpirationDateRequired { get; set; }
        public virtual CredentialType CredentialType { get; set; }
        public virtual Credential Credential { get; set; }
        public virtual EmployeeCredential EmployeeCredential { get; set; }
        //public virtual ICollection<NotificationLog> NotificationLog { get; set; }
        //public int? NewEmployeeCredentialId { get; set; }
        public bool IsRequired
        {
            get;
            set;

        }

    }
}
