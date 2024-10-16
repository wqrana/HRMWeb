-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultDisability
	@ClientId int,
	@IsNewClient int
AS
BEGIN

	IF @IsNewClient=0 
		BEGIN
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Vision ', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Hearing ', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Psychological ', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Other', N'Other', 1, GetDate(), 1, NULL, NULL, @ClientId)
		ENd
	ELSE
		BEGIN
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Yes', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
			INSERT [dbo].[Disability] ([DisabilityName], [DisabilityDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'No', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId)
		END
END

