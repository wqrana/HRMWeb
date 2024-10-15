CREATE PROCEDURE sp_CreateDefaultPrivilege
AS
BEGIN


SET IDENTITY_INSERT [dbo].[Privilege] ON 

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Code], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Add', N'Add', NULL, 1, 1, CAST(N'2019-08-29 20:34:31.280' AS DateTime), 1, NULL, NULL)

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Code], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Edit', N'Edit', NULL, 1, 1, CAST(N'2019-08-29 20:34:37.867' AS DateTime), 1, NULL, NULL)

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Code], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Delete', N'Delete', NULL, 1, 1, CAST(N'2019-08-29 20:34:43.323' AS DateTime), 1, NULL, NULL)

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Code], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (4, N'View', N'View', NULL, 1, 1, CAST(N'2019-08-29 20:34:52.650' AS DateTime), 1, NULL, NULL)

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Code], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (5, N'Change History', N'Change History', NULL, 1, 1, CAST(N'2019-08-29 20:34:52.650' AS DateTime), 1, NULL, NULL)

SET IDENTITY_INSERT [dbo].[Privilege] OFF

END