CREATE PROCEDURE sp_DM_PayInformationHistory
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[PayInformationHistory] (
			 [Old_Id]
			,[StartDate]
			,[EndDate]
			
			,[EEOCategoryId]
			,[PayTypeId]
			,[PayFrequencyId]
			,[RateAmount]
			,[RateFrequencyId]
			,[ChangeReason]
			--,[AuthorizeById]
			,[ApprovedDate]
			,[docName]
			,[docExtension]
			,[docFile]
			,[WCClassCodeId]
			,[PayScaleId]
			,[UserInformationId]
			,[EmploymentId]
			,[PeriodGrossPay]
			,[YearlyGrossPay]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
		   )
     SELECT  [intID]
			,[dtDateStart]
			,[dtDateEnd]
			
			,(select top(1) EEOCategoryId from EEOCategory where old_ID = intEEOCategory And ClientId = @ClientId) as intEEOCategory
			,(select top(1) PayTypeId from PayType where old_ID = intPayType And ClientId = @ClientId) as intPayType
			,(select top(1) PayFrequencyId from PayFrequency where old_ID = intPayFrequency And ClientId = @ClientId) as intPayFrequency
			,[fltRateAmount]
			,(select top(1) RateFrequencyId from RateFrequency where old_ID = intRateFrequency And ClientId = @ClientId) as intRateFrequency
			,[strChangeReason]
			--,(select top(1) UserInformationId from UserInformation where EmployeeId = strAuthorizeByID And ClientId = @ClientId) as strAuthorizeByID
			,[dtDateApproved]
			,[docName]
			,[docExtension]
			,[docFile]
			,(select top(1) WCClassCodeId from WCClassCode where old_ID = intWCClassCode And ClientId = @ClientId) as intWCClassCode
			,(select top(1) PayScaleId from PayScale where old_ID = intPayScaleID And ClientId = @ClientId) as intPayScaleID
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,IsNull((select top(1) EmploymentId from Employment where dtDateStart Between EffectiveHireDate And Isnull(TerminationDate,Getdate()) And ClientId = @ClientId And Employment.UserInformationId = (select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId)),
			 IsNull((select top(1) EmploymentId from Employment where Employment.UserInformationId = (select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) Order by EffectiveHireDate desc),1))
			,0
			,0
			,@ClientId
			,1
			,GetDate()
			,1
			
     FROM [TimeAideSource].[dbo].[tblPayInfoHistory]
END
