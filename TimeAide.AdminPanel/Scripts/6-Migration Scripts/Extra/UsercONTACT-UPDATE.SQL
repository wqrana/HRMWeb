Update [dbo].[UserContactInformation] 
			
			Set HomeAddress1 = NULLIF(LTRIM(RTRIM(HomeAddress1)),'')	,
			HomeAddress2=NULLIF(LTRIM(RTRIM(HomeAddress2)),'')	,
			HomeZipCode=NULLIF(LTRIM(RTRIM(HomeZipCode)),'')	,
			MailingAddress1=NULLIF(LTRIM(RTRIM(MailingAddress1)),'')	,
			MailingAddress2=NULLIF(LTRIM(RTRIM(MailingAddress2)),'')	,
			MailingZipCode=NULLIF(LTRIM(RTRIM(MailingZipCode)),'')	,
			HomeNumber = NULLIF(LTRIM(RTRIM(HomeNumber)),'')	,
			CelNumber=NULLIF(LTRIM(RTRIM(CelNumber)),'')	,
			FaxNumber=NULLIF(LTRIM(RTRIM(FaxNumber)),'')	,
			OtherNumber=NULLIF(LTRIM(RTRIM(OtherNumber)),'')	,
			WorkEmail=NULLIF(LTRIM(RTRIM(WorkEmail)),'')	,
			PersonalEmail=NULLIF(LTRIM(RTRIM(PersonalEmail)),'')	,
			OtherEmail=NULLIF(LTRIM(RTRIM(OtherEmail)),'')	,
			WorkNumber=NULLIF(LTRIM(RTRIM(WorkNumber)),'')	,
			WorkExtension = NULLIF(LTRIM(RTRIM(WorkExtension)),'')	