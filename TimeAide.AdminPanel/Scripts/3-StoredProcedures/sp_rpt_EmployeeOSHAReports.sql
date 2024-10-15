-- =============================================
-- Author:		<WaqarQ>
-- Create date: <2021-04-16>
-- Description:	<Fetch the Employee OHSA Reports data>
-- =============================================
CREATE PROCEDURE [dbo].[sp_rpt_EmployeeOSHAReports] 
	-- Add the parameters for the stored procedure here
	@employeeIds nvarchar(max)='',
	@reportYear int,
	@locationId int,
	@superviserId int,
	@clientId int,
	@companyId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @selectedEmployeeTbl As Table(Id Bigint)
	
	---Employee List
	Insert Into @selectedEmployeeTbl
	Select Id 
	From dbo.fn_splitIntIds(@employeeIds,',')
	

	SELECT Distinct
	ROW_NUMBER() over(order by employeeIncidentOSHA.IncidentDate) as RowId ,
	employeeIncidentOSHA.*,
	empgrm.ShortFullName as employeeName,
	empgrm.PositionName
	FROM vw_rpt_EmployeeGeneralRptMain empgrm
	INNER JOIN vw_rpt_EmployeeIncidentOSHA employeeIncidentOSHA On employeeIncidentOSHA.UserInformationId = empgrm.UserInformationId	
	WHERE empgrm.UserInformationId IN(Select distinct UserInformationId from dbo.fn_SupervisedEmployees(@clientId,@superviserId)) 
	AND empgrm.CompanyID = @companyId
	AND  empgrm.DataEntryStatus = 1
	--based on selected employees
	AND  (empgrm.EmployeeId in (Select * From @selectedEmployeeTbl)
	        OR empgrm.EmployeeId = IIF(@employeeIds='', empgrm.EmployeeId,0))	
	AND  (employeeIncidentOSHA.LocationId = @locationId)
	order by IncidentDate

END