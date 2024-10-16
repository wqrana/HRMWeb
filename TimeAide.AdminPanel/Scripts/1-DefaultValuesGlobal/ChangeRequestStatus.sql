-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultChangeRequestStatus
AS
BEGIN
	
SET IDENTITY_INSERT [dbo].[ChangeRequestStatus] ON 
Insert into changerequeststatus([ChangeRequestStatusId], [ChangeRequestStatusName], [Description], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id])
values(1,'In Progress',	NULL,1,1,'2021-01-01 00:00:00.000',1,NULL,NULL,NULL)
Insert into changerequeststatus([ChangeRequestStatusId], [ChangeRequestStatusName], [Description], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id])
values(2,'Approved',	NULL,1,1,'2021-01-01 00:00:00.000',1,NULL,NULL,NULL)
Insert into changerequeststatus([ChangeRequestStatusId], [ChangeRequestStatusName], [Description], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id])
values(3,'Declined',	NULL,1,1,'2021-01-01 00:00:00.000',1,NULL,NULL,NULL)
Insert into changerequeststatus([ChangeRequestStatusId], [ChangeRequestStatusName], [Description], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id])
values(4,'Cancelled',	NULL,1,1,'2021-01-01 00:00:00.000',1,NULL,NULL,NULL)
Insert into changerequeststatus([ChangeRequestStatusId], [ChangeRequestStatusName], [Description], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id])
values(5,'Auto Cancelled',	NULL,1,1,'2021-01-01 00:00:00.000',1,NULL,NULL,NULL)
SET IDENTITY_INSERT [dbo].[ChangeRequestStatus] OFF 


END




