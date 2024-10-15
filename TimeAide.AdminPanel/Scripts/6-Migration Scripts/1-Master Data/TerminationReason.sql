CREATE PROCEDURE sp_DM_TerminationReason
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[TerminationReason] (
		    [Old_Id]
		   ,[TerminationReasonName]
           ,[TerminationReasonDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strTerminationReason]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblTerminationReason]
END