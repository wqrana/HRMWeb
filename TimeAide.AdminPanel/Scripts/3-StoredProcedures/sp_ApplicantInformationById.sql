CREATE PROCEDURE [dbo].[sp_ApplicantInformationById] 
	-- Add the parameters for the stored procedure here		
	@applicantId int
	
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

	WHERE applicantInfo.ApplicantInformationId = @applicantId
	AND applicantInfo.dataentrystatus = 1
END