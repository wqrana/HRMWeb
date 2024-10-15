CREATE PROCEDURE sp_DM_CustomField
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[CustomField] (
		    [Old_Id]
		   ,[CustomFieldName]
           ,[CustomFieldDescription]
		   ,[IsExpirable]
		   ,[FieldDisplayOrder]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intIndex]
		   ,[strName]
		   ,[strDescription]
		   ,0
		   ,[intIndex]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblCustomFieldsName]
END	 
