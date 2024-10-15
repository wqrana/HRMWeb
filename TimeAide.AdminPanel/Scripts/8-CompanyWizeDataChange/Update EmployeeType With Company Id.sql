-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdateEmployeeTypeWithCompany
	@ClientId int
AS
BEGIN

	INSERT INTO [dbo].[EmployeeType]([EmployeeTypeName],[EmployeeTypeDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id],CompanyId)

	SELECT e.[EmployeeTypeName],[EmployeeTypeDescription],e.[ClientId],e.[CreatedBy],e.[CreatedDate],e.[DataEntryStatus],e.[ModifiedBy],e.[ModifiedDate],e.[Old_Id],C.CompanyId
	from EmployeeType e
	Inner join company c on e.ClientId=c.ClientId
	WHERE e.CLIENTID=@ClientId And NOT EXISTS ( SELECT * FROM [EmployeeType] Dep  WHERE Dep.EmployeeTypeName = e.EmployeeTypeName AND dep.CompanyId = c.CompanyId);


	Update SD
	Set SD.EmployeeTypeId = (Select top 1 dd.EmployeeTypeId from EmployeeType dd where dd.EmployeeTypeName=d.EmployeeTypeName And dd.CompanyId=SD.CompanyId)
	from EmploymentHistory SD
	inner join EmployeeType d on sd.EmployeeTypeId = d.EmployeeTypeId
	Inner join UserInformation u on sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	where u.ClientId=@ClientId --and d.CompanyId is null;
	and (Select top 1 dd.EmployeeTypeId from EmployeeType dd where dd.EmployeeTypeName=d.EmployeeTypeName And dd.CompanyId=SD.CompanyId) is not null
END