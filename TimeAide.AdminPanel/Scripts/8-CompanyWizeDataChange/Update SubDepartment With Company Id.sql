-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdateSubDepartmentWithCompany
	@ClientId int
AS
BEGIN
	IF OBJECT_ID(N'tempdb..#SubDepartment') IS NOT NULL
		BEGIN
			DROP TABLE #SubDepartment
		END

	Alter Table SubDepartment Alter Column [DepartmentId] int null;

	SELECT DISTINCT CompanyId,SubDepartmentId,SubDepartmentName,ClientId into #SubDepartment
	FROM
	(
		SELECT DISTINCT d.SubDepartmentId,SubDepartmentName,h.CompanyId,d.ClientId 
		FROM SubDepartment d
		INNER JOIN EmploymentHistory h ON d.SubDepartmentId=h.SubDepartmentId
		INNER JOIN UserInformation u ON h.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
		WHERE d.ClientId=@ClientId
		UNION
		SELECT DISTINCT d.SubDepartmentId,SubDepartmentName,h.CompanyId,d.ClientId 
		FROM SubDepartment d
		INNER JOIN EmployeeAppraisal h ON d.SubDepartmentId=h.SubDepartmentId
		WHERE d.ClientId=@ClientId

	) tbl
	GROUP BY CompanyId,SubDepartmentId,SubDepartmentName,ClientId;
	--Select * from #SubDepartment
	INSERT INTO [dbo].[SubDepartment]([SubDepartmentName],[SubDepartmentDescription],[USECFSEAssignment],[CFSECodeId],[CFSECompanyPercent],
									  [DepartmentId],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id],[CompanyId])

	SELECT      t.[SubDepartmentName],SD.[SubDepartmentDescription],SD.[USECFSEAssignment],SD.[CFSECodeId],SD.[CFSECompanyPercent],
				null [DepartmentId],t.[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate],[Old_Id],t.[CompanyId]
	FROM #SubDepartment t
	INNER JOIN SubDepartment SD on t.SubDepartmentId=SD.SubDepartmentId
	WHERE NOT EXISTS ( SELECT * FROM [SubDepartment] Dep  WHERE Dep.SubDepartmentName = t.SubDepartmentName AND dep.CompanyId = t.CompanyId);

	Update SD
	SET SD.SubDepartmentId = IsNull((SELECT TOP 1 dd.SubDepartmentId FROM SubDepartment dd WHERE dd.SubDepartmentName=d.SubDepartmentName And dd.CompanyId=SD.CompanyId),SD.SubDepartmentId)
	FROM EmploymentHistory SD
	INNER JOIN SubDepartment d ON sd.SubDepartmentId = d.SubDepartmentId
	INNER JOIN UserInformation u ON sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	WHERE d.ClientId=@ClientId and d.CompanyId is null;

	Update SD
	SET SD.SubDepartmentId = IsNull((SELECT TOP 1 IsNull(dd.SubDepartmentId,0) FROM SubDepartment dd WHERE dd.SubDepartmentName=d.SubDepartmentName And dd.CompanyId=SD.CompanyId),SD.SubDepartmentId)
	FROM EmployeeAppraisal SD
	INNER JOIN SubDepartment d ON sd.SubDepartmentId = d.SubDepartmentId
	WHERE d.ClientId=@ClientId and d.CompanyId is null ;

	Update d
	SET d.DepartmentId = (SELECT TOP 1 DepartmentId FROM Department dd WHERE dd.DepartmentId=SD2.DepartmentId And dd.CompanyId=SD2.CompanyId)
	FROM EmploymentHistory SD2
	INNER JOIN SubDepartment d ON SD2.SubDepartmentId = d.SubDepartmentId
	WHERE d.ClientId=@ClientId  
	And d.SubDepartmentId In
	(
	Select distinct SubDepartmentId from
	(
		Select distinct sd1.DepartmentId,sd1.SubDepartmentId,sd1.CompanyId from EmploymentHistory sd1
		INNER JOIN UserInformation u ON sd1.UserInformationId=u.UserInformationId 
		where u.ClientId=@ClientId
	) tbl group by SubDepartmentId
	having count(*)=1
	)
END

