-- =============================================
-- Author:		Salman
-- Create date: 25-Oct-2022
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultDocumentRequiredBy
AS
BEGIN

SET IDENTITY_INSERT [dbo].[DocumentRequiredBy] ON 
	INSERT [dbo].[DocumentRequiredBy] ([DocumentRequiredById], [DocumentRequiredByName], [DocumentRequiredByDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Employee', N'Employee', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[DocumentRequiredBy] ([DocumentRequiredById], [DocumentRequiredByName], [DocumentRequiredByDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Applicant', N'Applicant', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
	INSERT [dbo].[DocumentRequiredBy] ([DocumentRequiredById], [DocumentRequiredByName], [DocumentRequiredByDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Both', N'Both', 1, 1, GETDATE(), 1, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[DocumentRequiredBy] OFF

END

