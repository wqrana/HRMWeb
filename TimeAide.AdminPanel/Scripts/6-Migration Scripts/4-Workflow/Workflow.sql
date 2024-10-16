-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflow
	@ClientId int
AS
BEGIN
	
	SET IDENTITY_INSERT [dbo].[Workflow] ON

	INSERT [dbo].[Workflow] ([WorkflowId], [WorkflowName], [WorkflowDescription], [ClosingNotificationId], [ClosingNotificationMessageId],  [IsZeroLevel], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Change Request Workflow', N'Change Request Workflow', 3, 4,  0, 2, @ClientId, 1, CAST(N'2021-03-08 14:35:44.277' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[Workflow] ([WorkflowId], [WorkflowName], [WorkflowDescription], [ClosingNotificationId], [ClosingNotificationMessageId],  [IsZeroLevel], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'No Approval Needed', N'The employee can change the information directly.', 3, 5,  1, 13, @ClientId, 1, CAST(N'2021-04-09 14:30:58.097' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[Workflow] ([WorkflowId], [WorkflowName], [WorkflowDescription], [ClosingNotificationId], [ClosingNotificationMessageId],  [IsZeroLevel], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Supervisor Approval', N'Approval by immediate supervisor needed', 2, 6,  0, 13, @ClientId, 1, CAST(N'2021-04-09 14:42:37.223' AS DateTime), 1, NULL, NULL, NULL)

	SET IDENTITY_INSERT [dbo].[Workflow] OFF


END
