CREATE VIEW [dbo].[vw_rpt_EmployeeBenefitInfo]
AS
SELECT 
UserInformationId,
empBenefit.EmployeeBenefitHistoryId,
empBenefit.BenefitId,
ben.BenefitName,
empBenefit.StartDate,
empBenefit.ExpiryDate,
empBenefit.Amount as BenefitAmount,
payFreq.PayFrequencyName,
empBenefit.Notes
FROM EmployeeBenefitHistory empBenefit
LEFT JOIN Benefit ben ON  ben.BenefitId = empBenefit.BenefitId
LEFT JOIN PayFrequency payFreq ON payFreq.PayFrequencyId = empBenefit.PayFrequencyId
WHERE empBenefit.DataEntryStatus = 1