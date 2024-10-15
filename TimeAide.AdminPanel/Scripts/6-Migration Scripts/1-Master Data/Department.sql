CREATE PROCEDURE sp_DM_Department
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[Department] (
		    [Old_Id]
		   ,[DepartmentName]
           ,[DepartmentDescription]
		   ,[USECFSEAssignment]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strDepartment]
		   ,[strDescription]
		   ,0
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblDepartment]
	
END
