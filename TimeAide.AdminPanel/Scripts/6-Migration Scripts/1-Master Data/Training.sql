CREATE PROCEDURE sp_DM_Training
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[Training] (
		    [Old_Id]
		   ,[TrainingName]
           ,[TrainingDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strTrainingName]
		   ,[strTrainingDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblTrainingName]
	 
END