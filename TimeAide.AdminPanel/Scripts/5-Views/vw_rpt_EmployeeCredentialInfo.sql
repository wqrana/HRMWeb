CREATE VIEW [dbo].[vw_rpt_EmployeeCredentialInfo]
AS
SELECT 
  UserInformationId,
  empCredential.CredentialId,
  empCredential.EmployeeCredentialId,
  Cred.CredentialName,
  empCredential.EmployeeCredentialName as CredentialExternalId,
  empCredential.EmployeeCredentialDescription,
  empCredential.IssueDate,
  empCredential.ExpirationDate,
  empCredential.Note,
  empCredential.IsRequired,
  empCredential.DocumentName
  
FROM EmployeeCredential empCredential
LEFT JOIN Credential Cred ON  Cred.CredentialId = empCredential.CredentialId
WHERE empCredential.DataEntryStatus = 1