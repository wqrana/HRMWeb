CREATE PROCEDURE sp_DM_Benefit
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[Benefit] (
		    [Old_Id]
		   ,[BenefitName]
           ,[BenefitDescription]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus])
    SELECT  [intID]
		   ,[strBenefitName]
		   ,[strDescription]
		   ,@ClientId
		   ,1
		   ,GetDate()
		   ,1  
     FROM [TimeAideSource].[dbo].[tblBenefitList]
	 
END