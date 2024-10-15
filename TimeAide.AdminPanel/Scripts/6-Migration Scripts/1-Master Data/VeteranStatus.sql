Create PROCEDURE sp_DM_VeteranStatus
	@ClientId int
AS
BEGIN
	INSERT INTO [dbo].[VeteranStatus] (
		    [Old_Id]
		   ,[VeteranStatusName]
           ,[VeteranStatusDescription]
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
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEEOVeteranStatus]
END