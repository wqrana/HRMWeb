-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflowLevelType
AS
BEGIN
	
	SET IDENTITY_INSERT [dbo].[WorkflowLevelType] ON

	INSERT [dbo].[WorkflowLevelType] ([WorkflowLevelTypeId], [WorkflowLevelTypeName], [WorkflowLevelTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Employee Supervisor', N'Employee Supervisor', 2, 2, 1, CAST(N'2021-01-30 00:28:45.533' AS DateTime), 1, 1, CAST(N'2021-01-30 00:28:45.537' AS DateTime), NULL)
	INSERT [dbo].[WorkflowLevelType] ([WorkflowLevelTypeId], [WorkflowLevelTypeName], [WorkflowLevelTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Employee Group', N'Employee Group', 2, 2, 1, CAST(N'2021-01-30 00:30:25.523' AS DateTime), 1, NULL, NULL, NULL)

	SET IDENTITY_INSERT [dbo].[WorkflowLevelType] OFF

END
