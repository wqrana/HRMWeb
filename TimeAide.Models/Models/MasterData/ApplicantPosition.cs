
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantPosition")]
    public partial class ApplicantPosition : BaseEntity
    {
        [Column("ApplicantPositionId")]
        public override int Id { get; set; }
        [Required]

        [Display(Name = "Name")]
        public string PositionName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string Description { get; set; }
    }
}
