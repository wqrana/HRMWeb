namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Disability")]
    public partial class Disability : BaseEntity
    {
        public Disability()
        {
            UserInformation = new HashSet<UserInformation>();
        }

        [Column("DisabilityId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string DisabilityName { get; set; }

        [StringLength(1000)]
        public string DisabilityDescription { get; set; }

        
        public virtual ICollection<UserInformation> UserInformation { get; set; }
    }
}
