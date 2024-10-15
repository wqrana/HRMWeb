CREATE PROCEDURE sp_DM_EmployeeAppraisal
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeAppraisal] (
		     [Old_Id]
			,[UserInformationId]
			,[PositionId]
			,[CompanyId]
			,[DepartmentId]
			,[EmployeeTypeId]
			,[AppraisalReviewDate]
			,[AppraisalDueDate]
			,[EvaluationStartDate]
			,[EvaluationEndDate]
			,[NextAppraisalDueDate]
			,[AppraisalReviewerComments]
			,[AppraisalEmployeeComments]
			,[AppraisalTemplateId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,(select top(1) PositionId from Position where old_ID = intPosition And ClientId = @ClientId) as intPosition
			,(select top(1) CompanyId from Company where old_ID = intCompany And ClientId = @ClientId) as intCompany
			,IsNull((select top(1) DepartmentId from Department where old_ID = intDepartment And ClientId = @ClientId),1) as intDepartment
			,(select top(1) EmploymentTypeId from EmploymentType where old_ID = intEmploymentType And ClientId = @ClientId) as intEmploymentType
			,[dtReviewDate]
			,[dtDueDate]
			,[dtEvaluationPeriodStart]
			,[dtEvaluationPeriodEnd]
			,[dtNextAppraisal]
			,[strReviewerComments]
			,[strEmployeeComments]
			
			,(select top(1) AppraisalTemplateId from AppraisalTemplate  where AppraisalTemplate.ClientId = @ClientId ) as AppraisalTemplateId
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblEmployeeAppraisals]
	
END
