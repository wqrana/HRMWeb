-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdateEmploymentTypeWithCompany
	@ClientId int
AS
BEGIN

	INSERT INTO [dbo].[EmploymentType]([EmploymentTypeName],[EmploymentTypeDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id],CompanyId)

	SELECT e.[EmploymentTypeName],[EmploymentTypeDescription],e.[ClientId],e.[CreatedBy],e.[CreatedDate],e.[DataEntryStatus],e.[ModifiedBy],e.[ModifiedDate],e.[Old_Id],c.CompanyId
	from EmploymentType e
	Inner join company c on e.ClientId=c.ClientId
	WHERE e.CLIENTID= @ClientId And NOT EXISTS ( SELECT * FROM [EmploymentType] Dep  WHERE Dep.EmploymentTypeName = e.EmploymentTypeName AND dep.CompanyId = c.CompanyId);


	Update SD
	Set SD.EmploymentTypeId = (Select top 1 dd.EmploymentTypeId from EmploymentType dd where dd.EmploymentTypeName=d.EmploymentTypeName And dd.CompanyId=SD.CompanyId)
	from EmploymentHistory SD
	inner join EmploymentType d on sd.EmploymentTypeId = d.EmploymentTypeId
	Inner join UserInformation u on sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	where u.ClientId=@ClientId and d.CompanyId is null;
END