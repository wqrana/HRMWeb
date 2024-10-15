CREATE PROCEDURE [dbo].[sp_UserInformationById] 
	-- Add the parameters for the stored procedure here
	@userId int

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

	Select distinct *
	From vw_UserInformation
	Where (Id = @userId
	And DataEntryStatus = 1)
END
