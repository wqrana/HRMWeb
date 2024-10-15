CREATE PROCEDURE sp_DM_IncidentType
AS
BEGIN 
	
INSERT [dbo].[IncidentType] ([IncidentTypeName], [IncidentTypeDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) 
VALUES (N'CFSE', N'CFSE', 1, GetDate(), 1, NULL, NULL, 1,1);
INSERT [dbo].[IncidentType] ([IncidentTypeName], [IncidentTypeDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) 
VALUES (N'OSHA', N'OSHA', 1, GetDate(), 1, NULL, NULL, 1,2);
INSERT [dbo].[IncidentType] ([IncidentTypeName], [IncidentTypeDescription], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [Old_Id]) 
VALUES (N'OSHA/CFSE', N'OSHA/CFSE', 1, GetDate(), 1, NULL, NULL, 1,3);
	 
END