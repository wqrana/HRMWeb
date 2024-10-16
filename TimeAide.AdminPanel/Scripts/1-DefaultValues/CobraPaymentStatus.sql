-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultCobraPaymentStatus
	@ClientId int
AS
BEGIN
		INSERT [dbo].[CobraPaymentStatus] ([CobraPaymentStatusName], [Description], [DataEntryStatus], [CreatedBy], [CreatedDate], [ClientId]) VALUES (N'Pending', NULL, 1, 1, GETDATE(), @ClientId);
		INSERT [dbo].[CobraPaymentStatus] ([CobraPaymentStatusName], [Description], [DataEntryStatus], [CreatedBy], [CreatedDate], [ClientId]) VALUES (N'Paid', NULL, 1, 1, GETDATE(), @ClientId);
END

