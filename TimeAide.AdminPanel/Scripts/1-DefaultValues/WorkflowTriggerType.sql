---- =============================================
---- Author:		Salman
---- Create date: 20-Aug-2020
---- Description:	CreateClient
---- =============================================
--CREATE PROCEDURE sp_CreateDefaultWorkflowTriggerType
--	@ClientId int
--AS
--BEGIN

--	SET IDENTITY_INSERT [dbo].[WorkflowTriggerType] ON 
	
--	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Personal Information', N'Personal Information', NULL, 1, 1, CAST(N'2021-02-09 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
--	INSERT [dbo].[WorkflowTriggerType] ([WorkflowTriggerTypeId], [WorkflowTriggerTypeName], [WorkflowTriggerTypeDescription], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Employment Information', N'Employment Information', NULL, 1, 1, CAST(N'2021-02-09 00:00:00.000' AS DateTime), 1, NULL, NULL, NULL)
	
--	SET IDENTITY_INSERT [dbo].[WorkflowTriggerType] OFF

--END

