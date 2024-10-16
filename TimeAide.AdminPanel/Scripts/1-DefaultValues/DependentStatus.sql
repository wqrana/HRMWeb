-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultDependentStatus
	@ClientId int
AS
BEGIN
		INSERT [dbo].[DependentStatus] ([StatusName], [Description], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'Direct', N'Direct', 1, GETDATE(), 1,@ClientId)
		INSERT [dbo].[DependentStatus] ([StatusName], [Description], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'InDirect', N'InDirect', 1, GETDATE(), 1,@ClientId)
END

