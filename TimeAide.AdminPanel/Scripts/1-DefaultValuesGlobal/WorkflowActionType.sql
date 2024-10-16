-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflowActionType
AS
BEGIN
	
SET IDENTITY_INSERT [dbo].[WorkflowActionType] ON 
INSERT [dbo].[WorkflowActionType] ([WorkflowActionTypeId], [WorkflowActionTypeName], [WorkflowActionTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Pending', N'Pending', 1, 1, 1, CAST(N'2021-02-11 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
INSERT [dbo].[WorkflowActionType] ([WorkflowActionTypeId], [WorkflowActionTypeName], [WorkflowActionTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Approved', N'Approved', 1, 1, 1, CAST(N'2021-02-11 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
INSERT [dbo].[WorkflowActionType] ([WorkflowActionTypeId], [WorkflowActionTypeName], [WorkflowActionTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Declined', N'Declined', 1, 1, 1, CAST(N'2021-02-11 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
INSERT [dbo].[WorkflowActionType] ([WorkflowActionTypeId], [WorkflowActionTypeName], [WorkflowActionTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Cancelled', N'Cancelled', 1, 1, 1, CAST(N'2021-02-11 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
INSERT [dbo].[WorkflowActionType] ([WorkflowActionTypeId], [WorkflowActionTypeName], [WorkflowActionTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (6, N'Closed', N'Closed', 1, 1, 1, '2022-09-16 18:48:34.813', 1, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[WorkflowActionType] OFF

END




