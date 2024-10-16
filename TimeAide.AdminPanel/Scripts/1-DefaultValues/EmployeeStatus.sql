-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultEmployeeStatus
	@ClientId int
AS
BEGIN
	SET IDENTITY_INSERT [dbo].[EmployeeStatus] ON 
		INSERT [dbo].[EmployeeStatus] ([EmployeeStatusId], [EmployeeStatusName], [EmployeeStatusDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Active', NULL, @ClientId, 1, GetDate(), 1, NULL, NULL, 1);
		INSERT [dbo].[EmployeeStatus] ([EmployeeStatusId], [EmployeeStatusName], [EmployeeStatusDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Inactive', NULL, @ClientId, 1, GetDate(), 1, NULL, NULL, 2);
		INSERT [dbo].[EmployeeStatus] ([EmployeeStatusId], [EmployeeStatusName], [EmployeeStatusDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Closed', NULL, @ClientId, 1, GetDate(), 1, NULL, NULL, 3);
	SET IDENTITY_INSERT [dbo].[EmployeeStatus] OFF

END

