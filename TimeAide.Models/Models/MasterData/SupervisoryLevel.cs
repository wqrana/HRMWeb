namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("SupervisoryLevel")]
    public partial class SupervisoryLevel : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupervisoryLevel()
        {
            UserInformations = new HashSet<UserInformation>();
        }

        [Display(Name = "Supervisory Level Id")]
        [Column("SupervisoryLevelId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Supervisory Level")]
        public string SupervisoryLevelName { get; set; }

        [StringLength(150)]
        [Display(Name = "Supervisory Level Description")]
        public string SupervisoryLevelDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInformation> UserInformations { get; set; }
    }
}
