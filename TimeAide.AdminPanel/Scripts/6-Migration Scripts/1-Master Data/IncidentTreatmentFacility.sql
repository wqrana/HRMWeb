CREATE PROCEDURE sp_DM_IncidentTreatmentFacility
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[IncidentTreatmentFacility] (
		    [Old_Id]
		   ,[TreatmentFacilityName]
           ,[Description]
		   ,[TreatmentFacilityAddress]
		   ,[StateId]
		   ,[CityId]
		   ,[ZipCode]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strFacilityName]
		   ,[strDescription]
		   ,[strFacilityAddress]
		   ,IsNull((select top(1) StateId from City where CityName = strFacilityCity),(select top(1) StateId from [State] where ClientId = @ClientId)) as strFacilityState
		   ,IsNull((select top(1) CityId from City where CityName = strFacilityCity),(select top(1) CityId from City where ClientId = @ClientId)) as strFacilityCity
		   ,[strFacilityZipCode]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentTreatmentFacility]
END