namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("State")]
    public partial class State : BaseEntity
    {
        
        public State()
        {
            Cities = new HashSet<City>();
        }

        [Display(Name = "State Id")]
        [Column("StateId")]
        public override int Id { get; set; }

        [StringLength(20)]
        [Display(Name = "Code")]
        public string StateCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "State")]
        public string StateName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string StateDescription { get; set; }

        public int CountryId { get; set; }

        
        public virtual ICollection<City> Cities { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<UserContactInformation> MailingStateUserContactInformation { get; set; }
        public virtual ICollection<UserContactInformation> HomeStateUserContactInformation { get; set; }
    }
}
