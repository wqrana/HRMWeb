CREATE VIEW [dbo].[vw_rpt_EmployeeDependentInfo]
AS
SELECT 
  ed.UserInformationId,
  ed.EmployeeDependentId,
  ed.FirstName,
  ed.LastName,
  ed.SSN as EdSSN,
  ed.BirthDate,
  ds.StatusName,
  gd.GenderName,
  rel.RelationshipName,
  ed.DocName,
  ed.ExpiryDate,
  ed.IsFullTimeStudent,
  ed.IsDentalInsurance,
  ed.IsHealthInsurance,
  ed.IsTaxPurposes,
  ed.SchoolAttending 
FROM  EmployeeDependent ed
LEFT JOIN DependentStatus ds ON ds.DependentStatusId = ed.DependentStatusId
LEFT JOIN Gender gd ON gd.GenderId = ed.GenderId
LEFT JOIN Relationship rel ON rel.RelationshipId = ed.RelationshipId
WHERE ed.DataEntryStatus = 1