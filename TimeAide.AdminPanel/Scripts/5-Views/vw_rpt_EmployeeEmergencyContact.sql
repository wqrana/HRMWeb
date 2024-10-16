CREATE VIEW [dbo].[vw_rpt_EmployeeEmergencyContact]
AS
SELECT        
UserInformationId, 
EmergencyContactId, 
ec.RelationshipId,
r.RelationshipName, 
ContactPersonName, 
MainNumber, 
AlternateNumber
FROM EmergencyContact ec
LEFT JOIN Relationship r ON ec.RelationshipId = r.RelationshipId
WHERE ec.DataEntryStatus = 1
