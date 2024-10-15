CREATE PROCEDURE sp_DM_EmployeeStatus
	@ClientId int
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[EmployeeStatus] (
		    [Old_Id]
		   ,[EmployeeStatusName]
           ,[EmployeeStatusDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT [intID]
		   ,[strEmployeeStatus]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
    FROM [TimeAideSource].[dbo].[tblEmployeeStatus]
END