using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Web.ViewModel
{
    public class DashboardViewModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePicture { get; set; }
        public int UserRecordPeriodTypeId { get; set; }
        public int? UserRecordStatusTypeId { get; set; }
        public string ForecastMonthId { get; set; }
        public int? EmployeeId { get; set; }
        public string EncryptedUserId
        {
            get
            {
                var strId = UserId.ToString();
                return Common.Helpers.Encryption.EncryptURLParm(strId);

            }
        }
        public CompanyStats CompanyStats { get; set; }

    }
    public class DashboardUserWidgetViewModel
    {
        public int NewHireCount { get; set; }
        public int NewInactiveCount { get; set; }
        public int NewClosedCount { get; set; }

        public int NewHirePct { get { return getNewPct(NewHireCount); } }
        public int NewInactivePct { get { return getNewPct(NewInactiveCount); } }
        public int NewClosedPct { get { return getNewPct(NewClosedCount); } }

        public int ActiveUserCount { get; set; }
        public int InactiveUserCount { get; set; }
        public int ClosedRecordCount { get; set; }
        public int ActiveUserPct { get { return getPct(ActiveUserCount); } }
        public int InactiveUserPct { get { return getPct(InactiveUserCount); } }
        public int ClosedRecordPct { get { return getPct(ClosedRecordCount); } }

        public int NewUserCount { get { return ActiveUserCount + InactiveUserCount; } }
        public int HiringRatio { get
            {
                double ratio = 0.0;
                if (ActiveUserCount > 0)
                    ratio = (Convert.ToDouble(NewHireCount) / Convert.ToDouble(ActiveUserCount)) * 100;
                return Convert.ToInt32(ratio);
            }
        }
        public int TurnoverRatio
        {
            get
            {
                double ratio = 0.0;
                if (ActiveUserCount > 0)
                    ratio = (Convert.ToDouble(NewClosedCount) / Convert.ToDouble(ActiveUserCount)) * 100;
                return Convert.ToInt32(ratio);
            }
        }
        private int getPct(int val)
        {
            var total = ActiveUserCount + InactiveUserCount + ClosedRecordCount;
            double pct = 0.0;
            if (total > 0)
            {
                pct = (Convert.ToDouble(val) / Convert.ToDouble(total)) * 100;
            }
            return Convert.ToInt32(pct);
        }
        private int getNewPct(int val)
        {
            var total = NewHireCount + NewInactiveCount + NewClosedCount;
            double pct = 0.0;
            if (total > 0)
            {
                pct = (Convert.ToDouble(val) / Convert.ToDouble(total)) * 100;
            }
            return Convert.ToInt32(pct);
        }
    }
    public class DashboardGenderStatWidgetViewModel
    {
        public int TotalEmployee { get; set; }
        public IList<GenderStatViewModel> GenderStatistics { get; set; }
    }
    public class GenderStatViewModel
    {
       public int Id { get; set; }
        public string Name { get; set; }
        public int GenderCount { get; set; }
        public int TotalCount { get; set; }
        public int GenderRatio { get {
                return getPct();
            } }

        private int getPct()
        {            
            double pct = 0.0;
            if (TotalCount > 0)
            {
                pct = (Convert.ToDouble(GenderCount) / Convert.ToDouble(TotalCount)) * 100;
            }
            return Convert.ToInt32(pct);
        }

    }
        public class DashboardNotificationWidgetViewModel
    {
        public int NotifyingCount { get; set; }
        public int ExpiredCount { get; set; }
        public int NotifyingPct { get { return getPct(NotifyingCount); } }
        public int ExpiredPct { get { return getPct(ExpiredCount); } }
        private int getPct(int val)
        {
            var total = NotifyingCount + ExpiredCount;
            double pct = 0.0;
            if (total > 0)
            {
                pct = (Convert.ToDouble(val) / Convert.ToDouble(total)) * 100;
            }
            return Convert.ToInt32(pct);
        }
    }
    public class DashboardNotificationForecastViewModel
    {
       public int ForecastId { get; set; }
       public string ForecastMonthId { get; set; }
       public string ForecastMonthName { get; set; }
       public int ExpiryRecordCount { get; set; }
    }
    public class DashboardNotificationListViewModel
    {
      public int    NotificationLogId  { get; set; }
      public int    UserInformationId { get; set; }
      public int EmployeeId { get; set; }
      public String EmployeeName { get; set; }
      public DateTime NotificationDate { get; set; }
      public string RecordName { get; set; }
      public string RecordType { get; set; }
      public string RecordStatus { get; set; }
      public DateTime ExpirationDate { get; set; }

    }


}