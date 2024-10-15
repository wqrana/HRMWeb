CREATE PROCEDURE sp_DM_City
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[City] (
		    [Old_Id]
		   ,[CityName]
           ,[StateId]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strCity]
		   ,1
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblCities] where strCity not in (Select CityName from [dbo].[City] Where ClientId = @ClientId)
	 
END