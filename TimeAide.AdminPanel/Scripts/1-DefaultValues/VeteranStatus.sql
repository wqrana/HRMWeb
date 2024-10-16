-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultVeteranStatus
	@ClientId int
AS
BEGIN
	INSERT [dbo].[VeteranStatus] ([VeteranStatusName], [VeteranStatusDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Armed Forces Service Medal Veteran', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId);
	INSERT [dbo].[VeteranStatus] ([VeteranStatusName], [VeteranStatusDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Other Protected Veteran', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId);
	INSERT [dbo].[VeteranStatus] ([VeteranStatusName], [VeteranStatusDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Special Disabled Veteran', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId);
	INSERT [dbo].[VeteranStatus] ([VeteranStatusName], [VeteranStatusDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Recently Seperated Veteran', NULL, 1, GetDate(), 1, NULL, NULL, @ClientId);
	INSERT [dbo].[VeteranStatus] ([VeteranStatusName], [VeteranStatusDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId]) VALUES (N'Vietnam Era Veteran', N'Vietnam Era Veteran', 1, GetDate(), 1, null, null, @ClientId);
END

