CREATE PROCEDURE sp_DM_EmployeeCustomField
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeCustomField] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[CustomFieldId]
		   ,[CustomFieldValue]
		   ,[ExpirationDate]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) CustomFieldId from CustomField where Old_Id = intCustomFieldIndex And ClientId = @ClientId) as intCustomFieldIndex 
		   ,[strText]
		   ,[dtDate]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeCustomFields]
	
END
