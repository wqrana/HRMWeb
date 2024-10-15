-- =============================================
-- Author:		<WaqarQ>
-- Create date: <2021-04-16>
-- Description:	<Fetch the Employee OHSA Reports data>
-- =============================================
CREATE PROCEDURE [dbo].[sp_rpt_EmployeeOSHA301Report] 
	-- Add the parameters for the stored procedure here
	
	@employeeIncidentId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		

	SELECT Distinct
	
	employeeIncidentOSHA.*,	
	empgrm.ShortFullName as employeeName,
	empgrm.PositionName,
	empgrm.BirthDate as EmployeeBirthDate,
	empgrm.GenderName as EmployeeGender,
	empgrm.EffectiveHireDate,
	empgrm.MailingAddress1 +' '+ISNULL(empgrm.MailingAddress2,'') As MailingAddress,
	empgrm.MailingStateName,
	empgrm.MailingCityName,
	empgrm.MailingZipCode,
	completedByEmp.PositionName as CompletedByPositionName,
	completedByEmp.CelNumber as CompletedByCelNumber

	FROM vw_rpt_EmployeeGeneralRptMain empgrm
	INNER JOIN vw_rpt_EmployeeIncidentOSHA employeeIncidentOSHA On employeeIncidentOSHA.UserInformationId = empgrm.UserInformationId
	LEFT JOIN vw_rpt_EmployeeGeneralRptMain completedByEmp On completedByEmp.UserInformationId = employeeIncidentOSHA.CompletedById
	WHERE employeeIncidentOSHA.EmployeeIncidentId = @employeeIncidentId
	AND  empgrm.DataEntryStatus = 1
	--based on selected employees
	
END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[sp_ApplicantInformation]'
GO

CREATE PROCEDURE [dbo].[sp_ApplicantInformation] 
	-- Add the parameters for the stored procedure here

	@applicantName nvarchar(250) = '',
	@positionId int = 0,
	@applicantStatusId int = 0,
	@clientId int,
	@companyId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
		 
	Select distinct applicantInfo.*,
	cmp.CompanyName,	
	appStatus.ApplicantStatusName,
	appStatus.UseAsHire,
	CelNumber,
	PersonalEmail,
	aApp.PositionId as PositionId,
	apos.PositionName
	From ApplicantInformation applicantInfo
	Left Join ApplicantContactInformation applicantCntInfo On applicantCntInfo.ApplicantInformationId = applicantInfo.ApplicantInformationId
	Left Join ApplicantApplication aApp On aApp.ApplicantInformationId = applicantInfo.ApplicantInformationId
	Left Join Company cmp On applicantInfo.CompanyID = cmp.CompanyId		
	Left Join ApplicantStatus appStatus On applicantInfo.ApplicantStatusId = appStatus.ApplicantStatusId
	Left Join Gender gnd ON gnd.GenderId = applicantInfo.GenderId
	Left Join Position apos On aApp.PositionId = apos.PositionId

	WHERE  applicantInfo.ShortFullName like '%'+ @applicantName + '%'
	--AND IsNull(applicantInfo.PositionId,0)	= IIF(@positionId = 0,IsNull(applicantInfo.PositionId,0),@positionId)
	AND applicantInfo.ApplicantStatusId		= IIF(@applicantStatusId = 0,applicantInfo.ApplicantStatusId,@applicantStatusId)
	AND applicantInfo.clientid =  @clientId
	AND applicantInfo.companyid = @companyId
	AND applicantInfo.dataentrystatus = 1
END