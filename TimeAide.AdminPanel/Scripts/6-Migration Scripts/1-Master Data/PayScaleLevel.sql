CREATE PROCEDURE sp_DM_PayScaleLevel
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[PayScaleLevel]
           (
			[PayScaleId]
           ,[PayScaleLevelRate]
           ,[ClientId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[DataEntryStatus]
           
           ,[Old_Id])
     
	SELECT  
			 (select top(1) PayScaleId from PayScale where PayScale.[Old_Id] = intID And ClientId = @ClientId) as intPayScaleId
			,PayScaleLevelRate 
			,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
			,intID
	FROM(
		SELECT * FROM [TimeAideSource].[dbo].[tblPayScales]) p  
		UNPIVOT (PayScaleLevelRate FOR PayScaleLevelCount IN   
					([decLevel01] ,[decLevel02] ,[decLevel03] ,[decLevel04] ,[decLevel05] ,[decLevel06] ,[decLevel07] ,[decLevel08],[decLevel09],[decLevel10]
					,[decLevel11] ,[decLevel12] ,[decLevel13] ,[decLevel14] ,[decLevel15] ,[decLevel16] ,[decLevel17] ,[decLevel18],[decLevel19],[decLevel20]
					,[decLevel21]	,[decLevel22] ,[decLevel23] ,[decLevel24] ,[decLevel25] ,[decLevel26] ,[decLevel27] ,[decLevel28],[decLevel29],[decLevel30])  
	)AS unpvt;  
	 
END