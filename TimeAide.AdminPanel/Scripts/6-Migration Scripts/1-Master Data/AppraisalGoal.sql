CREATE PROCEDURE sp_DM_AppraisalGoal
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalGoal] (
			 [Old_Id]
			,[GoalName]
			,[GoalDescription]
			,[AppraisalRatingScaleId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   [intID]
			,[strGoalName]
			,[strGoalDescription]
			,(select top(1) AppraisalRatingScaleId from AppraisalRatingScale where Old_Id = intRatingScale And ClientId = @ClientId) as intRatingScale 
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM [TimeAideSource].[dbo].[tblAppraisalGoals]
	 
END