CREATE PROCEDURE sp_DM_AppraisalSkill
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalSkill] (
			 [Old_Id]
			,[SkillName]
			,[SkillDescription]
			,[AppraisalRatingScaleId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   [intID]
			,[strSkillName]
			,[strSkillDescription]
			,(select top(1) AppraisalRatingScaleId from AppraisalRatingScale where Old_Id = intRatingScale And ClientId = @ClientId) as intRatingScale 
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM [TimeAideSource].[dbo].[tblAppraisalSkills]
	 
END