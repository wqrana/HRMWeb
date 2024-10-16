-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateEmptySourceDefaultDisability
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[Disability] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) 
		VALUES (N'Yes', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)

		INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) 
		VALUES (N'No', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
	END
END

