using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantCompany")]
    public partial class ApplicantCompany : BaseEntity
    {
    
        [Column("ApplicantCompanyId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string Description { get; set; }
      }
}
