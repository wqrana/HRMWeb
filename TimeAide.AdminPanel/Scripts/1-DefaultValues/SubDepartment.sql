-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultSubDepartment
	@ClientId int
AS
BEGIN

	INSERT [dbo].[SubDepartment] ([SubDepartmentName], [SubDepartmentDescription], [USECFSEAssignment], [CFSECodeId], [CFSECompanyPercent], [DepartmentId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate]) VALUES (N'NONE', N'NONE', 0, 1, CAST(0.00 AS Decimal(18, 2)), 1, @ClientId, 1, GetDate(), 1, NULL, NULL);

END


