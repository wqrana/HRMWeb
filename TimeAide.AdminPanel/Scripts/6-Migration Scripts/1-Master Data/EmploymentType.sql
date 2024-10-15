CREATE PROCEDURE sp_DM_EmploymentType
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[EmploymentType] (
		    [Old_Id]
		   ,[EmploymentTypeName]
           ,[EmploymentTypeDescription]
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
     FROM [TimeAideSource].[dbo].[tblEmploymentType]
END	 
