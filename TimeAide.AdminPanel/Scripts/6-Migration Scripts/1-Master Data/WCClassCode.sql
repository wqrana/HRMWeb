CREATE PROCEDURE sp_DM_WCClassCode
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[WCClassCode] (
		    [Old_Id]
		   ,[ClassName]
		   ,[ClassCode]
           ,[ClassDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strClassName]
		   ,[strClassCode]
		   ,[strClassDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblWCClassCodes]
	 
END