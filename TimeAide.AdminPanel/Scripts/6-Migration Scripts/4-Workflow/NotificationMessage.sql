-- =============================================
-- Author:		Salman
-- Create date: 20-Aug-2020
-- Description:	CreateClient
-- =============================================
CREATE PROCEDURE sp_CreateDefaultNotificationMessage
	@ClientId int
AS
BEGIN
	
SET IDENTITY_INSERT [dbo].[NotificationMessage] ON 
INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (1, N'Change Request IPPR - Employee Supervisor', N'Change Request IPPR - Employee Supervisor', N'Hi,
<br\>
[ShortFullName] has requested to modify information, All Employee Supervisors are requested to review the ticket initiated by the employee. Please visit the following link to review the requested changes [URL].

Thanks', 2, @ClientId, 1, CAST(N'2021-03-08 14:10:40.310' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (2, N'Change Request IPPR - HR Employee Group', N'Change Request IPPR - HR Employee Group', N'Hi,
<br\>
HR Admis:
<br\>
[ShortFullName] has requested to modify information, Requested has been approved by employee supervisors. HR group is requested to review the ticket approved by the supervisor.

Please visit the following link to review the requested changes [URL].

Thanks,', 2, @ClientId, 1, CAST(N'2021-03-08 14:11:27.500' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (3, N'Change Request IPPR - Admin Group', N'Change Request IPPR - Admin Group', N'Hi,
<br\>
Management Group:
<br\>
[ShortFullName] has requested to modify information, Requested has been approved by employee supervisors and HR group, Admin group is requested to review the ticket approved by the supervisor and HR group.

Please visit the following link to review the requested changes [URL].

Thanks,', 2, @ClientId, 1, CAST(N'2021-03-08 14:11:58.983' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (4, N'Closing Change Request Message', N'Closing Change Request Message', N'Hi All,
<br\\>

[ShortFullName] had requested to modify information, Requested has been taken appropriate action by supervisors HR group, and Admin group. This ticket is being closed.
changes [URL].

Thanks,', 2, @ClientId, 1, CAST(N'2021-03-08 14:14:13.057' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (5, N'No Approval Needed', N'For No Approval Needed Message', N'Dear [ShortFullName]:
Your request has been made.
[FirstName]
[MiddleInitial]
[FirstLastName]
[SecondLastName]
[ShortFullName]
[DaysBefore]
[RecordName]
[CompanyName]
[Department]
[SubDepartment]
[EmployeeType]
[EmploymentStatus]
[PositionId]
[URL]
[LoginUserName]', 13, @ClientId, 1, CAST(N'2021-04-09 14:30:56.237' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (6, N'Supervisor Approval', N'Message for supervisor approval', N'Your change request has been approved.
[ShortFullName]
[URL]', 13, @ClientId, 1, CAST(N'2021-04-09 14:42:35.580' AS DateTime), 1, NULL, NULL, NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (7, N'Expiration Message', NULL, N'Dear [FirstLastName],

                                       Your [RecordName] has expired today.

Thanks,
', 2, @ClientId, 1, CAST(N'2020-06-16 18:58:20.640' AS DateTime), 1, 1, CAST(N'2020-06-16 18:58:20.640' AS DateTime), NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (8, N'Expiration Message NEW', NULL, N'document HAS EXPIRED, TAKE APPROPRIATE ACTION. [FirstName]  [RecordName]   ', 2, @ClientId, 1, CAST(N'2020-06-15 19:09:46.123' AS DateTime), 1, 1, CAST(N'2020-06-15 19:09:46.123' AS DateTime), NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (9, N'Type For High Alert', N'New Type', N'Immidiate action needed, Driving licence will expire in [DaysBefore]', 2, @ClientId, 1, CAST(N'2020-06-03 00:10:46.740' AS DateTime), 1, 1, CAST(N'2020-06-03 00:10:46.740' AS DateTime), NULL)

INSERT [dbo].[NotificationMessage] ([NotificationMessageId], [NotificationMessageName], [NotificationMessageDescription], [Message], [CompanyId], [ClientId], [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [Old_Id]) VALUES (10, N'For First alert', N'new', N'Driving licence will expire in [DaysBefore]   [RecordName]  ', 2, @ClientId, 1, CAST(N'2020-06-03 00:11:19.577' AS DateTime), 1, 1, CAST(N'2020-06-03 00:11:19.577' AS DateTime), NULL)

SET IDENTITY_INSERT [dbo].[NotificationMessage] OFF

END
