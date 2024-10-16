-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateEmptySourceDefaultTerminationEligibility
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[TerminationEligibility] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT INTO [dbo].[TerminationEligibility] ([TerminationEligibilityName],[TerminationEligibilityDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		Values   ('Yes','Yes',@ClientId,1,GetDate(),1)

		INSERT INTO [dbo].[TerminationEligibility] ([TerminationEligibilityName],[TerminationEligibilityDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		Values   ('No','No',@ClientId,1,GetDate(),1)
	END
END




