CREATE PROCEDURE sp_DM_CredentialType
	@ClientId int
AS
BEGIN
	
	 INSERT INTO [dbo].[CredentialType] (
		    [Old_Id]
		   ,[CredentialTypeName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strCertificationType]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT) 
     FROM [TimeAideSource].[dbo].[tblCertificationType]
END	 
