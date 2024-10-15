using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentType")]
    public partial class IncidentType : BaseEntity
    {
        [Column("IncidentTypeId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Incident Type")]
        public string IncidentTypeName { get; set; }
        [Display(Name = "Description")]
        public string IncidentTypeDescription { get; set; }
    }
}
