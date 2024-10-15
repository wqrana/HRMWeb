namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CertificationType")]
    public partial class CertificationType : BaseEntity
    {
        public CertificationType()
        {
            Certification = new HashSet<Certification>();
        }

        [Display(Name = "Credential Type")]
        [Column("CertificationTypeId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Credential Type")]
        [StringLength(50)]
        public string CertificationTypeName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public virtual ICollection<Certification> Certification { get; set; }
    }
}
