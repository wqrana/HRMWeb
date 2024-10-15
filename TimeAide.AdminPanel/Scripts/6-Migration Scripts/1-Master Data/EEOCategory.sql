CREATE PROCEDURE sp_DM_EEOCategory
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[EEOCategory] (
		    [Old_Id]
		   ,[EEONumber]
		   ,[EEOCategoryName]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strEEONumber]
		   ,[strEEOCategory]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEEOCategories]
END	 
