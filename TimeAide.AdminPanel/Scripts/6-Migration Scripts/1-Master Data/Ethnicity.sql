CREATE PROCEDURE sp_DM_Ethnicity
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Ethnicity] (
		    [Old_Id]
		   ,[EthnicityName]
           ,[EthnicityDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strEthnicity]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEEOEthnicity]
END