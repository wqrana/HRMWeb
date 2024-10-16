-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultEmailType
AS
BEGIN

	SET IDENTITY_INSERT [dbo].[EmailType] ON 
	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Registration', N'Registration', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'ForgotPassword', N'ForgotPassword', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Manual Registration', N'Manual Registration', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Auto Resend Registration', N'Auto Resend Registration', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[EmailType] ([EmailTypeId], [EmailTypeName], [EmailTypeDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'Email Blast', N'Email Blast', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	SET IDENTITY_INSERT [dbo].[EmailType] OFF

END


