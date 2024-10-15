/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [StateId]
      ,[StateCode]
      ,[StateName]
      ,[StateDescription]
      ,[CountryId]
      ,[ClientId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[DataEntryStatus]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[Old_Id]
  FROM [TimeAideCSIF10].[dbo].[State] where CountryId=2 order by StateName