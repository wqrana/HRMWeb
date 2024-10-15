namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantDocument")]
    public partial class ApplicantDocument : BaseApplicantObjects
    {
        public ApplicantDocument()
        {
            //ExpirationDate = DateTime.Now;
        }
        [Display(Name = "Applicant Document Id")]
        [Column("ApplicantDocumentId")]
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

        public virtual Document Document { get; set; }

        public virtual ApplicantInformation ApplicantInformation { get; set; }
    }
}
