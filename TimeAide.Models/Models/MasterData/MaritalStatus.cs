namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("MaritalStatus")]
    public partial class MaritalStatus : BaseEntity
    {
        
        public MaritalStatus()
        {
            UserInformation = new HashSet<UserInformation>();
        }
        [Column("MaritalStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Marital Status")]
        public string MaritalStatusName { get; set; }

        [StringLength(50)]
        [Display(Name = "Marital Status Description")]
        public string MaritalStatusDescription { get; set; }

        
        public virtual ICollection<UserInformation> UserInformation { get; set; }

    }
}
