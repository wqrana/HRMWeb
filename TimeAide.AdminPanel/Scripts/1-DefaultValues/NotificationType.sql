-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultNotificationType
	@ClientId int
AS
BEGIN

	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Document', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Credential', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Custom Field', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)

	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Probation Period', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Performance', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Training Expiration', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Health Insurance', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Dental Insurance', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Action History', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Benefits', 1, @ClientId, 1, GetDate(), 1, NULL, NULL)
END

