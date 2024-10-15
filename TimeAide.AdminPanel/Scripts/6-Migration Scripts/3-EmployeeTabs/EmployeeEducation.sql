CREATE PROCEDURE sp_DM_EmployeeEducation
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeEducation] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[InstitutionName]
		   ,[Title]
		   ,[DegreeId]
		   ,[DocName]
		   ,[DocFilePath]
		   ,[Note]
		   ,[DateCompleted]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,[strInstitution]
		   ,[strTitle]
		   ,(select top(1) DegreeId from Degree where Old_Id = intDegree And ClientId = @ClientId) as intDegree 
		   ,[docName]
		   ,NULL
		   ,[strNote]
		   ,[dtYearCompleted]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeeEducation]
	
END
