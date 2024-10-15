CREATE PROCEDURE sp_DM_EmployeeDocument
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeDocument] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[DocumentId]
		   ,[DocumentName]
		   ,[DocumentPath]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) DocumentId from Document where Old_Id = intDocumentID And ClientId = @ClientId) as intDocumentID 
		   ,[docName]
		   ,null
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeDocument] WHERE strEmpID IS NOT NULL AND LTRIM(RTRIM(strEmpID))<>''
	
END
