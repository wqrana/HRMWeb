CREATE VIEW [dbo].[vw_rpt_EmployeeEmploymentHistory]
AS
SELECT        
		UserInformationId,
		EmploymentId,
		EmploymentHistoryId,  
		StartDate, 
		EndDate,
		EmpHistory.CompanyId,
		CompanyName,
		EmpHistory.DepartmentId,
		DepartmentName, 
		EmpHistory.SubDepartmentId,
		SubDepartmentName,
		SupervisorId,
		EmpHistory.EmployeeTypeId,
		empType.EmployeeTypeName,  
		EmpHistory.EmploymentTypeId,
		empmtType.EmploymentTypeName, 
		EmpHistory.PositionId,
		EmpHistory.EndDate as EmploymentEndDate,
		p.PositionName,
		loc.LocationName
		
FROM    EmploymentHistory EmpHistory
LEFT JOIN Company cmp on EmpHistory.CompanyId = cmp.CompanyId
LEFT JOIN Department dept on EmpHistory.DepartmentId = dept.DepartmentId
LEFT JOIN SubDepartment subDept on EmpHistory.SubDepartmentId = subDept.SubDepartmentId
LEFT JOIN EmployeeType empType on EmpHistory.EmployeeTypeId = empType.EmployeeTypeId
LEFT JOIN EmploymentType empmtType on EmpHistory.EmploymentTypeId = empmtType.EmploymentTypeId 
LEFT JOIN Position p on  EmpHistory.PositionId = p.PositionId
LEFT JOIN Location loc on EmpHistory.LocationId = loc.LocationId
WHERE EmpHistory.DataEntryStatus = 1