using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantReferenceSource")]
    public partial class ApplicantReferenceSource : BaseEntity
    {
       
        [Column("ApplicantReferenceSourceId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Reference Source")]
        [StringLength(150)]
        public string ReferenceSourceName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string Description { get; set; }
       
    }
}
