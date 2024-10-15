-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultEmploymentStatus
	@ClientId int
AS
BEGIN

	INSERT [dbo].[EmploymentStatus] ([EmploymentStatusName], [EmploymentStatusDescription], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'NONE', N'NONE', @ClientId, 1, GetDate(), 1, NULL, NULL);
	
END

