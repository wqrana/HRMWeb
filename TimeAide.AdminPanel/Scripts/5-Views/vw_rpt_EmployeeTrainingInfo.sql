CREATE VIEW [dbo].[vw_rpt_EmployeeTrainingInfo]
AS
SELECT 
  UserInformationId,
  tr.TrainingId,
  tr.TrainingName,
  empTraining.TrainingDate,
  trType.TrainingTypeName as TrainingType,
  empTraining.ExpiryDate,
  empTraining.Note,
  empTraining.DocName,
  empTraining.TrainingTypeId,
  trType.TrainingTypeName
FROM EmployeeTraining empTraining
LEFT JOIN Training tr ON  empTraining.TrainingId = tr.TrainingId
LEFT JOIN TrainingType trType ON empTraining.TrainingTypeId = trType.TrainingTypeId
WHERE empTraining.DataEntryStatus = 1