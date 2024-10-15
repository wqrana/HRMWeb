CREATE PROCEDURE sp_DM_AppraisalTemplateSkill
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalTemplateSkill] (
			 [AppraisalTemplateId]
			,[AppraisalSkillId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   1
		    ,(select top(1) AppraisalSkillId from AppraisalSkill where Old_Id = [intID] And ClientId = @ClientId) as AppraisalSkillId  
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM [TimeAideSource].[dbo].[tblAppraisalSkills]
END