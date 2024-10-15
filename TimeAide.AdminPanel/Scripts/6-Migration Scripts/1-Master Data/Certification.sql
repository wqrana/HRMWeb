CREATE PROCEDURE sp_DM_Certification
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Certification] (
		    [Old_Id]
		   ,[CertificationName]
           ,[Description]
		   ,[CertificationTypeId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strCertificationTitle]
		   ,[strDescription]
		   ,(select top(1) CertificationTypeId from CertificationType where old_ID = intCertificationType And ClientId = @ClientId) as intCertificationType
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblCertificationTitle]
END	 
