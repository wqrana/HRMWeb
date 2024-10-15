CREATE PROCEDURE sp_DM_CobraStatus
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[CobraStatus] (
		    [Old_Id]
		   ,[CobraStatusName]
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
     FROM [TimeAideSource].[dbo].[tblCobraStatus]
END	 
