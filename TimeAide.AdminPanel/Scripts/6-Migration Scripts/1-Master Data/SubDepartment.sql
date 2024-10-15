CREATE PROCEDURE sp_DM_SubDepartment
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[SubDepartment] (
				[Old_Id]
			   ,[SubDepartmentName]
			   ,[SubDepartmentDescription]
			   ,[DepartmentId]
			   ,[USECFSEAssignment]
			   ,[ClientId]
			   ,[CreatedBy]
			   ,[CreatedDate]
			   ,[DataEntryStatus])
	SELECT 		[intID]
			   ,[strSubDepartment]
			   ,[strDescription]
			   ,1
			   ,0
			   ,@ClientId
			   ,1
			   ,GetDate()
			   ,CAST(IsNull([intEnabled],1) AS INT)  
	FROM [TimeAideSource].[dbo].[tblSubDepartment]
END
