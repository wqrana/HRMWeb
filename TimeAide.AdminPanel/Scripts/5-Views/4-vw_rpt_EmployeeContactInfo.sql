CREATE VIEW [dbo].[vw_rpt_EmployeeContactInfo]
AS
SELECT 
  UserInformationId
, UserContactInformationId
, HomeNumber
, CelNumber
, FaxNumber
, OtherNumber
, WorkEmail
, PersonalEmail
, OtherEmail
, WorkNumber
, WorkExtension
, HomeAddress1
, HomeAddress2
, HomeCityId
, homeCity.CityName as   HomeCityName
, HomeStateId
, homeState.StateName as HomeStateName
, HomeCountryId
, HomeZipCode
, MailingAddress1
, MailingAddress2
, MailingCityId
, mailingCity.CityName as  MailingCityName
, MailingStateId
, mailingState.StateName as MailingStateName
, MailingZipCode
FROM UserContactInformation uif
LEFT JOIN City homeCity ON homeCity.CityId = uif.HomeCityId
LEFT JOIN State homeState ON homeState.StateId = uif.HomeStateId
LEFT JOIN City mailingCity ON mailingCity.CityId = uif.MailingCityId
LEFT JOIN State mailingState ON mailingState.StateId = uif.MailingStateId
WHERE uif.DataEntryStatus = 1

