CREATE PROCEDURE sp_DM_Credential
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Credential] (
		    [Old_Id]
		   ,[CredentialName]
           ,[CredentialDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus]
		   ,[SelfServiceDisplay]
		   ,[SelfServiceUpload])
     SELECT [intID]
		   ,[strCredentialName]
		   ,[strCredentialDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
		   ,1
		   ,1
     FROM [TimeAideSource].[dbo].[tblCredentialName]
END	 
