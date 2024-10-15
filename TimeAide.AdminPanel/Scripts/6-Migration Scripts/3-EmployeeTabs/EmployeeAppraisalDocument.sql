CREATE PROCEDURE sp_DM_EmployeeAppraisalDocument
	@ClientId int
AS
BEGIN
	
	
INSERT INTO [dbo].[EmployeeAppraisalDocument] (
		     [Old_Id]
			,[EmployeeAppraisalId]
			,[DocumentFileName]
			,[DocumentFilePath]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  [intID]
			,(select top(1) EmployeeAppraisalId from EmployeeAppraisal where old_ID = intID And ClientId = @ClientId) as intID
			,[docAppraisalName]
			,null
			,@ClientId
			,1
			,GetDate()
			,1
     FROM 
	 (
		--Select [intID],docAppraisalName  docAppraisalName,docAppraisalExtension  docAppraisalExtension,docAppraisalFile  docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		--Union 
		Select [intID],docAdditional1Name docAppraisalName,docAdditional1Extension docAppraisalExtension,docAdditional1File docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		Union 
		Select [intID],docAdditional2Name docAppraisalName,docAdditional2Extension docAppraisalExtension,docAdditional2File docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		Union 
		Select [intID],docAdditional3Name docAppraisalName,docAdditional3Extension docAppraisalExtension,docAdditional3File docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		Union 
		Select [intID],docAdditional4Name docAppraisalName,docAdditional4Extension docAppraisalExtension,docAdditional4File docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		Union 
		Select [intID],docAdditional5Name docAppraisalName,docAdditional5Extension docAppraisalExtension,docAdditional5File docAppraisalFile from [TimeAideSource].[dbo].[tblEmployeeAppraisals]
		
	 ) tblEmployeeAppraisalDocument
	
END
