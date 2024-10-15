CREATE PROCEDURE sp_DM_DentalInsuranceCobraHistory
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[DentalInsuranceCobraHistory] (
		     [Old_Id]
			,[EmployeeDentalInsuranceId]
			,[DueDate]
			,[PaymentDate]
			,[CobraPaymentStatusId]
			,[PaymentAmount]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT [intID]
		   ,(Select top(1) EDI.EmployeeDentalInsuranceId from EmployeeDentalInsurance EDI where EDI.UserInformationId = (select top(1) emp.UserInformationId from UserInformation emp where emp.EmployeeId = strEmpID And emp.ClientId = @ClientId)) as strEmpID 
		   ,[dtDateDue]
		   ,[dtPaymentDate]
		   ,IsNull((select top(1) CobraPaymentStatusId from CobraPaymentStatus where CobraPaymentStatusName = [strPaymentStatus] And ClientId = @ClientId),2) as [strPaymentStatus] 
		   ,0
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblDentalCobraHistory]
	
END
