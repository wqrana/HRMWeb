
CREATE VIEW [dbo].[vw_rpt_EmployeeEducationInfo]
AS
SELECT 
  UserInformationId,
  dg.DegreeName,
  dg.DegreeId,
  empEdu.DateCompleted,
  empEdu.Title,
  empEdu.InstitutionName,
  empEdu.Note,
  empEdu.DocName
FROM EmployeeEducation empEdu
LEFT JOIN Degree dg ON  empEdu.DegreeId = dg.DegreeId
WHERE empEdu.DataEntryStatus = 1
