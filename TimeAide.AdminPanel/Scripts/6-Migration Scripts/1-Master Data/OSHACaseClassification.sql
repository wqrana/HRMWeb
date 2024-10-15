CREATE PROCEDURE sp_DM_OSHACaseClassification
	@ClientId int
AS
BEGIN
	
INSERT INTO [dbo].[OSHACaseClassification] (
		    [Old_Id]
		   ,[OSHACaseClassificationName]
           ,[OSHACaseClassificationDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
     SELECT [intID]
		   ,[strIncidentClassification]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)
     FROM [TimeAideSource].[dbo].[tblIncidentOSHAClassification]
END