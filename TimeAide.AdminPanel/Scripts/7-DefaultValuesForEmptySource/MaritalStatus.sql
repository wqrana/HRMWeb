CREATE PROCEDURE sp_CreateEmptySourceDefaultMaritalStatus
	@ClientId int
AS
BEGIN
	IF Not EXISTS (SELECT * FROM [dbo].[MaritalStatus] WHERE [ClientId] = @ClientId)
	BEGIN
		INSERT INTO [dbo].[MaritalStatus] ([MaritalStatusName],[MaritalStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES('Single','Single' ,@ClientId ,1 ,GetDate() ,1 )

		INSERT INTO [dbo].[MaritalStatus] ([MaritalStatusName],[MaritalStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES('Married','Married' ,@ClientId ,1 ,GetDate() ,1 )

		INSERT INTO [dbo].[MaritalStatus] ([MaritalStatusName],[MaritalStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES('Divorced','Divorced' ,@ClientId ,1 ,GetDate() ,1 )

		INSERT INTO [dbo].[MaritalStatus] ([MaritalStatusName],[MaritalStatusDescription],[ClientId],[CreatedBy],[CreatedDate],[DataEntryStatus])
		VALUES('Widower','Widower' ,@ClientId ,1 ,GetDate() ,1 )
	END     
END