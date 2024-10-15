CREATE PROCEDURE sp_DM_InsuranceStatus
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[InsuranceStatus] (
		    [Old_Id]
		   ,[InsuranceStatusName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strStatus]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblHealthStatus]
END