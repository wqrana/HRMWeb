CREATE PROCEDURE sp_DM_IncidentInjuryDescription
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[IncidentInjuryDescription] (
		    [Old_Id]
		   ,[IncidentInjuryDescriptionName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentInjuryDescription]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentInjuryDescription]
END