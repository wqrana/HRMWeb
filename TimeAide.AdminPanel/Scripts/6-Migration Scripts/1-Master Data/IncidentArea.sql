CREATE PROCEDURE sp_DM_IncidentArea
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[IncidentArea] (
		    [Old_Id]
		   ,[IncidentAreaName]
           ,[IncidentAreaDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentLocation]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentLocation]
END