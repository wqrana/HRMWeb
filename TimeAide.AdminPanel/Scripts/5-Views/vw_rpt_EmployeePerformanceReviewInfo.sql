CREATE VIEW [dbo].[vw_rpt_EmployeePerformanceReviewInfo]
AS
SELECT 
  ep.UserInformationId,
  ep.EmployeePerformanceId,  
  ep.ReviewDate,
  ep.SupervisorId,
  reviewer.ShortFullName as ReviewerName,
  ep.PerformanceDescriptionId,
  pd.PerformanceDescriptionName,
  ep.DocName,
  ep.PerformanceResultId,
  pr.PerformanceResultName,
  ep.ActionTakenId,
  acttk.ActionTakenName,
  ep.ExpiryDate,
  ep.ReviewSummary,
  ep.ReviewNote
 
FROM  EmployeePerformance ep
LEFT JOIN UserInformation reviewer ON  reviewer.UserInformationId = ep.UserInformationId
LEFT JOIN PerformanceDescription pd ON pd.PerformanceDescriptionId = ep.PerformanceDescriptionId
LEFT JOIN PerformanceResult pr		ON pr.PerformanceResultId = ep.PerformanceResultId
LEFT JOIN ActionTaken acttk ON acttk.ActionTakenId = ep.ActionTakenId 
WHERE ep.DataEntryStatus = 1