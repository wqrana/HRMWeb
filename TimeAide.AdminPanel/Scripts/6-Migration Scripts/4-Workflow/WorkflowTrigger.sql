-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflowTrigger
@ClientId int
AS
BEGIN
	
	SET IDENTITY_INSERT [dbo].[WorkflowTrigger] ON 

	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Personal Info Change Request', NULL, 1, 1, 2, @ClientId, 1, CAST(N'2021-03-08 14:55:18.200' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Change Request Contact Person', NULL, 1, 4, 2, @ClientId, 1, CAST(N'2021-03-26 16:44:58.400' AS DateTime), 1, 1, CAST(N'2021-03-26 16:44:58.400' AS DateTime), NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Change Request Address', NULL, 1, 6, 2, @ClientId, 1, CAST(N'2021-03-29 17:52:59.413' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Change Request Numbers', NULL, 1, 3, 2, @ClientId, 1, CAST(N'2021-03-29 17:53:20.943' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'Change Request Employee Dependant', NULL, 2, 5, 2, @ClientId, 1, CAST(N'2021-03-29 17:53:57.773' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (6, N'Emergency Contact', NULL, 2, 4, 13, @ClientId, 1, CAST(N'2021-04-09 14:31:34.357' AS DateTime), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTrigger] ([WorkflowTriggerId], [WorkflowTriggerName], [WorkflowTriggerDescription], [WorkflowId], [WorkflowTriggerTypeId], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (7, N'Employee Document', NULL, 1, 7, 2, @ClientId, 1, CAST(N'2021-04-09 14:38:10.670' AS DateTime), 1, NULL, NULL, NULL)
	
	SET IDENTITY_INSERT [dbo].[WorkflowTrigger] OFF


END
