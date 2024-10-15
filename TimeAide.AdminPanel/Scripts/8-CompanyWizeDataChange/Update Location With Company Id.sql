-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdateLocationWithCompany
	@ClientId int
AS
BEGIN
IF OBJECT_ID(N'tempdb..#Location') IS NOT NULL
	BEGIN
		DROP TABLE #Location
	END

SELECT DISTINCT CompanyId,LocationId,LocationName,ClientId into #Location
FROM
(
	SELECT DISTINCT d.LocationId,LocationName,h.CompanyId,d.ClientId 
	FROM Location d
	INNER JOIN EmploymentHistory h ON d.LocationId=h.LocationId
	INNER JOIN UserInformation u ON h.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	WHERE d.ClientId=@ClientId
	UNION
	SELECT DISTINCT d.LocationId,LocationName,his.CompanyId,d.ClientId 
	FROM Location d
	INNER JOIN EmployeeIncident h ON d.LocationId=h.LocationId
	INNER JOIN EmploymentHistory his ON h.UserInformationId=his.UserInformationId
	WHERE d.ClientId=@ClientId

) tbl
GROUP BY CompanyId,LocationId,LocationName,ClientId;

INSERT INTO [dbo].[Location]([LocationName],[LocationDescription],[Address],[Address2],[LocationCityId],[LocationStateId],[ZipCode],[PhysicalAddress1],[PhysicalAddress2],[PhysicalCityId],[PhysicalStateId],[PhysicalZipCode],[PhoneNumber1],
	  [ExtensionNumber1],[PhoneNumber2],[ExtensionNumber2],[PhoneNumber3],[ExtensionNumber3],[FaxNumber],[CompanyId],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id])
SELECT      t.[LocationName],[LocationDescription],[Address],[Address2],[LocationCityId],[LocationStateId],[ZipCode],[PhysicalAddress1],[PhysicalAddress2],[PhysicalCityId],[PhysicalStateId],[PhysicalZipCode],[PhoneNumber1],
	  [ExtensionNumber1],[PhoneNumber2],[ExtensionNumber2],[PhoneNumber3],[ExtensionNumber3],[FaxNumber],t.[CompanyId],d.[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id]
FROM #Location t
INNER JOIN Location d ON t.LocationId=d.LocationId
WHERE NOT EXISTS ( SELECT * FROM [Location] Dep  WHERE Dep.LocationName = t.LocationName AND dep.CompanyId = t.CompanyId);

Update SD
SET SD.LocationId = (SELECT TOP 1 dd.LocationId FROM Location dd WHERE dd.LocationName=d.LocationName And dd.CompanyId=SD.CompanyId)
FROM EmploymentHistory SD
INNER JOIN Location d ON sd.LocationId = d.LocationId
INNER JOIN UserInformation u ON sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
WHERE d.ClientId=@ClientId and d.CompanyId is null;

Update SD
SET SD.LocationId = (SELECT TOP 1 IsNull(dd.LocationId,0) FROM Location dd WHERE dd.LocationName=d.LocationName And dd.CompanyId=his.CompanyId)
FROM EmployeeIncident SD
INNER JOIN Location d ON sd.LocationId = d.LocationId
INNER JOIN EmploymentHistory his ON sd.UserInformationId=his.UserInformationId
WHERE d.ClientId=@ClientId and d.CompanyId is null ;
END


