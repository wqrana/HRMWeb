-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateEmptySourceDefaultInsuranceStatus
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[InsuranceStatus] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT INTO [dbo].[InsuranceStatus] ([InsuranceStatusName],[Description],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Active','Active',@ClientId,1 ,GetDate() ,1)  

		INSERT INTO [dbo].[InsuranceStatus] ([InsuranceStatusName],[Description],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Inactive','Inactive',@ClientId,1 ,GetDate() ,1)  

		INSERT INTO [dbo].[InsuranceStatus] ([InsuranceStatusName],[Description],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Ley Cobra','Ley Cobra',@ClientId,1 ,GetDate() ,1)  
	END
END