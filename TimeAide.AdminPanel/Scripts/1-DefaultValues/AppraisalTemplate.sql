-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateDefaultAppraisalTemplate
-- =============================================
CREATE PROCEDURE sp_CreateDefaultAppraisalTemplate
	@ClientId int
AS
BEGIN
		INSERT [dbo].[AppraisalTemplate] ([TemplateName], [CreatedBy], [CreatedDate], [DataEntryStatus], [ClientId]) VALUES (N'For Migration Data',1,GetDate(), 1,@ClientId);
END

