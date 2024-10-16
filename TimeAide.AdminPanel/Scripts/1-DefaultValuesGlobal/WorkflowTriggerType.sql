-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultWorkflowTriggerType
AS
BEGIN

	SET IDENTITY_INSERT [dbo].[WorkflowTriggerType] ON 
	
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Personal Information', N'Personal Information', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Employment Information', N'Employment Information', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Numbers', N'Numbers', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Emergency contact', N'Emergency contact', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'Employee Dependent', N'Employee Dependent', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (6, N'Address', N'Address', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (7, N'Time Off Request', N'Time Off Request', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (8, N'Document Upload', N'Document Upload', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (9, N'Punch Clock', N'Punch Clock', NULL, 1, 1, GetDate(), 1, NULL, NULL, NULL)
	SET IDENTITY_INSERT [dbo].[WorkflowTriggerType] OFF

END

