-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflowLevel
	@ClientId int
AS
BEGIN
	
	SET IDENTITY_INSERT [dbo].[WorkflowLevel] ON 

	INSERT [dbo].[WorkflowLevel] ([WorkflowLevelId], [WorkflowLevelName], [WorkflowId], [WorkflowLevelTypeId], [NotificationMessageId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Address change approval workflow - Employee Supervisor', 1, 1, 1, 2, @ClientId, 1, CAST(N'2021-03-08 14:43:18.700' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowLevel] ([WorkflowLevelId], [WorkflowLevelName], [WorkflowId], [WorkflowLevelTypeId], [NotificationMessageId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Address change approval workflow - HR', 1, 2, 2, 2, @ClientId, 1, CAST(N'2021-03-08 14:44:54.207' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowLevel] ([WorkflowLevelId], [WorkflowLevelName], [WorkflowId], [WorkflowLevelTypeId], [NotificationMessageId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Address change approval workflow - Admin', 1, 2, 3, 2, @ClientId, 1, CAST(N'2021-03-08 14:45:16.017' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowLevel] ([WorkflowLevelId], [WorkflowLevelName], [WorkflowId], [WorkflowLevelTypeId], [NotificationMessageId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Supervisor Approval', 3, 1, 6, 13, @ClientId, 1, CAST(N'2021-04-13 14:25:36.957' AS DateTime), 1, NULL, NULL, NULL)

	SET IDENTITY_INSERT [dbo].[WorkflowLevel] OFF

END
