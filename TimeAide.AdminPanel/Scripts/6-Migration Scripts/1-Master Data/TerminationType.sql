CREATE PROCEDURE sp_DM_TerminationType
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[TerminationType] (
		    [Old_Id]
		   ,[TerminationTypeName]
           ,[TerminationTypeDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strName]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblTerminationType]
END