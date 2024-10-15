CREATE PROCEDURE sp_DM_UserInformation
	@ClientId int
AS
BEGIN

INSERT INTO [dbo].[UserInformation] (
			Old_Id	,
			EmployeeId	,
			SystemId	,
			FirstName	,
			MiddleInitial	,
			FirstLastName	,
			SecondLastName	,
			ShortFullName	,
			EmployeeTypeID	,
			DisabilityId	,
			EthnicityId	,
			GenderId	,
			BirthDate	,
			BirthPlace	,
			MaritalStatusId	,
			SSNEncrypted	,
			EmployeeStatusId	,
			ResumeFilePath	,
			PictureFilePath,
			ClientId	,
			CompanyID,
			CreatedBy	,
			CreatedDate	,
			DataEntryStatus	
			)
SELECT 
			intId	,
			strEmployeeID	,
			strSystemID	,
			strFirstName	,
			strMiddleInitial	,
			strFirstLastName	,
			strSecondLastName	,
			CASE
				When Len(strFirstName + ' ' + strFirstLastName + ' ' + strSecondLastName)>50 Then
					LEFT((strFirstName + ' ' + strFirstLastName + ' ' + strSecondLastName),50)
				ELSE
					strFirstName + ' ' + strFirstLastName + ' ' + strSecondLastName
			END AS ShortFullName,
			(select top(1) EmployeeTypeId from EmployeeType where old_ID = intEmployeeType And ClientId = @ClientId) as intEmployeeType	,
			(select top(1) DisabilityId from Disability where old_ID = intEEODisability And ClientId = @ClientId) as intEEODisability	,
			(select top(1) EthnicityId from Ethnicity where old_ID = intEEOEthnicity And ClientId = @ClientId) as intEEOEthnicity	,
			CASE WHEN strGender like 'F%' THEN 2 WHEN strGender like 'M%' THEN 1 END As strGender	,
			dtBirthDate	,
			strBirthPlace	,
			(select top(1) MaritalStatusId from MaritalStatus where old_ID = intMaritalStatus And ClientId = @ClientId) as intMaritalStatus	,
			[dbo].[fn_CLREnDecriptSSN](strSSN,'E')	,
			--(select top(1) EmployeeStatusId from EmployeeStatus where old_ID = intEmployeeStatus) as intEmployeeStatus	,
			intEmployeeStatus,
			docName,
			CASE
				WHEN [imgPhoto] is not null Then 
					strEmployeeID + '.jpg' 
				ELSE
					Null
			END As EmployeePhoto,
			@ClientId As ClientId	,
			IsNull((Select top(1) CompanyId from [dbo].Company where  ClientId = @ClientId And Old_Id = (select top(1) intCompany from [TimeAideSource].[dbo].[tblEmploymentHistory] where tblEmployee.strEmployeeID = strEmpID And tblEmploymentHistory.intCurrentRecord=1)) 
			      ,ISNuLL((Select top(1) CompanyId from [dbo].Company where ClientId = @ClientId And Old_Id = (select top(1) intCompany from [TimeAideSource].[dbo].[tblEmploymentHistory] where tblEmployee.strEmployeeID = strEmpID Order by dtDateStart desc))
						  ,(Select top(1) CompanyId from [dbo].Company where ClientId = @ClientId And CompanyName = 'None-Company'))
			) as CompanyID ,
			1 As CreatedBy	,
			GetDate() CreayedDate	,
			1 as DataEntrtyStatus
     FROM [TimeAideSource].[dbo].[tblEmployee] 
	 
END