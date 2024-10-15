namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("Country")]
    public partial class Country : BaseEntity
    {
        
        public Country()
        {
            States = new HashSet<State>();
        }

        [Display(Name = "Country Id")]
        [Column("CountryId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Code")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Name")]
        public string CountryName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string CountryDescription { get; set; }

        
        public virtual ICollection<State> States { get; set; }

        public virtual ICollection<UserContactInformation> MailingCountryUserContactInformation { get; set; }
        public virtual ICollection<UserContactInformation> HomeCountryUserContactInformation { get; set; }
    }
}
