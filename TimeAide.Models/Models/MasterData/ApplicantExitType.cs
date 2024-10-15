using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantExitType")]
    public partial class ApplicantExitType : BaseEntity
    {
        [Column("ApplicantExitTypeId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ExitTypeName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string Description { get; set; }
    }
}
