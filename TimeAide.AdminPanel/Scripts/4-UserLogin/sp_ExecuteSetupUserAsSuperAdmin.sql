CREATE PROCEDURE sp_ExecuteSetupUserAsSuperAdmin
	@ClientId int
AS
BEGIN
		EXEC [dbo].[sp_SetupUserAsSuperAdmin]	@UserInformationId = 1,	@ClientId = 1, @CompanyId = 1
END