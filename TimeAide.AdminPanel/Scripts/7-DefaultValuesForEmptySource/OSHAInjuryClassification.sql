---- =============================================
---- Author:		Salman
---- Create date: 20-Aug-2020
---- Description:	CreateClient
---- =============================================
--CREATE PROCEDURE sp_CreateEmptySourceDefaultOSHAInjuryClassification
--	@ClientId int
--AS
--BEGIN
--	IF Not EXISTS (SELECT * FROM [dbo].[OSHAInjuryClassification] WHERE [ClientId] = @ClientId)
--	BEGIN
--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('All other illnesses','All other illnesses',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Hearing loss','Hearing loss',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Injury','Injury',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Poisoning','Poisoning',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Respiratory condition','Respiratory condition',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHAInjuryClassification] ([OSHAInjuryClassificationName],[OSHAInjuryClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Skin disorder','Skin disorder',@ClientId,1,GetDate(),1)
--	END
--END




