using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentArea")]
    public partial class IncidentArea : BaseCompanyObjects
    {
        [Column("IncidentAreaId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Incident Area")]
        public string IncidentAreaName { get; set; }
        [Display(Name = "Description")]
        public string IncidentAreaDescription { get; set; }
    }
}
