--CREATE PROCEDURE sp_CreateDefaultEmployeeGroupType
--	@ClientId int
--AS
--BEGIN
--		SET IDENTITY_INSERT [dbo].[EmployeeGroupType] ON 
		
--		INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Employee', N'Employee', 1, 1, GetDate(), 1, NULL, NULL, NULL);
--		INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Supervisor', N'Supervisor', 1, 1, GetDate(), 1, NULL, NULL, NULL);
--		INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Human Resource', N'Human Resource', 1, 1, GetDate(), 1, NULL, NULL, NULL);
--		INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Management', N'Management', 1, 1, GetDate(), 1, NULL, NULL, NULL);
--		INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'Company Portal Admin ', N'Company Portal Admin ', 1, 1, GetDate(), 1, NULL, NULL, NULL);
		
--		SET IDENTITY_INSERT [dbo].[EmployeeGroupType] off 	
--END