CREATE PROCEDURE sp_DM_ActionType
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[ActionType] (
		    [Old_Id]
		   ,[ActionTypeName]
           ,[ActionTypeDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strActionTypeName]
		   ,[strActionTypeDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblActionType]
	 
END