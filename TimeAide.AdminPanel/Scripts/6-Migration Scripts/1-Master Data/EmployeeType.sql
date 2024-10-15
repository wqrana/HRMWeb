CREATE PROCEDURE sp_DM_EmployeeType
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[EmployeeType] (
		    [Old_Id]
		   ,[EmployeeTypeName]
           ,[EmployeeTypeDescription]
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
     FROM [TimeAideSource].[dbo].[tblEmployeeType]
END	 
