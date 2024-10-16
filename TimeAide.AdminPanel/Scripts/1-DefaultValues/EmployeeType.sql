-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultEmployeeType
	@ClientId int
AS
BEGIN
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'No', N'No', @ClientId, 1, GetDate(), 1, NULL, NULL);
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Full Time', N'Full Time', @ClientId, 1, GetDate(), 1, NULL, NULL);
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Part Time', N'Part Time', @ClientId, 1, GetDate(), 1, NULL, NULL);
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Professional Services', N'Professional Services', @ClientId, 1, GetDate(), 1, NULL, NULL);
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Temp Full Time', N'Temp Full Time', @ClientId, 1, GetDate(), 1, NULL, NULL);
	INSERT [dbo].[EmployeeType] ([EmployeeTypeName], [EmployeeTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'Temp Part Time', N'Temp Part Time', @ClientId, 1, GetDate(), 1, NULL, NULL);
END

