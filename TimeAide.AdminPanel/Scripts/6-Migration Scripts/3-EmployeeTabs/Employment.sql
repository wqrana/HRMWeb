CREATE PROCEDURE sp_DM_Employment
	@ClientId int
AS
BEGIN

INSERT INTO [dbo].[Employment] (
			 [Old_Id]
			,[UserInformationId]
			,[OriginalHireDate]
			,[EffectiveHireDate]
			,[ProbationStartDate]
			,[ProbationEndDate]
			,[EmploymentStatusId]
			,[TerminationDate]
			,[TerminationTypeId]
			,[TerminationReasonId]
			,[DocumentName]
			,[DocumentPath]
			,[TerminationEligibilityId]
			,[TerminationNotes]
			,[UseHireDateforYearsInService]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]	
			)
SELECT 
			[intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
			,[dtOriginalHireDate]
			,[dtEffectiveHireDate]
			,[dtProbationStart]
			,[dtProbationEnd]
			,(select top(1) EmploymentStatusId from EmploymentStatus where old_ID = intStatus And ClientId = @ClientId) as intStatus --intStatus
			,[dtTerminationDate]
			,(select top(1) TerminationTypeId from TerminationType where old_ID = intTerminationType And ClientId = @ClientId) as intTerminationType --intTerminationType
			,(select top(1) TerminationReasonId from TerminationReason where old_ID = intTerminationReason And ClientId = @ClientId) as intTerminationReason --intTerminationReason
			,[docName]
			,NULL
			,(select top(1) TerminationEligibilityId from TerminationEligibility where old_ID = intTerminationEligibility And ClientId = @ClientId) as intTerminationEligibility --intTerminationEligibility
			,[strTerminationNotes]
			,0
			,@ClientId
			,1
			,GetDate()
			,1	
     FROM [TimeAideSource].[dbo].[tblEmployment] 
END
