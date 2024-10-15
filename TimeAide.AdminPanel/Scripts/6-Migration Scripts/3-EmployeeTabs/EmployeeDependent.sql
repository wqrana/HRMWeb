CREATE PROCEDURE sp_DM_EmployeeDependent
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmployeeDependent] (
			 [Old_Id]
			,[UserInformationId]
			,[FirstName]
			,[LastName]
			,[BirthDate]
			,[SSN]
			,[DependentStatusId]
			,[GenderId]
			,[RelationshipId]
			,[ExpiryDate]
			,[IsHealthInsurance]
			,[IsDentalInsurance]
			,[IsTaxPurposes]
			,[IsFullTimeStudent]
			,[SchoolAttending]
			,[DocName]
			,[DocFilePath]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
     SELECT [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
			,[strName]
			,[strLastName]
			,[dtBirthDate]
			,[dbo].[fn_CLREnDecriptSSN](strSSN,'E')	
			,IsNull((select top(1) DependentStatusId from DependentStatus where StatusName = strStatus And ClientId = @ClientId),1) as [strStatus] 
			,(select top(1) GenderId from Gender where Old_Id = intGender And ClientId = @ClientId) as [intGender]
			,(select top(1) RelationshipId from Relationship where Old_Id = intRelationship And ClientId = @ClientId) as [intRelationship] 
			,[dtExpirationDate]
			,Isnull([intCountsForHealthInsurance],0)
			,Isnull([intCountsForDentalInsurance],0)
			,Isnull([intCountsForTaxPurposes],0)
			,Isnull([intFullTimeStudent],0)
			,[strSchoolAttending]
			,[docName]
			,NULL
			,@ClientId
			,1
			,GetDate()
			,CAST(IsNull([intStatus],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblDependents]
END
