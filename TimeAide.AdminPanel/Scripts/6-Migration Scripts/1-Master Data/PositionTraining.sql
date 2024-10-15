CREATE PROCEDURE sp_DM_PositionTraining
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[PositionTraining] (
		    [Old_Id]
		   ,[PositionId]
		   ,[TrainingId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) PositionId from Position where old_ID = intPositionID And ClientId = @ClientId) as intPositionID
		   ,(select top(1) TrainingId from [Training] where old_ID = intTrainingID And ClientId = @ClientId) as intTrainingID
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblPositionTrainings]
	 
END