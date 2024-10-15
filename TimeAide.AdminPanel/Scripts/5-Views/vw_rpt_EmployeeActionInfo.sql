CREATE VIEW [dbo].[vw_rpt_EmployeeActionInfo]
AS
SELECT 
UserInformationId,
empAction.EmployeeActionId,
empAction.ActionTypeId,
act.ActionTypeName,
empAction.ActionName,
empAction.ActionDescription,
empAction.ActionNotes,
empAction.ActionDate,
empAction.ActionExpiryDate

FROM EmployeeAction empAction
LEFT JOIN ActionType act ON  act.ActionTypeId = empAction.ActionTypeId
WHERE empAction.DataEntryStatus = 1