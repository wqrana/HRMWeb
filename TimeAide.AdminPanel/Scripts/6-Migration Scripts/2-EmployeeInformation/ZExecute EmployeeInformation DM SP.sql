CREATE PROCEDURE sp_DM_EmployeeInformationSPs
	@ClientId int
AS
BEGIN

	EXEC sp_DM_UserInformation @ClientId
	EXEC sp_DM_UserContactInformation @ClientId
	EXEC sp_DM_EmergencyContact @ClientId
End