namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  

    [Table("EmergencyContact")]
    public partial class EmergencyContact : BaseEntity
    {
        public EmergencyContact()
        {
            ChangeRequestEmergencyContact = new HashSet<ChangeRequestEmergencyContact>();
        }

        [Column("EmergencyContactId")]
        public override int Id { get; set; }

        public int? UserInformationId { get; set; }
        [Required]
        public int? RelationshipId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Contact Name")]
        public string ContactPersonName { get; set; }

        public bool IsDefault { get; set; }

        [Required]
        [StringLength(20)]
        public string MainNumber { get; set; }

        [StringLength(20)]
        public string AlternateNumber { get; set; }

        [NotMapped]
        public virtual string RelationshipName { get; set; }

        public virtual Relationship Relationship { get; set; }

        public virtual UserInformation UserInformation { get; set; }

        public virtual ICollection<ChangeRequestEmergencyContact> ChangeRequestEmergencyContact { get; set; }

        [NotMapped]
        public int RequestTypeId { get; set; }
        [NotMapped]
        //[Required]
        public string ReasonForDelete { get; set; }

        [NotMapped]
        public string SefServiceRemarks { get; set; }
    }
}
