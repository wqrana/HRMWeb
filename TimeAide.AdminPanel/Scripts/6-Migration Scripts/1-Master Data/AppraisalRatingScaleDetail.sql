CREATE PROCEDURE sp_DM_AppraisalRatingScaleDetail
	@ClientId int
AS
BEGIN
	
	INSERT INTO [dbo].[AppraisalRatingScaleDetail] (
			 [Old_Id]
			,[AppraisalRatingScaleId]
			,[RatingLevelId]
			,[RatingName]
			,[RatingDescription]
			,[RatingAbbreviation]
			,[RatingValue]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus])
    SELECT   [intID]
			,(select top(1) intID from AppraisalRatingScale where Old_Id = intID And ClientId = @ClientId) as intID 
			,RatingLevelId
			,RatingName
			,RatingDescription
			,RatingAbbreviation
			,RatingValue
		    ,@ClientId
		    ,1
		    ,GetDate()
		    ,1  
    FROM 
		(
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='0' And ClientId = @ClientId) as RatingLevelId, strRating0Name as RatingName,strRating0Description as RatingDescription,strRating0Abbreviation as RatingAbbreviation,intRating0Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
			Union All
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='1' And ClientId = @ClientId) as RatingLevelId, strRating1Name as RatingName,strRating1Description as RatingDescription,strRating1Abbreviation as RatingAbbreviation,intRating1Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
			Union All
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='2' And ClientId = @ClientId) as RatingLevelId, strRating2Name as RatingName,strRating2Description as RatingDescription,strRating2Abbreviation as RatingAbbreviation,intRating2Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
			Union All
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='3' And ClientId = @ClientId) as RatingLevelId, strRating3Name as RatingName,strRating3Description as RatingDescription,strRating3Abbreviation as RatingAbbreviation,intRating3Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
			Union All
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='4' And ClientId = @ClientId) as RatingLevelId, strRating4Name as RatingName,strRating4Description as RatingDescription,strRating4Abbreviation as RatingAbbreviation,intRating4Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
			Union All
			SELECT intID,(Select top(1) RatingLevelId from [dbo].[RatingLevel] Where [RatingLevelName]='5' And ClientId = @ClientId) as RatingLevelId, strRating5Name as RatingName,strRating5Description as RatingDescription,strRating5Abbreviation as RatingAbbreviation,intRating5Value as RatingValue FROM [TimeAideSource].[dbo].[tblAppraisalRatingScales]
		
		) tblAppraisalRatingScale
	 
END