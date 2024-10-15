
--CREATE PROCEDURE sp_CreateDefaultEmployeeGroupType
--	@ClientId int=1
--AS
--BEGIN
--SET IDENTITY_INSERT [dbo].[EmployeeGroupType] ON 
--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (1, N'Employee', N'Employee', 1, 1, GETDATE(), 1)

--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (2, N'Supervisor', N'Supervisor', 1, 1, GETDATE(),1)

--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (3, N'Human Resource', N'Human Resource', 1, 1, GETDATE(),1)

--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (4, N'Management', N'Management', 1, 1, GETDATE(), 1)

--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (5, N'Company Portal Admin', N'Company Portal Admin', 1, 1, GETDATE(), 1)
--SET IDENTITY_INSERT [dbo].[EmployeeGroupType] off 	
--END