-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultCountry 
	@ClientId int
AS
BEGIN
	INSERT [dbo].[Country] ([CountryCode], [CountryName], [CountryDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (N'PR', N'Puerto Rico', NULL, @ClientId, 1, CAST(N'2020-08-26 19:16:44.723' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[Country] ([CountryCode], [CountryName], [CountryDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (N'US', N'United States', NULL, @ClientId, 1, CAST(N'2020-08-26 19:16:44.727' AS DateTime), 1, NULL, NULL, NULL)
END

