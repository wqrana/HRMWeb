/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [CityId]
      ,[CityCode]
      ,[CityName]
      ,[CityDescription]
      ,[StateId]
      ,[ClientId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[DataEntryStatus]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[Old_Id]
  FROM [TimeAideCSIF10].[dbo].[City] where cityname like '%Grande%'  --Naranjito