CREATE PROCEDURE sp_DM_PayType
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[PayType] (
		    [Old_Id]
		   ,[PayTypeName]
           ,[PayTypeDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strPayType]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblPayType]
	 
END