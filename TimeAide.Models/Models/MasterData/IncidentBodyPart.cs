
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentBodyPart")]
    public partial class IncidentBodyPart : BaseEntity
    {
        [Column("IncidentBodyPartId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Part Name")]
        public string IncidentBodyPartName { get; set; }
        [Display(Name = "Description")]
        public string IncidentBodyPartDescription { get; set; }
    }
}
