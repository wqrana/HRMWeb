namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 
    [Table("City")]
    public partial class City:BaseEntity
    {
        
        public City()
        {
        }

        [Display(Name = "City")]
        [Column("CityId")]
        public override int Id { get; set; }

        [StringLength(20)]
        [Display(Name = "Code")]
        public string CityCode { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "City")]
        public string CityName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string CityDescription { get; set; }

        public int StateId { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<UserContactInformation> MailingCityUserContactInformation { get; set; }
        public virtual ICollection<UserContactInformation> HomeCityUserContactInformation { get; set; }
    }
}
