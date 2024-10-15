CREATE PROCEDURE sp_DM_IncidentInjurySource
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[IncidentInjurySource] (
		    [Old_Id]
		   ,[IncidentInjurySourceName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentInjurySource]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentInjurySource]
END