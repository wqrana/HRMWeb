CREATE PROCEDURE sp_DM_EmployeeVeteranStatus
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[EmployeeVeteranStatus] (
		    [Old_Id]
		   ,[UserInformationId]
           ,[VeteranStatusId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID And ClientId = @ClientId) as strEmpID 
		   ,(select top(1) VeteranStatusId from VeteranStatus where Old_Id = intEEOVeteranStatus And ClientId = @ClientId) as intEEOVeteranStatus 
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblEEOVeteranStatusEnlisted]
	
END
