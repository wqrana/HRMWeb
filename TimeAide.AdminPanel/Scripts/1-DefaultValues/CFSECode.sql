-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultCFSECode
	@ClientId int
AS
BEGIN
		INSERT [dbo].[CFSECode] ([CFSECodeName], [CFSECodeDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'None', N'None', 1, GetDate(), 1, NULL, NULL, @ClientId);
END




