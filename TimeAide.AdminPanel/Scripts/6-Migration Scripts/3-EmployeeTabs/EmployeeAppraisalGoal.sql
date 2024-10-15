CREATE PROCEDURE sp_DM_EmployeeAppraisalGoal
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeAppraisalGoal] (
		     [Old_Id]
			,[EmployeeAppraisalId]
			,[AppraisalGoalId]
			,[AppraisalRatingScaleDetailId]
			,[GoalRatingName]
			,[GoalRatingValue]
			,[GoalScaleMaxValue]
			,[ReviewerComments]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  
			 [intID]
			,(select top(1) EmployeeAppraisalId from EmployeeAppraisal where Old_Id = intAppraisalID And ClientId = @ClientId) as intAppraisalID
			,(select top(1) AppraisalGoalId from AppraisalGoal where GoalName = strGoalName And ClientId = @ClientId) as strGoalName
			,(select top(1) AppraisalRatingScaleDetailId from AppraisalRatingScaleDetail where Old_Id = intRatingScale And ClientId = @ClientId) as intRatingScale   
			,[strReviewerRatingName]
			,IsNull([intReviewerRatingValue],0)
			,0
			,[strReviewerComments]
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblEmployeeAppraisalsGoals]
	
END
