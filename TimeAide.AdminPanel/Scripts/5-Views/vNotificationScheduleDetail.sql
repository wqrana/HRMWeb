CREATE VIEW [dbo].[vNotificationScheduleDetail]
AS
SELECT        [NotificationScheduleDetailId], [NotificationScheduleId], [DaysBefore], LAG([DaysBefore], 1, 0) OVER (PARTITION BY [NotificationScheduleId]
ORDER BY [DaysBefore]) AS PreviousDaysBefore, [CreatedBy], [CreatedDate], [DataEntryStatus], [ModifiedBy], [ModifiedDate], [ClientId], [NotificationMessageId]
FROM            [NotificationScheduleDetail]