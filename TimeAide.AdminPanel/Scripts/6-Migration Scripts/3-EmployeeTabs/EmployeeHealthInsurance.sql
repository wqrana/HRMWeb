CREATE PROCEDURE sp_DM_EmployeeHealthInsurance
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmployeeHealthInsurance] (
		     [Old_Id]
			,[UserInformationId]
			,[GroupId]
			,[IsEnlisted]
			,[InsuranceCoverageId]
			,[InsuranceTypeId]
			,[InsuranceStatusId]
			,[InsuranceStartDate]
			,[InsuranceExpiryDate]
			,[CobraStatusId]
			,[LeyCobraStartDate]
			,[LeyCobraExpiryDate]
			,[EmployeeContribution]
			,[CompanyContribution]
			,[OtherContribution]
			,[TotalContribution]
			,[PCORIFee]
			,[InsurancePremium]
            ,[ClientId]
            ,[CreatedBy]
            ,[CreatedDate]
            ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,[strGroupId]
		   ,CAST(IsNull([intEnlisted],0) AS Bit)
		   ,(select top(1) InsuranceCoverageId from InsuranceCoverage where old_ID = intCoverage And ClientId = @ClientId) as [intCoverage]
		   ,(select top(1) InsuranceTypeId from InsuranceType where old_ID = [intType] And ClientId = @ClientId) as [intType]
		   ,(select top(1) InsuranceStatusId from InsuranceStatus where old_ID = [intStatus] And ClientId = @ClientId) as [intStatus]
		   ,[dtStartDate]
		   ,[dtExpirationDate]
		   ,(select top(1) CobraStatusId from CobraStatus where old_ID = [intLeyCobraStatus] And ClientId = @ClientId) as [intLeyCobraStatus]  
		   ,[dtLeyCobraStart]
		   ,[dtLeyCobraExpiration]
		   ,IsNull([decEmployeeContribution],0)
		   ,IsNull([decCompanyContribution],0)
		   ,IsNull([decOtherContribution],0)
		   ,IsNull([decTotalContribution],0)
		   ,IsNull([decPCORIFee],0)
		   ,0
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblHealthInsurance]
	
END
