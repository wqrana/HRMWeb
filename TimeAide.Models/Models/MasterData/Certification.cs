namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Certification")]
    public partial class Certification : BaseEntity
    {
        
        public Certification()
        {
            //EmployeeCredential = new HashSet<EmployeeCredential>();
        }

        [Display(Name = "Certification")]
        [Column("CertificationId")]
        public override int Id { get; set; }

        [Column("CertificationName")]
        [Display(Name = "Certification")]
        [Required]
        [StringLength(50)]
        public string CertificationName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Display(Name = "Credential Type")]
        public int? CertificationTypeId { get; set; }

        public virtual CertificationType CertificationType { get; set; }
        //public virtual ICollection<EmployeeCredential> EmployeeCredential { get; set; }
    }
}
