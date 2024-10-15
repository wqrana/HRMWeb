CREATE PROCEDURE sp_DM_Degree
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[Degree] (
		    [Old_Id]
		   ,[DegreeName]
           ,[DegreeDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT [intID]
		   ,[strDegree]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,CAST(IsNull([intEnabled],1) AS INT)  
    FROM [TimeAideSource].[dbo].[tblDegree]
END