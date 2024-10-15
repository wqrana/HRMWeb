CREATE PROCEDURE sp_DM_EmployeeTraining
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeTraining] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[TrainingId]
		   ,[Type]
		   ,[TrainingDate]
		   ,[ExpiryDate]
		   ,[Note]
		   ,[DocName]
		   ,[DocFilePath]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) TrainingId from Training where Old_Id = intTrainingName And ClientId = @ClientId) as intTrainingName 
		   ,[strTrainingType]
		   ,[dtTrainingDate]
		   ,[dtExpirationDate]
		   ,[strTrainingNote]
		   ,[docName]
		   ,NULL
		   ,@ClientId
		   ,1
		   ,GetDate()  
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeTraining]
	
END

