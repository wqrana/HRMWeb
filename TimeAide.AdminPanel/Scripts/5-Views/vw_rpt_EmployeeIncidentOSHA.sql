CREATE VIEW [dbo].[vw_rpt_EmployeeIncidentOSHA]
AS
SELECT 
empIncOSHA.EmployeeIncidentId,
empIncOSHA.UserInformationId,
empIncOSHA.IsOSHARecordable,
empIncOSHA.IncidentTypeId,
IncType.IncidentTypeName,
comp.NAICS,
comp.SIC,
empIncOSHA.LocationId, 
loc.LocationName,
loc.Address as LocationAddress,
loc.ZipCode as LocationZipCode,
st.StateName,
ci.CityName,
empIncOSHA.IncidentAreaId, 
IncArea.IncidentAreaName,
empIncOSHA.OSHACaseClassificationId,
OSHACase.OSHACaseClassificationName,
empIncOSHA.OSHAInjuryClassificationId,
OSHAInjury.OSHAInjuryClassificationName,
empIncOSHA.IncidentBodyPartId, 
IncidentBody.IncidentBodyPartName,
empIncOSHA.IncidentInjuryDescriptionId,
IncidentInjury.IncidentInjuryDescriptionName,
empIncOSHA.IncidentInjurySourceId,
IncidentInjurySrc.IncidentInjurySourceName,
empIncOSHA.IncidentTreatmentFacilityId,
IncidentTrtFac.TreatmentFacilityName,
IncidentTrtFac.TreatmentFacilityAddress,
IncidentTrtFac.ZipCode AS TreatmentFacilityZipCode,
TreatmentFacilityState.StateName TreatmentFacilityStateName,
TreatmentFacilityCity.CityName As TreatmentFacilityCityName,
empIncOSHA.IncidentDate, 
empIncOSHA.IncidentTime, 
empIncOSHA.EmployeeBeganWorkTime, 
empIncOSHA.RestrictedFromWorkDays, 
empIncOSHA.AwayFromWorkDays, 
empIncOSHA.EmployeeDoingBeforeIncident, 
empIncOSHA.HowIncidentOccured, 
empIncOSHA.DateOfDeath, 
empIncOSHA.PhysicianName, 
empIncOSHA.IsTreatedInEmergencyRoom, 
empIncOSHA.IsHospitalizedOvernight, 
empIncOSHA.HospitalizedDays, 
empIncOSHA.CompletedById, 
CompletedBy.ShortFullName CompletedByEmployee,
empIncOSHA.CompletedDate, 
empIncOSHA.ClientId
From EmployeeIncident empIncOSHA
Left Join IncidentType IncType On IncType.IncidentTypeId = empIncOSHA.IncidentTypeId
Left Join Location loc On loc.LocationId = empIncOSHA.LocationId
Left Join Company comp On comp.CompanyId =  loc.CompanyId
Left Join State st On st.StateId = loc.LocationStateId
Left Join City ci On ci.CityId = loc.LocationCityId 
Left Join IncidentArea IncArea On IncArea.IncidentAreaId = empIncOSHA.IncidentAreaId
Left Join OSHACaseClassification OSHACase On OSHACase.OSHACaseClassificationId = empIncOSHA.OSHACaseClassificationId
Left Join OSHAInjuryClassification OSHAInjury On OSHAInjury.OSHAInjuryClassificationId = empIncOSHA.OSHAInjuryClassificationId
Left Join IncidentBodyPart IncidentBody On IncidentBody.IncidentBodyPartId = empIncOSHA.IncidentBodyPartId
Left Join IncidentInjuryDescription IncidentInjury On IncidentInjury.IncidentInjuryDescriptionId = empIncOSHA.IncidentInjuryDescriptionId
Left Join IncidentInjurySource IncidentInjurySrc On IncidentInjurySrc.IncidentInjurySourceId = empIncOSHA.IncidentInjurySourceId
Left Join IncidentTreatmentFacility IncidentTrtFac On IncidentTrtFac.IncidentTreatmentFacilityId = empIncOSHA.IncidentTreatmentFacilityId
Left Join State TreatmentFacilityState On TreatmentFacilityState.StateId = IncidentTrtFac.StateId
Left Join City TreatmentFacilityCity On TreatmentFacilityCity.CityId = IncidentTrtFac.CityId 
Left Join UserInformation CompletedBy On CompletedBy.UserInformationId = empIncOSHA.CompletedById

Where empIncOSHA.DataEntryStatus = 1
And empIncOSHA.IsOSHARecordable = 1
