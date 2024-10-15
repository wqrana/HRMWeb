namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Document")]
    public partial class Document : BaseEntity
    {
        public Document()
        {
            EmployeeDocument = new HashSet<EmployeeDocument>();
        }
        [Column("DocumentId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(500)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        [StringLength(1000)]
        [Display(Name = "Document Description")]
        public string DocumentDescription { get; set; }
        [Display(Name = "Is Expirable")]
        public bool IsExpirable { get; set; }
        public virtual ICollection<EmployeeDocument> EmployeeDocument { get; set; }
        [ForeignKey("NotificationSchedule")]
        public int? NotificationScheduleId { get; set; }
        public virtual NotificationSchedule NotificationSchedule { get; set; }
        [Display(Name = "Applies To")]
        public int? DocumentRequiredById { get; set; }
        public virtual DocumentRequiredBy DocumentRequiredBy { get; set; }
        public bool SelfServiceDisplay { get; set; }
        public bool SelfServiceUpload { get; set; }
    }
}
