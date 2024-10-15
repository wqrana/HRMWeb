CREATE PROCEDURE sp_DM_EmploymentStatus
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[EmploymentStatus] (
		    [Old_Id]
		   ,[EmploymentStatusName]
           ,[EmploymentStatusDescription]
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
     FROM [TimeAideSource].[dbo].[tblEmploymentStatus]
	 
END