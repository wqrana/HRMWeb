---- =============================================
---- Author:		Salman
---- Create date: 20-Aug-2020
---- Description:	CreateClient
---- =============================================
--CREATE PROCEDURE sp_CreateDefaultEmailType
--	@ClientId int
--AS
--BEGIN

--	SET IDENTITY_INSERT [dbo].[EmailType] ON 
--	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Registration', N'Registration', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
--	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'ForgotPassword', N'ForgotPassword', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
--	SET IDENTITY_INSERT [dbo].[EmailType] OFF

--END


