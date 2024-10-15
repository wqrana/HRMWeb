CREATE PROCEDURE sp_DM_SupervisorInformation
	@ClientId int
AS
BEGIN

INSERT INTO [dbo].[UserInformationRole]
           ([RoleId]
           ,[UserInformationId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus]
		   )
Select
		    (SELECT top(1) RoleId FROM [dbo].[Role] where RoleName='Supervisor' and ClientId=@ClientId)
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) as [strSupervisorID] 
	       ,@ClientId
	       ,1
	       ,GetDate()
	       ,1
FROM [TimeAideSource].[dbo].[tblEmploymentHistory] 
Where (select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) is not null


INSERT INTO [dbo].[UserEmployeeGroup]
           ([EmployeeGroupId]
           ,[UserInformationId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus]
		   )
Select
		    (SELECT top(1) EmployeeGroupId FROM [dbo].[EmployeeGroup] where EmployeeGroupName='Supervisor' and ClientId=@ClientId)
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) as [strSupervisorID] 
	       ,@ClientId
	       ,1
	       ,GetDate()
	       ,1
FROM [TimeAideSource].[dbo].[tblEmploymentHistory] 
Where (select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) is not null



INSERT INTO [dbo].[EmployeeSupervisor]
           ([EmployeeUserId]
           ,[SupervisorUserId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     Select
		    (select top(1) UserInformationId from UserInformation where EmployeeId = [strEmpID] And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) as [strSupervisorID] 
	       ,@ClientId
	       ,1
	       ,GetDate()
	       ,1
	FROM [TimeAideSource].[dbo].[tblEmploymentHistory]
	Where Not (select top(1) UserInformationId from UserInformation where EmployeeId = [strSupervisorID] And ClientId = @ClientId) is null 


	INSERT INTO [dbo].[PayInformationHistoryAuthorizer]
           ([PayInformationHistoryId]
           ,[AuthorizeById]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     Select
		    (select top(1) PayInformationHistoryId from PayInformationHistory where [Old_Id] = [intID] And ClientId = @ClientId) as [intID] 
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = [strAuthorizeByID] And ClientId = @ClientId) as [strAuthorizeByID] 
	       ,@ClientId
	       ,1
	       ,GetDate()
	       ,1
	FROM [TimeAideSource].[dbo].[tblPayInfoHistory]
	Where Not (select top(1) UserInformationId from UserInformation where EmployeeId = [strAuthorizeByID] And ClientId = @ClientId) is null 
	

END
