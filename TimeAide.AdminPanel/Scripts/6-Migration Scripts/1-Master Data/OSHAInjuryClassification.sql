CREATE PROCEDURE sp_DM_OSHAInjuryClassification
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[OSHAInjuryClassification] (
		    [Old_Id]
		   ,[OSHAInjuryClassificationName]
           ,[OSHAInjuryClassificationDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentOSHAInjuryClassification]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentOSHAInjuryClassification]
END