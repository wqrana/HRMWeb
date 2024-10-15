CREATE PROCEDURE sp_DM_InsuranceCoverage
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[InsuranceCoverage] (
		    [Old_Id]
		   ,[InsuranceCoverageName]
           ,[Description]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strCoverage]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblHealthCoverage]
END