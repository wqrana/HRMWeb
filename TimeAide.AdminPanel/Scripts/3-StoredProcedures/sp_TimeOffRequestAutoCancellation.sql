CREATE PROCEDURE [dbo].[sp_TimeOffRequestAutoCancellation] 
	-- Add the parameters for the stored procedure here
	@clientId INT,
	@executionDate Datetime	
AS
BEGIN
	DECLARE @autoCancelDays INT = 3
	Declare @cancellationDate datetime
	Select top 1  @autoCancelDays= IsNull(Convert(int,[ApplicationConfigurationValue]),2)
	From ApplicationConfiguration
	Where [ApplicationConfigurationName] = 'TimeOffAutoCancelDays'
	And ClientId = @clientId

	Select @cancellationDate= DATEADD(DAY, @autoCancelDays, @executionDate)

	Select 
	EmployeeTimeOffRequestId,
	ClientId,
	'Auto-Cancel' as ProcessType
	From EmployeeTimeOffRequest
	Where DataEntryStatus = 1
	And ChangeRequestStatusId = 1 -- in progress
	And ClientId = @clientId
	And StartDate<= @cancellationDate
	Union All
	Select 
	EmployeeTimeOffRequestId,
	ClientId,
	'Reminder' as ProcessType
	From EmployeeTimeOffRequest
	Where DataEntryStatus = 1
	And ChangeRequestStatusId = 1 -- in progress
	And ClientId = @clientId
	And StartDate> @cancellationDate
END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[sp_CRAutoCancellation]'
GO
CREATE PROCEDURE [dbo].[sp_CRAutoCancellation] 
	-- Add the parameters for the stored procedure here
	@clientId INT,
	@executionDate Datetime	
AS
BEGIN
	DECLARE @autoCancelDays INT = 3
	Declare @cancellationDate datetime
	Select top 1  @autoCancelDays= IsNull(Convert(int,[ApplicationConfigurationValue]),2)
	From ApplicationConfiguration
	Where [ApplicationConfigurationName] = 'CRAutoCancelAfterDays'
	And ClientId = @clientId

	--Select @cancellationDate= DATEADD(DAY, @autoCancelDays, @executionDate)
	select * 
	From(
		--CRAddress
		Select *
		 From(
				Select 
				CRAddress.ChangeRequestAddressId as ReferenceId,
				ClientId,
				'CRAddress' as RequestType,
				'Auto-Cancel' as ProcessType	
				From [dbo].[ChangeRequestAddress] CRAddress
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)<= @executionDate
				Union 
				Select 
				CRAddress.ChangeRequestAddressId as ReferenceId,
				ClientId,
				'CRAddress' as RequestType,
				'Reminder' as ProcessType	
				From [dbo].[ChangeRequestAddress] CRAddress
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)> @executionDate
			) As CRAddress
		 Union 
		 --CREmailNumbers
		 Select *
		 From(
				Select 
				CREmailNumbers.ChangeRequestEmailNumbersId as ReferenceId,
				ClientId,
				'CREmailNumbers' as RequestType,
				'Auto-Cancel' as ProcessType	
				From [dbo].[ChangeRequestEmailNumbers] CREmailNumbers
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)<= @executionDate
				Union 
				Select 
				CREmailNumbers.ChangeRequestEmailNumbersId as ReferenceId,
				ClientId,
				'CREmailNumbers' as RequestType,
				'Reminder' as ProcessType	
				From [dbo].[ChangeRequestEmailNumbers] CREmailNumbers
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)> @executionDate
			) As CREmailNumbers
		 Union 
		 --CREmergencyContact
		 Select *
		 From(
				Select 
				CREmergencyContact.ChangeRequestEmergencyContactId as ReferenceId,
				ClientId,
				'CREmergencyContact' as RequestType,
				'Auto-Cancel' as ProcessType	
				From [dbo].[ChangeRequestEmergencyContact] CREmergencyContact
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)<= @executionDate
				Union 
				Select 
				CREmergencyContact.ChangeRequestEmergencyContactId as ReferenceId,
				ClientId,
				'CREmergencyContact' as RequestType,
				'Auto-Cancel' as ProcessType	
				From [dbo].[ChangeRequestEmergencyContact] CREmergencyContact
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)> @executionDate
			) As CREmergencyContact
			Union
			--CREmployeeDependent
			Select *
			From(
				Select 
				CREmployeeDependent.ChangeRequestEmployeeDependentId as ReferenceId,
				ClientId,
				'CREmployeeDependent' as RequestType,
				'Auto-Cancel' as ProcessType	
				From [dbo].[ChangeRequestEmployeeDependent] CREmployeeDependent
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)<= @executionDate
				Union 
				Select 
				CREmployeeDependent.ChangeRequestEmployeeDependentId as ReferenceId,
				ClientId,
				'CREmployeeDependent' as RequestType,
				'Reminder' as ProcessType	
				From [dbo].[ChangeRequestEmployeeDependent] CREmployeeDependent
				Where DataEntryStatus = 1
				And ChangeRequestStatusId = 1 -- in progress
				And ClientId = @clientId
				And DATEADD(DAY, @autoCancelDays, CreatedDate)> @executionDate
			) As CREmployeeDependent
		) CRData
		Order by ClientId,RequestType
END
GO