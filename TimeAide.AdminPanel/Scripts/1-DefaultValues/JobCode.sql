-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultJobCode
	@ClientId int
AS
BEGIN

	INSERT [dbo].[JobCode] ([JobCodeName], [JobCodeDescription], [Enabled], [ProjectId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'NONE', N'NONE', 0, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	
END

