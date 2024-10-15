namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ethnicity")]
    public partial class Ethnicity : BaseEntity
    {
        public Ethnicity()
        {
            UserInformation = new HashSet<UserInformation>();
        }

        [Column("EthnicityId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string EthnicityName { get; set; }

        [StringLength(1000)]
        public string EthnicityDescription { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
    }
}
