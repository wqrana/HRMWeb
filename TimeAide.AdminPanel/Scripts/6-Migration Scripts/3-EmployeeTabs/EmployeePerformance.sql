CREATE PROCEDURE sp_DM_EmployeePerformance
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmployeePerformance] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[SupervisorId]
		   ,[ActionTakenId]
		   ,[PerformanceDescriptionId]
		   ,[PerformanceResultId]
		   ,[ReviewDate]
		   ,[ExpiryDate]
		   ,[ReviewSummary]
		   ,[ReviewNote]
		   ,[DocName]
		   ,[DocFilePath]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strSupervisorID And ClientId = @ClientId) as strSupervisorID  
		   ,(select top(1) ActionTakenId from ActionTaken where Old_Id = intActionTaken And ClientId = @ClientId) as [intActionTaken] 
		   ,(select top(1) PerformanceDescriptionId from PerformanceDescription where Old_Id = intReviewDescription And ClientId = @ClientId) as [intReviewDescription] 
		   ,(select top(1) PerformanceResultId from PerformanceResult where Old_Id = [intReviewResult] And ClientId = @ClientId) as [intReviewResult] 
		   ,[dtReviewDate]
		   ,[dtExpirationDate]
		   ,[strReviewSummary]
		   ,[strReviewNote]
		   ,[docName]
		   ,NULL
		   ,@ClientId
		   ,1
		   ,GetDate()  
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEmployeePerformance]
END

