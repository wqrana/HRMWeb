CREATE PROCEDURE sp_DM_AppraisalRatingScale
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalRatingScale] (
			 [Old_Id]
			,[ScaleName]
			,[ScaleDescription]
			,[ScaleMaxValue]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   [intID]
			,[strScaleName]
			,''
			,0
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
	 
END