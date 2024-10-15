namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("DocumentRequiredBy")]
    public partial class DocumentRequiredBy : BaseGlobalEntity
    {

        public DocumentRequiredBy()
        {
            Document = new HashSet<Document>();
            CustomField = new HashSet<CustomField>();

        }

        [Display(Name = "Applies To")]
        [Column("DocumentRequiredById")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Employee Group")]
        public string DocumentRequiredByName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string DocumentRequiredByDescription { get; set; }


        public virtual ICollection<Document> Document { get; set; }
        public virtual ICollection<CustomField> CustomField { get; set; }
    }

    public enum DocumentRequiredBys
    {

        Employee = 1,
        Applicant = 2,
        Both = 3,
    }
}
