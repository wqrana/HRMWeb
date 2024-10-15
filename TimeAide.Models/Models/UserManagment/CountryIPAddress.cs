namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("CountryIPAddress")]
    public partial class CountryIPAddress : BaseEntity
    {
        
        public CountryIPAddress()
        {
        }

        [Display(Name = "Country Id")]
        [Column("CountryId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [StringLength(150)]
        [Display(Name = "Country Description")]
        public string CountryDescription { get; set; }

        [StringLength(150)]
        [Display(Name = "Start IP Number")]
        public string StartIPNumber { get; set; }

        [StringLength(150)]
        [Display(Name = "End IP Number")]
        public string EndIPNumber { get; set; }
    }
}
