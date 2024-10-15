-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultNotificationType
AS
BEGIN

	SET IDENTITY_INSERT [dbo].[NotificationType] ON 

	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (1,N'Document', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (2,N'Credential', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (3,N'Custom Field', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (4,N'Probation Period', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (5,N'Performance', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (6,N'Training Expiration', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (7,N'Health Insurance', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (8,N'Dental Insurance', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (9,N'Action History', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (10,N'Benefits', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (11,N'Birthday', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (12,N'Work Anniversary', 1, 1, 1, GetDate(), 1, NULL, NULL)
	INSERT [dbo].[NotificationType] ([NotificationTypeId],[NotificationTypeName], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (13,N'Email Blast', 1, 1, 1, GetDate(), 1, NULL, NULL)

	SET IDENTITY_INSERT [dbo].[NotificationType] OFF

END

