-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultSenderEmailConfiguration
	@ClientId int
AS
BEGIN
	SET IDENTITY_INSERT [dbo].[SenderEmailConfiguration] ON 
	INSERT [dbo].[SenderEmailConfiguration] ([SenderEmailConfigurationId], [MailProvider], [HostName], [ProviderAccount], [SenderName], [Environment], [Port], [EnableSsl], [Password], [FromEmail], [SampleEmail], [UseFixedForm], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (1, N'sendinblue', N'smtp-relay.sendinblue.com', N'alexrt@identechinc.com', N'Identech', N'All', 587, 1, N'LHBZ02TksQJEcrS1', N'identech@cloudaide.net', N'This is sample email', 1, NULL, 1, 1, GETDATE(), 1)
	SET IDENTITY_INSERT [dbo].[SenderEmailConfiguration] OFF
END




