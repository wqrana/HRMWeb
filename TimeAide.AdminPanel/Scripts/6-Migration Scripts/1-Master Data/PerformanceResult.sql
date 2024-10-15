CREATE PROCEDURE sp_DM_PerformanceResult
	@ClientId int
AS
BEGIN
	INSERT INTO [dbo].[PerformanceResult] (
		    [Old_Id]
		   ,[PerformanceResultName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strResult]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblResult]
END