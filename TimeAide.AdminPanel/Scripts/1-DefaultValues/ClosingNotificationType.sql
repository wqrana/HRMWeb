---- =============================================
---- Author:		Salman
---- Create date: 20-Aug-2020
---- Description:	CreateClient
---- =============================================
--CREATE PROCEDURE sp_CreateDefaultClosingNotificationType
--	@ClientId int
--AS
--BEGIN

--	SET IDENTITY_INSERT [dbo].[ClosingNotificationType] ON 

--	INSERT [dbo].[ClosingNotificationType] ([ClosingNotificationTypeId], [ClosingNotificationTypeName], [ClosingNotificationTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Employee', N'Employee', 1, 1, 1, CAST(N'2021-02-05 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
--	INSERT [dbo].[ClosingNotificationType] ([ClosingNotificationTypeId], [ClosingNotificationTypeName], [ClosingNotificationTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Employee , Approvers', N'Employee , Approvers', 1, 1, 1, CAST(N'2021-02-05 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
--	INSERT [dbo].[ClosingNotificationType] ([ClosingNotificationTypeId], [ClosingNotificationTypeName], [ClosingNotificationTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'All', N'All', 1, 1, 1, CAST(N'2021-02-05 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
	
--	SET IDENTITY_INSERT [dbo].[ClosingNotificationType] OFF
	
--END

