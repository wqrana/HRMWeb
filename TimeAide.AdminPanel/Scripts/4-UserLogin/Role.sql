CREATE PROCEDURE sp_CreateDefaultRole
	@ClientId int=1
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE RoleName = 'Super Admin' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Role] ([RoleName], [Description], [RoleTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Super Admin', N'Super Admin', 1, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	END

	IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE RoleName = 'Admin' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Role] ([RoleName], [Description], [RoleTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Admin', N'Admin', 2, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	END

	IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE RoleName = 'Supervisor' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Role] ([RoleName], [Description], [RoleTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Supervisor', N'Supervisor', 3, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	END

	IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE RoleName = 'User Role' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Role] ([RoleName], [Description], [RoleTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'User Role', NULL, 4, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	END

	IF NOT EXISTS (SELECT * FROM [dbo].[Role] WHERE RoleName = 'None' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Role] ([RoleName], [Description], [RoleTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'None', NULL, 5, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
	END
END
