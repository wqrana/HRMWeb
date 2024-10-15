-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultGender
	@ClientId int
AS
BEGIN
	INSERT [dbo].[Gender] ([GenderName], [GenderDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) VALUES (N'Male', N'Male', 1, GetDate(), 1, NULL, NULL, @ClientId,1);
	INSERT [dbo].[Gender] ([GenderName], [GenderDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) VALUES (N'Female', N'Female', 1, GetDate(), 1, NULL, NULL, @ClientId,2);
	--INSERT [dbo].[Gender] ([GenderName], [GenderDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) VALUES (N'Other', N'Other', 1, GetDate(), 1, NULL, NULL, @ClientId,3);
END




