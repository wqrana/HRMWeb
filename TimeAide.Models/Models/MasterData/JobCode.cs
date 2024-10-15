namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("JobCode")]
    public partial class JobCode : BaseEntity
    {
        
        public JobCode()
        {
            UserInformations = new HashSet<UserInformation>();
        }
        [Display(Name = "Job Code Id")]
        [Column("JobCodeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Job Code")]
        public string JobCodeName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string JobCodeDescription { get; set; }

        public bool Enabled { get; set; }

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        //public int CreatedBy { get; set; }

        //public DateTime CreatedDate { get; set; }

        //public int? ModifiedBy { get; set; }

        //public DateTime? ModifiedDate { get; set; }

        //public int DataEntryStatus { get; set; }

        
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual Client Client { get; set; }
    }
}
