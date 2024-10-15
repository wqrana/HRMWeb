CREATE PROCEDURE sp_DM_Document
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Document] (
		    [Old_Id]
		   ,[DocumentName]
           ,[DocumentDescription]
		   ,[IsExpirable]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus]
		   ,[SelfServiceDisplay]
		   ,[SelfServiceUpload])
     SELECT [intID]
		   ,[strDocumentName]
		   ,[strDescription]
		   ,0
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
		   ,1
		   ,1
     FROM [TimeAideSource].[dbo].[tblDocumentList]
END	 
