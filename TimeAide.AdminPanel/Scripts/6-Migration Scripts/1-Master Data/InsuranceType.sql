CREATE PROCEDURE sp_DM_InsuranceType
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[InsuranceType] (
		    [Old_Id]
		   ,[InsuranceTypeName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strType]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblHealthType]
END