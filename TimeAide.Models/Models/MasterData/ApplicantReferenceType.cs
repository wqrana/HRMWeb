using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantReferenceType")]
    public partial class ApplicantReferenceType : BaseEntity
    {

        [Column("ApplicantReferenceTypeId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Reference Type")]
        [StringLength(150)]
        public string ReferenceTypeName { get; set; }
        [Display(Name = "Description")]
        [StringLength(150)]
        public string Description { get; set; }

    }
}
