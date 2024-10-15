CREATE PROCEDURE sp_DM_EmergencyContact
	@ClientId int
AS
BEGIN

INSERT INTO [dbo].[EmergencyContact] (
			UserInformationId,
			RelationshipId,
			ContactPersonName,
			IsDefault,
			MainNumber,
			AlternateNumber,
			ClientId,
			CreatedBy,
			CreatedDate,
			DataEntryStatus
			)
SELECT 
			(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID,
			(select top(1) RelationshipId from Relationship where RelationshipName = strEmergencyRelationship And ClientId = @ClientId) as strEmergencyRelationship,
			strEmergencyContact,
			0,
			strEmergencyNumber1,
			strEmergencyNumber2,
			@ClientId As ClientId	,
			1 As CreatedBy	,
			GetDate() CreayedDate	,
			1 as DataEntrtyStatus
     FROM [TimeAideSource].[dbo].[tblEmployeeContact] 
	 Where (LTRIM(RTRIM(strEmergencyRelationship)) <> '' And Not strEmergencyRelationship is null) 
END