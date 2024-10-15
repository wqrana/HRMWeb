CREATE PROCEDURE sp_DM_EmployeeBenefitHistory
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeBenefitHistory] (
		     [Old_Id]
			,[StartDate]
			,[Amount]
			,[BenefitId]
			,[PayFrequencyId]
			,[ExpiryDate]
			,[Notes]
			,[EmployeeContribution]
			,[CompanyContribution]
			,[OtherContribution]
			,[TotalContribution]
			,[UserInformationId]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT 
			 [intID]
			,[dtStartDate]
			,Case When ISNUMERIC([strAmount])=0  Then 0.0
				Else CAST(strAmount AS DECIMAL(10,2)) 
			 End as [strAmount]
			,(select top(1) BenefitId from Benefit where old_ID = intBenefitID And ClientId = @ClientId) as intBenefitID
			,(select top(1) [PayFrequencyId] from [PayFrequency] where old_ID = intFrequency And ClientId = @ClientId) as intFrequency 
			,[dtExpirationDate]
			,[strNotes]
			,[decEmployeeContribution]
			,[decCompanyContribution]
			,[decOtherContribution]
			,IsNull([decEmployeeContribution],0) + IsNull([decCompanyContribution],0) + IsNull([decOtherContribution],0)
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblBenefitHistory]
	
END
