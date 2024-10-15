--CREATE PROCEDURE sp_CreateDefaultEmployeeGroup
--	@ClientId int=1 
--AS
--BEGIN
--	IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeGroup] WHERE EmployeeGroupName = 'Employee' And [ClientId]=@ClientId)
--	BEGIN
--		INSERT [dbo].[EmployeeGroup] ([EmployeeGroupName], [EmployeeGroupDescription], [EmployeeGroupTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Employee', N'Employee', 1, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
--	END

--	IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeGroup] WHERE EmployeeGroupName = 'Supervisor' And [ClientId]=@ClientId)
--	BEGIN
--		INSERT [dbo].[EmployeeGroup] ([EmployeeGroupName], [EmployeeGroupDescription], [EmployeeGroupTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Supervisor', N'Supervisor', 2, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
--	END

--	IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeGroup] WHERE EmployeeGroupName = 'Human Resource' And [ClientId]=@ClientId)
--	BEGIN
--		INSERT [dbo].[EmployeeGroup] ([EmployeeGroupName], [EmployeeGroupDescription], [EmployeeGroupTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Human Resource', N'Human Resource', 3, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
--	END

--	IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeGroup] WHERE EmployeeGroupName = 'Management' And [ClientId]=@ClientId)
--	BEGIN
--		INSERT [dbo].[EmployeeGroup] ([EmployeeGroupName], [EmployeeGroupDescription], [EmployeeGroupTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Management', N'Management', 4, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
--	END

--	IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeGroup] WHERE EmployeeGroupName = 'Company Portal Admin' And [ClientId]=@ClientId)
--	BEGIN
--		INSERT [dbo].[EmployeeGroup] ([EmployeeGroupName], [EmployeeGroupDescription], [EmployeeGroupTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Company Portal Admin', N'Company Portal Admin', 5, NULL, @ClientId, 1, GetDate(), 1, NULL, NULL);
--	END
--END
