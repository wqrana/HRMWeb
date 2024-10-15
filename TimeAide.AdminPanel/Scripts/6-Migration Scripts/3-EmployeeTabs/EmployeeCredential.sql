CREATE PROCEDURE sp_DM_EmployeeCredential
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeCredential] (
		    [Old_Id]
		   ,[UserInformationId]
		   ,[EmployeeCredentialName]
		   ,[EmployeeCredentialDescription]
		   ,[IssueDate]
           
		   ,[CredentialId]
		   ,[Note]
		   ,[ExpirationDate]
		   ,[ExpirationDateRequired]
		   ,[IsRequired]
		   ,[DocumentName]
		   ,[DocumentPath]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,[strCredentialID]
		   ,[strDescription]
		   ,[dtIssueDate]
		   
		   ,(select top(1) CredentialId from [Credential] where Old_Id = intName And ClientId = @ClientId) as intName 
		   ,[strNote]
		   ,[dtExpirationDate]
		   ,[intExpirationDateRequired]
		   ,0
		   ,[docName]
		   ,null
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeCredentials]
	
END
