-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultEthnicity
	@ClientId int
AS
BEGIN
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'American Indian or Alaska Native', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Asian', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Black or African American', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Hispanic or Latino', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Native Hawaiian or Other Pacific Islander', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Two or More Races', NULL, 1, GetDate(), 1, @ClientId)
	INSERT [dbo].[Ethnicity] ([EthnicityName], [EthnicityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'White', NULL, 1, GetDate(), 1, @ClientId)
END
