CREATE PROCEDURE sp_DM_Location
	@ClientId int
AS
BEGIN
	

	INSERT INTO [dbo].[Location] (
		     [Old_Id]
			,[LocationName]
			,[LocationDescription]
			,[CompanyId]
			,[Address]
			,[LocationCityId]
			,[LocationStateId]
			,[ZipCode]
			,[Address2]
			,[PhysicalAddress1]
			,[PhysicalAddress2]
			,[PhysicalCityId]
			,[PhysicalStateId]
			,[PhysicalZipCode]
			,[PhoneNumber1]
			,[ExtensionNumber1]
			,[PhoneNumber2]
			,[ExtensionNumber2]
			,[PhoneNumber3]
			,[ExtensionNumber3]
			,[FaxNumber]
			,[ClientId]
			,[CreatedBy]
			,[CreatedDate]
			,[DataEntryStatus]
			)
     SELECT  [intID]
			,[strLocation]
			,[strDescription]
			,[intCompany]
			,[strAddress]
			,IsNull((select top(1) CityId from City where CityName = strCity And ClientId = @ClientId),(select top(1) CityId from City where ClientId = @ClientId)) as strCity
			,IsNull((select top(1) StateId from City where CityName = strCity And ClientId = @ClientId),(select top(1) StateId from [State] where ClientId = @ClientId)) as strState
			,[strZipCode]
			,[strAddress2]
			,[strPhysicalAddress1]
			,[strPhysicalAddress2]
			,IsNull((select top(1) CityId from City where CityName = strPhysicalCity And ClientId = @ClientId),(select top(1) CityId from City where ClientId = @ClientId)) as strPhysicalCity
			,IsNull((select top(1) StateId from City where CityName = strPhysicalState And ClientId = @ClientId),(select top(1) StateId from [State] where ClientId = @ClientId)) as strPhysicalState
			,[strPhysicalZipCode]
			,[strPhoneNumber1]
			,[strExtensionNumber1]
			,[strPhoneNumber2]
			,[strExtensionNumber2]
			,[strPhoneNumber3]
			,[strExtensionNumber3]
			,[strFaxNumber]
			,@ClientId
			,1
			,GetDate()
			,CAST(IsNull([intEnabled],1) AS INT
			)  
     FROM [TimeAideSource].[dbo].[tblLocation]
	 
END