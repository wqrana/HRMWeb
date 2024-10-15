CREATE PROCEDURE sp_DM_EmployeeAppraisalSkill
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeAppraisalSkill] (
		     [Old_Id]
			,[EmployeeAppraisalId]
			,[AppraisalSkillId]
			,[AppraisalRatingScaleDetailId]
			,[SkillRatingName]
			,[SkillRatingValue]
			,[SkillScaleMaxValue]
			,[ReviewerComments]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  
			 [intID]
			,(select top(1) EmployeeAppraisalId from EmployeeAppraisal where Old_Id = intAppraisalID And ClientId = @ClientId) as intAppraisalID
			,IsNull((select top(1) AppraisalSkillId from AppraisalSkill where SkillName = strSkillName And ClientId = @ClientId),(select top(1) AppraisalSkillId from AppraisalSkill where ClientId = @ClientId)) as strSkillName
			,(select top(1) AppraisalRatingScaleDetailId from AppraisalRatingScaleDetail where Old_Id = intRatingScale And ClientId = @ClientId) as intRatingScale   
			,[strReviewerRatingName]
			,IsNull([intReviewerRatingValue],0)
			,0
			,[strReviewerComments]
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblEmployeeAppraisalsSkills]
	
END
