CREATE PROCEDURE sp_DM_MaritalStatus
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[MaritalStatus] (
		    [Old_Id]
		   ,[MaritalStatusName]
           ,[MaritalStatusDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strStatus]
		   ,'' as [strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblMaritalStatus]
	 
END