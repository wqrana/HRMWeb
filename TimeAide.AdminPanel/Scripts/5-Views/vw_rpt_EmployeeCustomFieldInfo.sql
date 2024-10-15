CREATE VIEW [dbo].[vw_rpt_EmployeeCustomFieldInfo]
AS
SELECT 
  UserInformationId,
  empCustomField.CustomFieldId,
  empCustomField.EmployeeCustomFieldId,
  CustFd.CustomFieldName,
  CustFd.CustomFieldDescription,
  CustFd.IsExpirable,
  empCustomField.CustomFieldValue,
  empCustomField.CustomFieldNote,
  empCustomField.ExpirationDate 
  
FROM EmployeeCustomField empCustomField
LEFT JOIN CustomField CustFd ON  CustFd.CustomFieldId = empCustomField.CustomFieldId
WHERE empCustomField.DataEntryStatus = 1