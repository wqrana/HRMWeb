CREATE PROCEDURE sp_DM_PerformanceDescription
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[PerformanceDescription] (
		    [Old_Id]
		   ,[PerformanceDescriptionName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strName]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblPerformanceDescription]
	 
END