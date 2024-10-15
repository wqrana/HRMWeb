CREATE PROCEDURE sp_DM_PayScale
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[PayScale] (
		    [Old_Id]
		   ,[PayScaleName]
           ,[RateFrequencyId]
		   ,[LevelCount]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strPayScaleName]
		   ,(select top(1) RateFrequencyId from RateFrequency where old_ID = intRateFrequency And ClientId = @ClientId) as intRateFrequency
		   ,[intLevelCount]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblPayScales]
	 
END