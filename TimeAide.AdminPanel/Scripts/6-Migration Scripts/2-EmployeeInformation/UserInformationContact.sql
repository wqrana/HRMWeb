CREATE PROCEDURE sp_DM_UserContactInformation
	@ClientId int
AS
BEGIN

INSERT INTO [dbo].[UserContactInformation] (
			Old_Id	,
			UserInformationId	,
			HomeAddress1	,
			HomeAddress2	,
			HomeCityId	,
			HomeStateId	,
			HomeCountryId	,
			HomeZipCode	,
			MailingAddress1	,
			MailingAddress2	,
			MailingCityId	,
			MailingStateId	,
			MailingCountryId	,
			MailingZipCode	,
			HomeNumber	,
			CelNumber	,
			FaxNumber	,
			OtherNumber	,
			WorkEmail	,
			PersonalEmail	,
			OtherEmail	,
			WorkNumber	,
			WorkExtension	,
			IsSameHomeAddress	,
			ClientId	,
			CreatedBy	,
			CreatedDate	,
			DataEntryStatus	
			)
SELECT 
			intID	,
			(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID) as strEmpID,
			strHomeAddress1	,
			strHomeAddress2	,
			IsNull((select top(1) CityId from City where CityName = strHomeCity And ClientId = @ClientId),(select top(1) CityId from City where ClientId = @ClientId)) as strHomeCity,
			IsNull((select top(1) StateId from City where CityName = strHomeCity And ClientId = @ClientId),(select top(1) StateId from [State] where ClientId = @ClientId)) as strHomeState	,
			IsNull((select top(1) CountryId from [State] where StateId = (select top(1) StateId from City where CityName = strHomeCity And ClientId = @ClientId)),(select top(1) CountryId from [Country] where ClientId = @ClientId)) as CountryId,
			NULLIF(LTRIM(RTRIM(strHomeZipCode)),''),
			strMailingAddress1	,
			strMailingAddress2	,
			IsNull((select top(1) CityId from City where CityName = strMailingCity And ClientId = @ClientId),(select top(1) CityId from City where ClientId = @ClientId)) as strMailingCity,
			IsNull((select top(1) StateId from City where CityName = strMailingCity And ClientId = @ClientId),(select top(1) StateId from [State] where ClientId = @ClientId)) as strMailingState	,
			IsNull((select top(1) CountryId from [State] where StateId = (select top(1) StateId from City where CityName = strMailingCity And ClientId = @ClientId)),(select top(1) CountryId from [Country] where ClientId = @ClientId)) as CountryId,
			NULLIF(LTRIM(RTRIM(strMailingZipCode)),''),
			NULLIF(LTRIM(RTRIM(strHomeNumber)),''),
			NULLIF(LTRIM(RTRIM(strCelNumber)),''),
			NULLIF(LTRIM(RTRIM(strFaxNumber)),''),
			NULLIF(LTRIM(RTRIM(strOtherNumber)),''),
			NULLIF(LTRIM(RTRIM(strWorkEmail)),''),
			NULLIF(LTRIM(RTRIM(strPersonalEmail)),''),
			NULLIF(LTRIM(RTRIM(strOtherEmail)),''),
			NULLIF(LTRIM(RTRIM(strWorkNumber)),''),
			NULLIF(LTRIM(RTRIM(strWorkExtension)),''),
			0,
			@ClientId As ClientId	,
			1 As CreatedBy	,
			GetDate() CreayedDate	,
			1 as DataEntrtyStatus
     FROM [TimeAideSource].[dbo].[tblEmployeeContact] 
	 
END
