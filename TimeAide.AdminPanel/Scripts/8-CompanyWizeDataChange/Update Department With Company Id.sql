-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdateDepartmentWithCompany
	@ClientId int
AS
BEGIN
	IF OBJECT_ID(N'tempdb..#Department') IS NOT NULL
		BEGIN
			DROP TABLE #Department
		END

	SELECT DISTINCT CompanyId,DepartmentId,DepartmentName,ClientId into #Department
	FROM
	(
		SELECT DISTINCT d.departmentId,DepartmentName,h.CompanyId,d.ClientId 
		FROM Department d
		INNER JOIN EmploymentHistory h ON d.DepartmentId=h.DepartmentId
		INNER JOIN UserInformation u ON h.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
		WHERE d.ClientId=@ClientId
		UNION
		SELECT DISTINCT d.departmentId,DepartmentName,h.CompanyId,d.ClientId 
		FROM Department d
		INNER JOIN EmployeeAppraisal h ON d.DepartmentId=h.DepartmentId
		WHERE d.ClientId=@ClientId

	) tbl
	GROUP BY CompanyId,DepartmentId,DepartmentName,ClientId;

	INSERT INTO [dbo].[Department]([DepartmentName],[DepartmentDescription],[USECFSEAssignment],[CFSECodeId],[CFSECompanyPercent],[ClientId],
								   [CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id],[CompanyId])
	SELECT      d.[DepartmentName],d.[DepartmentDescription],d.[USECFSEAssignment],d.[CFSECodeId],d.[CFSECompanyPercent],d.[ClientId],d.[CreatedBy],d.[CreatedDate],d.[DataEntryStatus],
				d.[ModifiedBy],d.[ModifiedDate],d.[Old_Id],t.[CompanyId] 
	FROM #Department t
	INNER JOIN department d ON t.DepartmentId=d.DepartmentId
	WHERE NOT EXISTS ( SELECT * FROM [Department] Dep  WHERE Dep.DepartmentName = t.DepartmentName AND dep.CompanyId = t.CompanyId);

	Update SD
	SET SD.DepartmentId = (SELECT TOP 1 dd.DepartmentId FROM Department dd WHERE dd.DepartmentName=d.DepartmentName And dd.CompanyId=SD.CompanyId)
	FROM EmploymentHistory SD
	INNER JOIN department d ON sd.DepartmentId = d.DepartmentId
	INNER JOIN UserInformation u ON sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	WHERE d.ClientId=@ClientId and d.CompanyId is null;

	Update SD
	SET SD.DepartmentId = (SELECT TOP 1 IsNull(dd.DepartmentId,0) FROM Department dd WHERE dd.DepartmentName=d.DepartmentName And dd.CompanyId=SD.CompanyId)
	FROM EmployeeAppraisal SD
	INNER JOIN department d ON sd.DepartmentId = d.DepartmentId
	WHERE d.ClientId=@ClientId and d.CompanyId is null ;
END

