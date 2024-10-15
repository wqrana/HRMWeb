
CREATE PROCEDURE sp_CreateDefaultEmployeeGroupType
AS
BEGIN
SET IDENTITY_INSERT [dbo].[EmployeeGroupType] ON 
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (1, N'Employee', N'Employee', 1, 1, GETDATE(), 1)
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (2, N'Supervisor', N'Supervisor', 1, 1, GETDATE(),1)
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (3, N'Human Resource', N'Human Resource', 1, 1, GETDATE(),1)
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (4, N'Management', N'Management', 1, 1, GETDATE(), 1)
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (5, N'Company Portal Admin', N'Company Portal Admin', 1, 1, GETDATE(), 1)
--INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (6, N'Notification Group', N'Notification Group', 1, 1, GETDATE(), 1) 
INSERT [dbo].[EmployeeGroupType] ([EmployeeGroupTypeId], [EmployeeGroupTypeName], [EmployeeGroupTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (8, N'Company Media Admin','Company Media Admin',1,1,'2022-01-01',1)
SET IDENTITY_INSERT [dbo].[EmployeeGroupType] off 	
END