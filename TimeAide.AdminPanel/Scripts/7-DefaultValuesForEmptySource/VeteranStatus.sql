Create PROCEDURE sp_CreateEmptySourceDefaultVeteranStatus
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[VeteranStatus] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT INTO [dbo].[VeteranStatus] ( [VeteranStatusName],[VeteranStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES( 'Armed Forces Service Medal Veteran','Veteran who, while serving on active duty in the U.S. military, ground, naval or air service, participated in a United States military operation for which an Armed Forces service medal was awarded pursuant to Executive Order 12985 (61 Fed. Reg. 1209) at http://www.opm.gov/veterans/html/vgmedal2.asp',@ClientId,1,GetDate(),1)
		
		INSERT INTO [dbo].[VeteranStatus] ( [VeteranStatusName],[VeteranStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES( 'Other Protected Veteran','Veteran who served on active duty in the U.S. military, ground, naval or air service during a war or in a campaign or expedition for which a campaign badge has been authorized. The information required to make this determination is available at http://www.opm.gov/veterans/html/vgmedal2.htm. A copy of the list also may be obtained by calling (301) 306-6752 and requesting that a copy of the list be mailed to you.',@ClientId,1,GetDate(),1)
		
		INSERT INTO [dbo].[VeteranStatus] ( [VeteranStatusName],[VeteranStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES( 'Recently Separated Veteran','Veteran who served on active duty in the U.S. military, ground, naval or air service during the three-year period beginning on the date of such veteran''s discharge or release from active duty.',@ClientId,1,GetDate(),1)
		
		INSERT INTO [dbo].[VeteranStatus] ( [VeteranStatusName],[VeteranStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES( 'Special Disabled Veteran','(I) Veteran of the U.S. military, ground, naval or air service who is entitled to compensation (or who but for the receipt of military retired pay would be entitled to compensation) under laws administered by the Secretary of Veterans Affairs, or (II) a person who was discharged or released from active duty because of a service-connected disability.',@ClientId,1,GetDate(),1)
		
		INSERT INTO [dbo].[VeteranStatus] ( [VeteranStatusName],[VeteranStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES( 'Vietnam Era Veteran','Veteran who: (I) served on active duty in the U.S. military, ground, naval or air service for a period of more than 180 days, and who was discharged or released there from with other than a dishonorable discharge, if any part of such active duty was performed: (A) in the Republic of Vietnam between February 28, 1961, and May 7, 1975; or (B) between August 5, 1964, and May 7, 1975, in all other cases; or (II) was discharged or released from active duty in the U.S. military, ground, naval or air service for a service-connected disability if any part of such active duty was performed (A) in the Republic of Vietnam between February 28, 1961, and May 7, 1975; or (B) between August 5, 1964, and May 7, 1975, in any other location.',@ClientId,1,GetDate(),1)
	END
END