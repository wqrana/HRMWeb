using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("IncidentTreatmentFacility")]
    public partial class IncidentTreatmentFacility : BaseEntity
    {
        [Column("IncidentTreatmentFacilityId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Facility Name")]
        public string TreatmentFacilityName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Facility Address")]
        public String TreatmentFacilityAddress {get; set;}
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string ZipCode { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
    }
}
