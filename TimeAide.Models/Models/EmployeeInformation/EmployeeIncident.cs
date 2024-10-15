 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeIncident")]
    public partial class EmployeeIncident : BaseUserObjects
    {
        [Column("EmployeeIncidentId")]
        public override int Id { get; set; }
        public int? IncidentTypeId { get; set; }
        public int? LocationId { get; set; }
        public int? IncidentAreaId { get; set; }
        public int? OSHACaseClassificationId { get; set; }
        public int? OSHAInjuryClassificationId { get; set; }
        public int? IncidentBodyPartId { get; set; }
        public int? IncidentInjuryDescriptionId { get; set; }
        public int? IncidentInjurySourceId { get; set; }
        public int? IncidentTreatmentFacilityId { get; set; }
        public bool IsOSHARecordable { get; set; }
        public DateTime? IncidentDate { get; set; }
        public DateTime? IncidentTime { get; set; }
        public DateTime? EmployeeBeganWorkTime { get; set; }
        public int? RestrictedFromWorkDays { get; set; }
        public int? AwayFromWorkDays { get; set; }
        public string EmployeeDoingBeforeIncident { get; set; }
        public string HowIncidentOccured { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string PhysicianName { get; set; }
        public bool IsTreatedInEmergencyRoom { get; set; }
        public bool IsHospitalizedOvernight { get; set; }
        public int? HospitalizedDays { get; set; }
        public int? CompletedById { get; set; }
        public DateTime? CompletedDate { get; set; }
        public virtual IncidentType IncidentType { get; set; }
        public virtual Location Location { get; set; }
        public virtual IncidentArea IncidentArea { get; set; }
        public virtual OSHACaseClassification OSHACaseClassification { get; set; }
        public virtual OSHAInjuryClassification OSHAInjuryClassification { get; set; }
        public virtual IncidentBodyPart IncidentBodyPart { get; set; }
        public virtual IncidentInjuryDescription IncidentInjuryDescription { get; set; }
        public virtual IncidentInjurySource IncidentInjurySource { get; set; }
        public virtual IncidentTreatmentFacility IncidentTreatmentFacility { get; set; }
        [ForeignKey("CompletedById")]
        public virtual UserInformation CompletedBy { get; set; }


    }
}
