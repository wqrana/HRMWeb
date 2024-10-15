CREATE PROCEDURE sp_DM_TerminationEligibility
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[TerminationEligibility] (
		    [Old_Id]
		   ,[TerminationEligibilityName]
           ,[TerminationEligibilityDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strTerminationEligibility]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblTerminationEligibility]
	 
END