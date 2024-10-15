
CREATE PROCEDURE sp_CreateDefaultModule
AS
BEGIN

SET IDENTITY_INSERT [dbo].[Module] ON 

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'UserManagment', N'Employees', N'Employees', 1, NULL, 1, 1, CAST(N'2019-09-09 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Settings', N'Settings', N'Settings', 1, NULL, 1, 1, CAST(N'2019-09-09 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Employment', N'Employment', N'Employment', 1, 2, 1, 1, CAST(N'2019-09-22 02:32:24.857' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'General', N'General', N'General', 1, 2, 1, 1, CAST(N'2019-09-22 02:32:24.857' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'HumanResources', N'Human Resources', N' Human Resources ', 1, 2, 1, 1, CAST(N'2020-07-22 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (6, N'ApplicationAccess', N'Application Access', N'Application Access', 1, 2, 1, 1, CAST(N'2020-07-22 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (7, N'Notifications', N'Notifications', N'Notifications', 1, 2, 1, 1, CAST(N'2020-07-22 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (8, N'HiringTermination', N'Hiring and Termination', N'Hiring and Termination', 1, 5, 1, 1, CAST(N'2020-07-21 12:19:46.030' AS DateTime), 1, 1, CAST(N'2020-07-21 12:19:46.030' AS DateTime), NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (9, N'PayInfo', N'PayInfo', N'PayInfo', 1, 5, 1, 1, CAST(N'2020-07-21 12:54:03.830' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (10, N'Education', N'Education', N'Education', 1, 5, 1, 1, CAST(N'2020-07-21 12:54:30.253' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (11, N'Training', N'Training', N'Training', 1, 5, 1, 1, CAST(N'2020-07-21 12:55:07.730' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (12, N'Performance Review', N'Performance Review', N'Performance Review', 1, 5, 1, 1, CAST(N'2020-07-21 12:55:27.967' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (13, N'Documents', N'Documents', N'Documents', 1, 5, 1, 1, CAST(N'2020-07-21 12:55:51.080' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (14, N'Credentials', N'Credentials', N'Credentials', 1, 5, 1, 1, CAST(N'2020-07-21 12:56:16.513' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (15, N'Custom Fields', N'Custom Fields', N'Custom Fields', 1, 5, 1, 1, CAST(N'2020-07-21 12:56:40.947' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (16, N'Benefits', N'Benefits', N'Benefits', 1, 5, 1, 1, CAST(N'2020-07-21 12:57:00.703' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (17, N'Action History', N'Action History', N'Action History', 1, 5, 1, 1, CAST(N'2020-07-21 12:57:16.623' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (18, N'Appraisals', N'Appraisals', N'Appraisals', 1, 5, 1, 1, CAST(N'2020-07-21 12:57:30.710' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (19, N'Incidents', N'Incidents', N'Incidents', 1, 5, 1, 1, CAST(N'2020-07-21 12:57:45.307' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (20, N'Reports', N'Reports', N'Reports', 1, NULL, 1, 1, CAST(N'2020-07-21 12:57:45.307' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (21, N'Workflow', N'Workflow', N'Workflow', 1, NULL, 1, 1, CAST(N'2020-07-21 12:57:45.307' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (22, N'Employee General', N'Employee General', N'Employee General', 1, 20, 1, 1, CAST(N'2021-03-11 18:43:53.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (23, N'Hiring Info.', N'Hiring Info.', N'Hiring Info.', 1, 20, 1, 1, CAST(N'2021-03-11 18:43:53.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (24, N'Employment Info.', N'Employment Info.', N'Employment Info.', 1, 20, 1, 1, CAST(N'2021-03-11 18:43:53.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (25, N'Pay Info.', N'Pay Info.', N'Pay Info.', 1, 20, 1, 1, CAST(N'2021-03-11 18:43:53.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (26, N'ApplicantManagement', N'Applicant Management', N'Applicants', 1, NULL, 1, 1, CAST(N'2021-02-15 23:50:17.633' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (27, N'Applicant', N'Applicants', N'Applicant', 1, 5, 1, 1, CAST(N'2021-02-15 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (28, N'OSHA Reports', N'OSHA Reports', N'OSHA Reports', 1, 20, 1, 1, CAST(N'2021-05-05 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (29, N'Attendance', N'Attendance', N'Attendance', 1, NULL, 1, 1, CAST(N'2021-06-10 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (30, N'App', N'App', N'App', 1, NULL, 1, 1, CAST(N'2021-06-10 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (31, N'Chat', N'Chat', N'Chat', NULL, 30, 1, 1, CAST(N'2021-06-10 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (32, N'ChangeRequest', N'ChangeRequest', N'Change Request', 1, NULL, 1, 1, CAST(N'2020-07-21 12:57:45.307' AS DateTime), 1, NULL, NULL, NULL)

INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (33, N'LicensesAndAccruals', N'Licenses And Accruals', N'Accruals', 1, NULL, 1, 1, '2020-01-01 00:00:00.000', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (34, N'Payroll', N'Payroll', N'Payroll', 1, NULL, 1, 1, '2022-02-16 10:39:55.393', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (35, N'Transaction', N'Transaction', N'Transaction', 1, NULL, 1, 1, '2022-02-18 10:07:06.283', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (36, N'SelfServiceModule', N'SelfServiceModule', N'SelfServiceModule', 1, NULL, 1, 1, GETDATE(), 1, NULL, NULL, NULL)

INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (37, N'Accruals', N'Accruals', N'Accruals', 1, NULL, 1, 1, '2022-04-18 11:24:48.360', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (38, N'AttendanceWeb', N'AttendanceWeb', N'AttendanceWeb', 1, NULL, 1, 1, '2022-04-18 11:24:48.360', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (39, N'Audit', N'Audit', N'Audit', 1, NULL, 1, 1, '2022-04-18 11:24:48.360', 1, NULL, NULL, NULL)
INSERT INTO [dbo].[Module] ([ModuleId], [Code], [ModuleName], [ModuleLabel], [CompanyId], [ParentModuleId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (42, N'TA-Window', N'TA-Window', N'TA-Window', 1, NULL, 1, 1, '2022-04-18 11:24:48.360', 1, NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[Module] OFF
END