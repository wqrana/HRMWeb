CREATE PROCEDURE sp_DM_PositionCredential
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[PositionCredential] (
		    [Old_Id]
		   ,[PositionId]
		   ,[CredentialId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) PositionId from Position where old_ID = intPositionID And ClientId = @ClientId) as intPositionID
		   ,(select top(1) CredentialId from [Credential] where old_ID = intCredentialID And ClientId = @ClientId) as intCredentialID
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblPositionCredentials]
	 
END