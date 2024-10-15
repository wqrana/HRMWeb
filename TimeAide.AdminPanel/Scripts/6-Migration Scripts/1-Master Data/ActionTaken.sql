CREATE PROCEDURE sp_DM_ActionTaken
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[ActionTaken] (
		    [Old_Id]
		   ,[ActionTakenName]
           ,[ActionTakenDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strActionTaken]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblActionTaken]
	 
END