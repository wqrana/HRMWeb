-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateEmptySourceDefaultOSHAInjuryClassification
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[OSHAInjuryClassification] WHERE [ClientId] = 1)
	BEGIN
		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('All other illnesses','All other illnesses',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Hearing loss','Hearing loss',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Injury','Injury',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Poisoning','Poisoning',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Respiratory condition','Respiratory condition',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Skin disorder','Skin disorder',1,1,GetDate(),1)
	END
END




