using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentInjurySource")]
    public partial class IncidentInjurySource : BaseEntity
    {
        [Column("IncidentInjurySourceId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Injury Source")]
        public string IncidentInjurySourceName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
