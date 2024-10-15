--CREATE PROCEDURE sp_CreateEmptySourceDefaultOSHACaseClassification
--	@ClientId int
--AS
--BEGIN
--	IF Not EXISTS (SELECT * FROM [dbo].[OSHACaseClassification] WHERE [ClientId] = @ClientId)
--	BEGIN
--		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Days away from work','Days away from work',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Death','Death',@ClientId,1,GetDate(),1)

--		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Job transfer or restriction','Job transfer or restriction',@ClientId,1,GetDate(),1)
		
--		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
--		VALUES ('Other recordable cases','Other recordable cases',@ClientId,1,GetDate(),1)
--	END
--END