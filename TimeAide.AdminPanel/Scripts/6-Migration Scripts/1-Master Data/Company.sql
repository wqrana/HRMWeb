CREATE PROCEDURE sp_DM_Company
	@ClientId int
AS
BEGIN
	
	
	INSERT INTO [dbo].[Company](
			 [Old_Id]
			,[CompanyName]
			,[CompanyDescription]
			,[ParentCompany]
			,[DisplayName]
			,[Address1]
			,[Address2]
			,[CityId]
			,[StateId]
			,[ZipCode]
			,[ContactName]
			,[ContactTelephone]
			,[ContactEmail]
			,[ContactPosition]
			,[NAICS]
			,[DUNS]
			,[EmployerID]
			,[NameInLetters]
			,[SIC]
			,[CompanyLogo]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
								)
	SELECT   [intID]
			,[strCompany]
			,[strDescription]
			,[strParentCompany]
			,[strName]
			,[strAddress1]
			,[strAddress2]
			,IsNull((select top(1) CityId from City where CityName = strCity),(select top(1) CityId from City where ClientId = @ClientId)) as strCity
			,IsNull((select top(1) StateId from City where CityName = strCity),(select top(1) CityId from City where ClientId = @ClientId)) as strState
			,[strZipCode]
			,[strContactName]
			,[strContactTelephone]
			,[strContactEmail]
			,[strContactPosition]
			,[strNAICS]
			,[strDUNS]
			,[strEmployerID]
			,[strNameInLetters]
			,[strSIC]
			,[binCompanyLogo]
			,@ClientId
			,1
			,GetDate()
			,CAST(IsNull([intEnabled],1) AS INT)  
	FROM [TimeAideSource].[dbo].[tblCompany]

	IF NOT EXISTS (SELECT * FROM [dbo].[Company] WHERE CompanyName = 'None-Company' And [ClientId]=@ClientId)
	BEGIN
		INSERT [dbo].[Company] ([CompanyName],[ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus]) VALUES (N'None-Company', @ClientId, 1, GetDate(), 1);
	END
END	 
