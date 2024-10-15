CREATE PROCEDURE sp_DM_IncidentBodyPart
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[IncidentBodyPart] (
		    [Old_Id]
		   ,[IncidentBodyPartName]
           ,[IncidentBodyPartDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentBodyParts]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentBodyParts]
END