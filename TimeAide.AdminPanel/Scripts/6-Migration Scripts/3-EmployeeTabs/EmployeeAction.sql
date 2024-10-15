CREATE PROCEDURE sp_DM_EmployeeAction
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmployeeAction] (
		     [Old_Id]
			,[UserInformationId]
			,[ApprovedById]
			,[ActionTypeId]
			,[ActionDate]
			,[ActionEndDate]
			,[ActionName]
			,[ActionDescription]
			,[ActionNotes]
			,[ActionClosingInfo]
			,[ActionApprovedDate]
			,[DocName]
			,[DocFilePath]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  [intID]
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID
			,(select top(1) UserInformationId from UserInformation where EmployeeId = strActionApprovedBy And ClientId = @ClientId) as strActionApprovedBy
			,(select top(1) ActionTypeId from ActionType where old_ID = intActionType And ClientId = @ClientId) as intActionType
			,[dtActionDate]
			,[dtActionEndDate]
			,[strActionName]
			,[strActionDescription]
			,[strActionNotes]
			,[strActionClosingInfo]
			,[dtActionApprovedDate]
			,[docName]
			,NULL
			,@ClientId
			,1
			,GetDate()
			,1
     FROM [TimeAideSource].[dbo].[tblEmployeeAction]
END
