CREATE PROCEDURE sp_DM_AppraisalTemplateGoal
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalTemplateGoal] (
			 [AppraisalTemplateId]
			,[AppraisalGoalId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   1
		    ,(select top(1) AppraisalGoalId from AppraisalGoal where Old_Id = [intID] And ClientId = @ClientId) as AppraisalGoalId  
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM [TimeAideSource].[dbo].[tblAppraisalGoals]
END