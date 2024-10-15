CREATE PROCEDURE sp_DM_PayFrequency
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[PayFrequency] (
		    [Old_Id]
		   ,[PayFrequencyName]
           ,[PayFrequencyDescription]
		   ,[HourlyMultiplier]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strPayFrequency]
		   ,[strDescription]
		   ,CASE
				WHEN [strPayFrequency] = 'Daily' OR  [strPayFrequency] = 'Diario' THEN 8    
				WHEN [strPayFrequency] = 'Weekly' OR  [strPayFrequency] = 'Semanal' THEN 40
				WHEN [strPayFrequency] = 'Biweekly' OR  [strPayFrequency] = 'Bisemanal' THEN 80
				WHEN [strPayFrequency] = 'SemiMonthly' OR  [strPayFrequency] = 'Quincenal' THEN 86.67
				WHEN [strPayFrequency] = 'Monthly' OR  [strPayFrequency] = 'Mensual' THEN 173.33
				WHEN [strPayFrequency] = 'Quarterly' OR  [strPayFrequency] = 'Trimestral' THEN 173.33
				WHEN [strPayFrequency] = 'Monthly' OR  [strPayFrequency] = 'Mensual' THEN 173.33
				 
				WHEN [strPayFrequency] = 'Yearly' OR  [strPayFrequency] = 'Annual' THEN 2080.00
				ELSE -1
			END
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblPayFrequency]
	 
END