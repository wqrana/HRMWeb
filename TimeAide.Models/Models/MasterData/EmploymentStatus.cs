namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    public partial class EmploymentStatus : BaseEntity
    {
        
        public EmploymentStatus()
        {
            Employment = new HashSet<Employment>();
            UserInformations = new HashSet<UserInformation>();
        }

        [Key]
        [Display(Name = "Employment Status ID")]
        [Column("EmploymentStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string EmploymentStatusName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string EmploymentStatusDescription { get; set; }

        //public bool Enabled { get; set; }

        //public int CreatedBy { get; set; }

        //public DateTime CreatedDate { get; set; }

        //public int? ModifiedBy { get; set; }

        //public DateTime? ModifiedDate { get; set; }

        //public int DataEntryStatus { get; set; }

        
        public virtual ICollection<UserInformation> UserInformations { get; set; }

        
        public virtual ICollection<Employment> Employment { get; set; }
        public virtual Client Client { get; set; }
    }
}
