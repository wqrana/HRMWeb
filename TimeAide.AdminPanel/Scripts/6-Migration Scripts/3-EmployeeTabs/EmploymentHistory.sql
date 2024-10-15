CREATE PROCEDURE sp_DM_EmploymentHistory
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmploymentHistory] (
			 [Old_Id]
			,[UserInformationId]
			,[EmploymentId]
			,[StartDate]
			,[EndDate]
			,[PositionId]
			,[EmployeeTypeId]
			,[EmploymentTypeId]
			,[ChangeReason]
			,[LocationId]
			,[DepartmentId]
			,[SubDepartmentId]
			,[CompanyId]
			,[ApprovedDate]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
SELECT 
			 [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID --strEmpID
			,IsNull((select top(1) EmploymentId from Employment where dtDateStart Between EffectiveHireDate And Isnull(TerminationDate,Getdate()) And ClientId = @ClientId And Employment.UserInformationId = (select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId)),
			 IsNull((select top(1) EmploymentId from Employment where Employment.UserInformationId = (select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) Order by EffectiveHireDate desc),1))
			 as EmploymentId
			,[dtDateStart]
			,[dtDateEnd]
			,(select top(1) PositionId from Position where old_ID = intPosition And ClientId = @ClientId) as intPosition
			,1
			,(select top(1) EmploymentTypeId from EmploymentType where old_ID = intEmploymentType And ClientId = @ClientId) as intEmploymentType
			,strChangeReason
			,(select top(1) LocationId from Location where old_ID = intLocation And ClientId = @ClientId) as intLocation
			,(select top(1) DepartmentId from Department where old_ID = intDepartment And ClientId = @ClientId) as intDepartment
			,(select top(1) SubDepartmentId from SubDepartment where old_ID = intSubDepartment And ClientId = @ClientId) as intSubDepartment
			,(select top(1) CompanyId from Company where old_ID = intCompany And ClientId = @ClientId) as intCompany
			,[dtDateApproved]
			,2
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblEmploymentHistory] 
END
