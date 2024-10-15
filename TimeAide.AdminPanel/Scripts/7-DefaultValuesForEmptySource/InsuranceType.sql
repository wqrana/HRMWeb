CREATE PROCEDURE sp_CreateEmptySourceDefaultInsuranceType
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[InsuranceType] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT INTO [dbo].[InsuranceType] ([InsuranceTypeName],[Description],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Primary','Primary',@ClientId,1,GetDate(),1)  

		INSERT INTO [dbo].[InsuranceType] ([InsuranceTypeName],[Description],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Secondary','Secondary',@ClientId,1,GetDate(),1)  
	END
END