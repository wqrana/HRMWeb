CREATE PROCEDURE sp_DM_EmployeeIncident
	@ClientId int
AS
BEGIN
	INSERT INTO [dbo].[EmployeeIncident] (
		     [Old_Id]
			,[UserInformationId]
			,[IncidentTypeId]
			,[LocationId]
			,[IncidentAreaId]
			,[OSHACaseClassificationId]
			,[OSHAInjuryClassificationId]
			,[IncidentBodyPartId]
			,[IncidentInjuryDescriptionId]
			,[IncidentInjurySourceId]
			,[IncidentTreatmentFacilityId]
			,[IsOSHARecordable]
			,[IncidentDate]
			,[IncidentTime]
			,[EmployeeBeganWorkTime]
			,[RestrictedFromWorkDays]
			,[AwayFromWorkDays]
			,[EmployeeDoingBeforeIncident]
			,[HowIncidentOccured]
			,[DateOfDeath]
			,[PhysicianName]
			,[IsTreatedInEmergencyRoom]
			,[IsHospitalizedOvernight]
			,[HospitalizedDays]
			,[CompletedById]
			,[CompletedDate]
			,[ClientId]
            ,[CreatedBy]
            ,[CreatedDate]
            ,[DataEntryStatus]
		)
     SELECT  [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,intIncidentType 
			,(select top(1) LocationId from [Location] where Old_Id = intEstablishment And ClientId = @ClientId) as [intEstablishment]
			,(select top(1) IncidentAreaId from IncidentArea where Old_Id = [intIncidentLocation] And ClientId = @ClientId) as [intIncidentLocation]
			,[intIncidentClassification]
			,[intOSHAInjuryClassification]
			,(select top(1) IncidentBodyPartId from IncidentBodyPart where Old_Id = [intBodyPartAffected] And ClientId = @ClientId) as [intBodyPartAffected]
			,(select top(1) IncidentInjuryDescriptionId from IncidentInjuryDescription where Old_Id = [intInjuryDescription] And ClientId = @ClientId) [intInjuryDescription]
			,(select top(1) IncidentInjurySourceId from IncidentInjurySource where Old_Id = [intInjurySource] And ClientId = @ClientId) [intInjurySource]
			,(select top(1) IncidentTreatmentFacilityId from IncidentTreatmentFacility where Old_Id = [intTreatmentLocation] And ClientId = @ClientId) [intTreatmentLocation]
			,IsNull([intOshaRecordable],0) 
			,[dtIncidentDate]
			,[dtIncidentTime]
			,[dtTimeEmployeeBeganWork]
			,[intDaysRestrictedFromWork]
			,[intDaysAwayFromWork]
			,[strEmployeeDoingBeforeIncident]
			,[strHowIncidentOccured]
			,[dtDateOfDeath]
			,[strPhysicianName]
			,IsNull([intTreatedInEmergencyRoom],0) 
			,IsNull([intHospitalizedOvernight],0) 
			,IsNull([intDaysHospitalized],0) 
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strCompletedBy And ClientId = @ClientId) as strCompletedBy
			,[dtCompletedDate]
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
     FROM [TimeAideSource].[dbo].[tblIncidents]
	 
END