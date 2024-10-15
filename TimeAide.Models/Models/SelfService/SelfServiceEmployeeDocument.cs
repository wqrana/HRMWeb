namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SelfServiceEmployeeDocument")]
    public partial class SelfServiceEmployeeDocument : ChangeRequestBase
    {
        public SelfServiceEmployeeDocument()
        {
        }
        [Display(Name = "Employee Document Id")]
        [Column("SelfServiceEmployeeDocumentId")]
        public override int Id { get; set; }

        [Display(Name = "Employee Document Id")]
        public int? EmployeeDocumentId { get; set; }

        [Display(Name = "Document")]
        public int DocumentId { get; set; }

        [Display(Name = "Document Name")]
        [Required]
        [StringLength(250)]
        public string DocumentName { get; set; }

        [StringLength(250)]
        public string OriginalDocumentName { get; set; }
        

        [Display(Name = "Document Path")]
        [StringLength(250)]
        public string DocumentPath { get; set; }

        [Display(Name = "Document Note")]
        [StringLength(250)]
       
        public string DocumentNote { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        //public int? NewEmployeeDocumentId { get; set; }

        public virtual Document Document { get; set; }

        public virtual EmployeeDocument EmployeeDocument { get; set; }

    }
}
