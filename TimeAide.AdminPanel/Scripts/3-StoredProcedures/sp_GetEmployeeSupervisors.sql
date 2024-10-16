Create PROCEDURE [dbo].[sp_GetEmployeeSupervisors] 
	@UserInformationId int =0 ,
	@ClientId int=0
	
AS
BEGIN
		 
	Select distinct userInfo.*, UCI.LoginEmail
	From vw_UserInformation userInfo
	Inner Join UserContactInformation UCI on  userInfo.UserInformationId=UCI.UserInformationId
	Inner Join EmployeeSupervisor ES on ES.SupervisorUserId = userInfo.UserInformationId
	Where ES.EmployeeUserId = @UserInformationId And userInfo.ClientId = @ClientId And ES.ClientId = @ClientId 
	And UCI.ClientId = @ClientId
END