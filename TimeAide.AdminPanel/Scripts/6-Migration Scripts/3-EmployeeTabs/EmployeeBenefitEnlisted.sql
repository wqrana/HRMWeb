CREATE PROCEDURE sp_DM_EmployeeBenefitEnlisted
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeBenefitEnlisted] (
		     [Old_Id]
			,[UserInformationId]
			,[BenefitId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,(select top(1) BenefitId from Benefit where old_ID = intBenefitID And ClientId = @ClientId) as intBenefitID
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblBenefitEnlisted]
	
END
