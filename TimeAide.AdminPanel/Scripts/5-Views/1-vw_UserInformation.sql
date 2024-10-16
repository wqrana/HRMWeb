--CREATE VIEW [dbo].[vw_UserInformation]
--AS
--SELECT ui.[UserInformationId] As Id
--,ui.[UserInformationId]
--,ui.Old_Id
--,[EmployeeId]
--,[IdNumber]
--,[SystemId]
--,[FirstName]
--,[MiddleInitial]
--,[FirstLastName]
--,[SecondLastName]
--,[ShortFullName]
--,ui.[ClientId]
--,[DefaultJobCodeId]
--,ui.[EthnicityId]
--,EthnicityName
--,ui.[DisabilityId]
--,DisabilityName
--,ui.[EmployeeNote]
--,ui.[GenderId]
--,GenderName
--,[BirthDate]
--,[BirthPlace]
--,ui.[MaritalStatusId]
--,MaritalStatusName
--,[SSNEncrypted]
--,[SSNEnd]
--,[PasswordHash]
--,ui.[EmployeeStatusId]
--,es.EmployeeStatusName
--,[AspNetUserId]
--,ui.[CreatedBy]
--,ui.[CreatedDate]
--,ui.[DataEntryStatus]
--,ui.[ModifiedBy]
--,ui.[ModifiedDate]
--,[PictureFilePath]
--,[ResumeFilePath]
--,[EmployeeStatusDate]
--,ui.[CompanyID]
--,EmpHistory.EmploymentHistoryId
--,EmpHistory.[DepartmentId]
--,EmpHistory.[SubDepartmentId]
--,EmpHistory.[EmployeeTypeID]
--,EmpHistory.[PositionId]
--,EmpHistory.[LocationId]
--,EmpHistory.[EmploymentTypeId]
--,EmpHistory.[SupervisorId]
--,EmpHiring.EmploymentId
--,EmpHiring.[EmploymentStatusId]
--,PayInfo.PayInformationHistoryId

--FROM UserInformation AS ui
--LEFT JOIN Ethnicity et ON et.EthnicityId = ui.EthnicityId
--LEFT JOIN Disability dis ON dis.DisabilityId = ui.DisabilityId
--LEFT JOIN Gender gn ON gn.GenderId = ui.GenderId
--LEFT JOIN MaritalStatus ms ON ms.MaritalStatusId = ui.MaritalStatusId
--LEFT JOIN EmployeeStatus es ON es.EmployeeStatusId = ui.EmployeeStatusId

--LEFT JOIN (Select eh.UserInformationId, eh.EmploymentHistoryId,eh.[DepartmentId],eh.[SubDepartmentId],eh.[EmployeeTypeID],eh.[EmploymentTypeId],eh.[SupervisorId],eh.LocationId , eh.PositionId
--From EmploymentHistory AS eh
--INNER JOIN (SELECT UserInformationId, MAX(StartDate) AS StartDate
--FROM EmploymentHistory
--WHERE DataEntryStatus = 1
--GROUP BY UserInformationId) AS currEH ON EH.UserInformationId = currEH.UserInformationId AND EH.StartDate = currEH.StartDate
--) EmpHistory ON EmpHistory.UserInformationId = ui.UserInformationId

--LEFT JOIN( Select h.UserInformationId,h.EmploymentId,h.EmploymentStatusId
--From Employment h
--INNER JOIN (SELECT UserInformationId, MAX(OriginalHireDate) AS OriginalHireDate
--FROM Employment
--WHERE DataEntryStatus = 1
--GROUP BY UserInformationId) AS currHiring ON currHiring.UserInformationId = h.UserInformationId AND currHiring.OriginalHireDate = h.OriginalHireDate
--)EmpHiring ON EmpHiring.UserInformationId = ui.UserInformationId
--LEFT JOIN( Select pih.UserInformationId, pih.PayInformationHistoryId
--From PayInformationHistory AS pih
--INNER JOIN (SELECT UserInformationId, MAX(StartDate) AS StartDate
--FROM PayInformationHistory
--WHERE DataEntryStatus = 1
--GROUP BY UserInformationId) AS currPayInfo ON currPayInfo.UserInformationId = pih.UserInformationId AND currPayInfo.StartDate = pih.StartDate
--) PayInfo ON PayInfo.UserInformationId = ui.UserInformationId

CREATE VIEW [dbo].[vw_UserInformation]
AS
SELECT ui.[UserInformationId] As Id
,ui.[UserInformationId]
,ui.Old_Id
,[EmployeeId]
,[IdNumber]
,[SystemId]
,[FirstName]
,[MiddleInitial]
,[FirstLastName]
,[SecondLastName]
,[ShortFullName]
,ui.[ClientId]
,[DefaultJobCodeId]
,ui.[EthnicityId]
,EthnicityName
,ui.[DisabilityId]
,DisabilityName
,ui.[EmployeeNote]
,ui.[GenderId]
,GenderName
,[BirthDate]
,[BirthPlace]
,ui.[MaritalStatusId]
,MaritalStatusName
,[SSNEncrypted]
,[SSNEnd]
,[PasswordHash]
,ui.[EmployeeStatusId]
,es.EmployeeStatusName
,[AspNetUserId]
,ui.[CreatedBy]
,ui.[CreatedDate]
,ui.[DataEntryStatus]
,ui.[ModifiedBy]
,ui.[ModifiedDate]
,[PictureFilePath]
,[ResumeFilePath]
,[EmployeeStatusDate]
,ui.[CompanyID]
,ui.refUserInformationId
,ui.IsRotatingSchedule
,ui.BaseScheduleId
,EmpHistory.EmploymentHistoryId
,EmpHistory.[DepartmentId]
,EmpHistory.[SubDepartmentId]
,EmpHistory.[EmployeeTypeID]
,EmpHistory.[PositionId]
,EmpHistory.[LocationId]
,EmpHistory.[EmploymentTypeId]
,EmpHistory.[SupervisorId]
,EmpHiring.EmploymentId
,EmpHiring.[EmploymentStatusId]
,PayInfo.PayInformationHistoryId

FROM UserInformation AS ui
LEFT JOIN Ethnicity et ON et.EthnicityId = ui.EthnicityId
LEFT JOIN Disability dis ON dis.DisabilityId = ui.DisabilityId
LEFT JOIN Gender gn ON gn.GenderId = ui.GenderId
LEFT JOIN MaritalStatus ms ON ms.MaritalStatusId = ui.MaritalStatusId
LEFT JOIN EmployeeStatus es ON es.EmployeeStatusId = ui.EmployeeStatusId
LEFT JOIN( 
		Select *
		From(
			Select ROW_NUMBER() over(partition by UserInformationId,EffectiveHireDate order by EmploymentId desc ) as EmpSeq, UserInformationId,EmploymentId,EmploymentStatusId,EffectiveHireDate
			From Employment) as h
		INNER JOIN (SELECT UserInformationId as CurrUserInformationId, MAX(EffectiveHireDate) AS CurrEffectiveHireDate
		FROM Employment
		WHERE DataEntryStatus = 1
	GROUP BY UserInformationId) AS currHiring ON currHiring.CurrUserInformationId = h.UserInformationId AND currHiring.CurrEffectiveHireDate = h.EffectiveHireDate
											  AND h.EmpSeq =1
)EmpHiring ON EmpHiring.UserInformationId = ui.UserInformationId

LEFT JOIN (
	Select * 
	From(
		Select ROW_NUMBER() over(partition by EmploymentId,StartDate order by EmploymentHistoryId desc ) as EmpHistorySeq ,UserInformationId,EmploymentId ,EmploymentHistoryId,[DepartmentId],[SubDepartmentId],[EmployeeTypeID],[EmploymentTypeId],[SupervisorId],LocationId ,PositionId,StartDate
		From EmploymentHistory ) as eh
		INNER JOIN (Select EmploymentId as CurrEmploymentId, MAX(StartDate) AS CurrStartDate
		FROM EmploymentHistory
		WHERE DataEntryStatus = 1 		
		GROUP BY EmploymentId) AS currEH ON EH.EmploymentId = currEH.CurrEmploymentId AND EH.StartDate = currEH.CurrStartDate AND eh.EmpHistorySeq=1
) EmpHistory ON EmpHistory.UserInformationId = ui.UserInformationId AND EmpHistory.EmploymentId = EmpHiring.EmploymentId


LEFT JOIN( select *
			From(
				Select ROW_NUMBER() over(partition by EmploymentId,StartDate order by PayInformationHistoryId desc ) as PayInfoSeq, UserInformationId,EmploymentId ,PayInformationHistoryId,StartDate
				From PayInformationHistory) AS pih
INNER JOIN (SELECT EmploymentId as CurrEmploymentId, MAX(StartDate) AS CurrStartDate
FROM PayInformationHistory
WHERE DataEntryStatus = 1
GROUP BY EmploymentId) AS currPayInfo ON currPayInfo.CurrEmploymentId = pih.EmploymentId AND currPayInfo.CurrStartDate = pih.StartDate And pih.PayInfoSeq =1
) PayInfo ON PayInfo.UserInformationId = ui.UserInformationId AND PayInfo.EmploymentId = EmpHiring.EmploymentId



--SELECT ui.[UserInformationId] As Id
--,ui.[UserInformationId]
--,ui.Old_Id
--,[EmployeeId]
--,[IdNumber]
--,[SystemId]
--,[FirstName]
--,[MiddleInitial]
--,[FirstLastName]
--,[SecondLastName]
--,[ShortFullName]
--,ui.[ClientId]
--,[DefaultJobCodeId]
--,ui.[EthnicityId]
--,EthnicityName
--,ui.[DisabilityId]
--,DisabilityName
--,ui.[EmployeeNote]
--,ui.[GenderId]
--,GenderName
--,[BirthDate]
--,[BirthPlace]
--,ui.[MaritalStatusId]
--,MaritalStatusName
--,[SSNEncrypted]
--,[SSNEnd]
--,[PasswordHash]
--,ui.[EmployeeStatusId]
--,es.EmployeeStatusName
--,[AspNetUserId]
--,ui.[CreatedBy]
--,ui.[CreatedDate]
--,ui.[DataEntryStatus]
--,ui.[ModifiedBy]
--,ui.[ModifiedDate]
--,[PictureFilePath]
--,[ResumeFilePath]
--,[EmployeeStatusDate]
--,ui.[CompanyID]
--,ui.refUserInformationId
--,EmpHistory.EmploymentHistoryId
--,EmpHistory.[DepartmentId]
--,EmpHistory.[SubDepartmentId]
--,EmpHistory.[EmployeeTypeID]
--,EmpHistory.[PositionId]
--,EmpHistory.[LocationId]
--,EmpHistory.[EmploymentTypeId]
--,EmpHistory.[SupervisorId]
--,EmpHiring.EmploymentId
--,EmpHiring.[EmploymentStatusId]
--,PayInfo.PayInformationHistoryId

--FROM UserInformation AS ui
--LEFT JOIN Ethnicity et ON et.EthnicityId = ui.EthnicityId
--LEFT JOIN Disability dis ON dis.DisabilityId = ui.DisabilityId
--LEFT JOIN Gender gn ON gn.GenderId = ui.GenderId
--LEFT JOIN MaritalStatus ms ON ms.MaritalStatusId = ui.MaritalStatusId
--LEFT JOIN EmployeeStatus es ON es.EmployeeStatusId = ui.EmployeeStatusId

--LEFT JOIN (
--	Select * 
--	From(
--		Select ROW_NUMBER() over(partition by UserInformationId order by EmploymentHistoryId desc ) as EmpHistorySeq ,UserInformationId, EmploymentHistoryId,[DepartmentId],[SubDepartmentId],[EmployeeTypeID],[EmploymentTypeId],[SupervisorId],LocationId ,PositionId,StartDate
--		From EmploymentHistory ) as eh
--		INNER JOIN (Select UserInformationId as CurrUserInformationId, MAX(StartDate) AS CurrStartDate
--		FROM EmploymentHistory
--		WHERE DataEntryStatus = 1
--		GROUP BY UserInformationId) AS currEH ON EH.UserInformationId = currEH.CurrUserInformationId AND EH.StartDate = currEH.CurrStartDate AND eh.EmpHistorySeq=1
--) EmpHistory ON EmpHistory.UserInformationId = ui.UserInformationId

--LEFT JOIN( 
--		Select *
--		From(
--			Select ROW_NUMBER() over(partition by UserInformationId order by EmploymentId desc ) as EmpSeq, UserInformationId,EmploymentId,EmploymentStatusId,EffectiveHireDate
--			From Employment) as h
--		INNER JOIN (SELECT UserInformationId as CurrUserInformationId, MAX(EffectiveHireDate) AS CurrEffectiveHireDate
--		FROM Employment
--		WHERE DataEntryStatus = 1
--	GROUP BY UserInformationId) AS currHiring ON currHiring.CurrUserInformationId = h.UserInformationId AND currHiring.CurrEffectiveHireDate = h.EffectiveHireDate
--											  AND h.EmpSeq =1
--)EmpHiring ON EmpHiring.UserInformationId = ui.UserInformationId

--LEFT JOIN( select *
--			From(
--				Select ROW_NUMBER() over(partition by UserInformationId order by PayInformationHistoryId desc ) as PayInfoSeq, UserInformationId, PayInformationHistoryId,StartDate
--				From PayInformationHistory) AS pih
--INNER JOIN (SELECT UserInformationId as CurrUserInformationId, MAX(StartDate) AS CurrStartDate
--FROM PayInformationHistory
--WHERE DataEntryStatus = 1
--GROUP BY UserInformationId) AS currPayInfo ON currPayInfo.CurrUserInformationId = pih.UserInformationId AND currPayInfo.CurrStartDate = pih.StartDate And pih.PayInfoSeq =1
--) PayInfo ON PayInfo.UserInformationId = ui.UserInformationId


