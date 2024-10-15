CREATE PROCEDURE sp_DM_RateFrequency
	@ClientId int
AS
BEGIN
INSERT INTO [dbo].[RateFrequency] (
		    [Old_Id]
		   ,[RateFrequencyName]
           ,[RateFrequencyDescription]
		   ,[HourlyMultiplier]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strRateFrequency]
		   ,[strDescription]
		   ,[dblHourlyMultiplier]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
     FROM [TimeAideSource].[dbo].[tblRateFrequency]
	 
END