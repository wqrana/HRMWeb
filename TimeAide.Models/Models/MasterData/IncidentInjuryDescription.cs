using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentInjuryDescription")]
    public partial class IncidentInjuryDescription : BaseEntity
    {
        [Column("IncidentInjuryDescriptionId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Injury Name")]
        public string IncidentInjuryDescriptionName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
