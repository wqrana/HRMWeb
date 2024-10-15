CREATE PROCEDURE sp_DM_Position
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Position] (
		    [Old_Id]
		   ,[PositionName]
		   ,[PositionDescription]
		   ,[PositionCode]
		   ,[DefaultPayScaleId]
		   ,[DefaultEEOCategoryId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strPosition]
		   ,[strDescription]
	       ,[strPositionCode]
		   ,(select top(1) PayScaleId from PayScale where old_ID = intDefaultPayScale And ClientId = @ClientId) as intDefaultPayScale
		   ,(select top(1) EEOCategoryId from EEOCategory where old_ID = intDefaultEEOCategory And ClientId = @ClientId) as intDefaultEEOCategory
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblPosition]
	 
END