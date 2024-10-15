namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeDocument")]
    public partial class EmployeeDocument : BaseUserObjects
    {
        public EmployeeDocument()
        {
            NotificationLog = new HashSet<NotificationLog>();
            SelfServiceEmployeeDocument = new HashSet<SelfServiceEmployeeDocument>();
        }
        [Display(Name = "Employee Document Id")]
        [Column("EmployeeDocumentId")]
        public override int Id { get; set; }

        [Display(Name = "Document")]
        public int DocumentId { get; set; }

        [Display(Name = "Document Name")]
        [StringLength(250)]
        public string DocumentName { get; set; }

        [Display(Name = "Document Path")]
        [StringLength(250)]
        public string DocumentPath { get; set; }

        [Display(Name = "Document Note")]
        [StringLength(250)]
        public string DocumentNote { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        
        [Display(Name = "Submission Date")]
        public DateTime? SubmissionDate { get; set; }

        public virtual Document Document { get; set; }

        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<NotificationLog> NotificationLog { get; set; }
        public virtual ICollection<SelfServiceEmployeeDocument> SelfServiceEmployeeDocument { get; set; }
        [NotMapped]
        public string SefServiceRemarks { get; set; }
        [NotMapped]
        public bool IsExpired
        {
            get 
            {
                return NotificationLog.Count > 0 || (ExpirationDate.HasValue && ExpirationDate.Value<DateTime.Now);
            }
        }
    }
}
