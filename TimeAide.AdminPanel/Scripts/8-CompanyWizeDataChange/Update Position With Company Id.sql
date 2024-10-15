-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_UpdatePositionWithCompany
	@ClientId int
AS
BEGIN
	IF OBJECT_ID(N'tempdb..#Position') IS NOT NULL
		BEGIN
			DROP TABLE #Position
		END

	SELECT DISTINCT CompanyId,PositionId,PositionName,ClientId into #Position
	FROM
	(
		SELECT DISTINCT d.PositionId,PositionName,h.CompanyId,d.ClientId 
		FROM Position d
		INNER JOIN EmploymentHistory h ON d.PositionId=h.PositionId
		INNER JOIN UserInformation u ON h.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
		WHERE d.ClientId=@ClientId
		UNION
		SELECT DISTINCT d.PositionId,PositionName,h.CompanyId,d.ClientId 
		FROM Position d
		INNER JOIN EmployeeAppraisal h ON d.PositionId=h.PositionId
		WHERE d.ClientId=@ClientId

	) tbl
	GROUP BY CompanyId,PositionId,PositionName,ClientId;

	INSERT INTO [dbo].[Position]([PositionName],[PositionDescription],[PositionCode],[DefaultPayScaleId],[DefaultEEOCategoryId],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate]
			   ,[Old_Id],CompanyId)
	SELECT t.[PositionName],[PositionDescription],[PositionCode],[DefaultPayScaleId],[DefaultEEOCategoryId],SD.[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus],[ModifiedBy],[ModifiedDate]
		  ,[Old_Id],t.CompanyId
	  FROM #Position t
	INNER JOIN Position SD on t.PositionId=SD.PositionId
	WHERE NOT EXISTS ( SELECT * FROM [Position] POS  WHERE POS.PositionName = t.PositionName AND POS.CompanyId = t.CompanyId);

	Update SD
	SET SD.PositionId = (SELECT TOP 1 dd.PositionId FROM Position dd WHERE dd.PositionName=d.PositionName And dd.CompanyId=SD.CompanyId)
	FROM EmploymentHistory SD
	INNER JOIN Position d ON sd.PositionId = d.PositionId
	INNER JOIN UserInformation u ON sd.UserInformationId=u.UserInformationId and u.ClientId=d.ClientId
	WHERE d.ClientId=@ClientId and d.CompanyId is null;

	Update SD
	SET SD.PositionId = (SELECT TOP 1 IsNull(dd.PositionId,0) FROM Position dd WHERE dd.PositionName=d.PositionName And dd.CompanyId=SD.CompanyId)
	FROM EmployeeAppraisal SD
	INNER JOIN Position d ON sd.PositionId = d.PositionId
	WHERE d.ClientId=@ClientId and d.CompanyId is null ;
END