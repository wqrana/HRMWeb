-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultRatingLevel
	@ClientId int
AS
BEGIN

	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'0',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'1',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'2',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'3',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'4',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'5',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'6',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'7',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'8',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'9',1,GetDate(), 1,@ClientId);
	INSERT [dbo].[RatingLevel] ( [RatingLevelName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'10',1,GetDate(), 1,@ClientId);

END

