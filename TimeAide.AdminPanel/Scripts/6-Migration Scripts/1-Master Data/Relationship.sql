CREATE PROCEDURE sp_DM_Relationship
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[Relationship] (
			RelationshipName,
			RelationshipDescription,
			ClientId	,
			CreatedBy	,
			CreatedDate	,
			DataEntryStatus
			)
     SELECT 
			distinct strEmergencyRelationship
		   ,Null
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeContact] Where LTRIM(RTRIM(strEmergencyRelationship))<>'' And Not strEmergencyRelationship is null
	 
END