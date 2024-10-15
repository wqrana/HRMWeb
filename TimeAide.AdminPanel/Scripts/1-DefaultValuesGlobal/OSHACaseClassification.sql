CREATE PROCEDURE sp_CreateEmptySourceDefaultOSHACaseClassification
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[OSHACaseClassification] WHERE [ClientId] = 1)
	BEGIN
		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Days away from work','Days away from work',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Death','Death',1,1,GetDate(),1)

		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Job transfer or restriction','Job transfer or restriction',1,1,GetDate(),1)
		
		INSERT INTO [dbo].[OSHACaseClassification] ([OSHACaseClassificationName],[OSHACaseClassificationDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES ('Other recordable cases','Other recordable cases',1,1,GetDate(),1)
	END
END