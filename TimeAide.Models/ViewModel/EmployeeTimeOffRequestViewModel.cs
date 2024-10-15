using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TimeAide.Models.ViewModel
{
   public class EmployeeTimeOffRequestViewModel
    {
     public int EmployeeTimeOffRequestId { get; set; }
     public int ClientId { get; set; }
     public string ProcessType { get; set; }
    }
    public class ChangeRequestViewModel
    {
        public int Id { get; set; }
        public int UserInformationId { get; set; }
        public int ChangeRequestStatusId { get; set; }
        public string ChangeRequestRemarks { get; set; }
        //public abstract Dictionary<String, String> CanCreateRequest();
        public int ReferenceId { get; set; }
        public int ClientId { get; set; }
        public string RequestType { get; set; }
        public string ProcessType { get; set; }
    }
    public class EmployeeTimeOffRequestDocumentViewModel
    {
        public int EmployeeTimeOffRequestId { get; set; }
        public int EmployeeTimeOffDocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string SubmissionType { get; set; }
        public int? TimeoffDays { get; set; }
        public string Status { get; set; }

        public string DocumentFile1Name { get; set; }
        public string DocumentFile1Action { get; set; }
        public HttpPostedFileBase DocumentFile1 { get; set; }

        public string DocumentFile2Name { get; set; }
        public string DocumentFile2Action { get; set; }
        public HttpPostedFileBase DocumentFile2 { get; set; }

        public string DocumentFile3Name { get; set; }
        public string DocumentFile3Action { get; set; }
        public HttpPostedFileBase DocumentFile3 { get; set; }
    }

    public class TimeOffCalendarViewModel 
    { 
        public DateTime CalendarStartDate { get;set; }
        public DateTime CalendarEndDate { get;set; }       
        public string CalenderMonths { get; set; }
    
    }

    public class TimeOffRenderMonthViewModel
    {
        public int UserInformationId { get; set; }
        public int EmployeeId { get; set; }
        public string AccrualType { get; set; }
        public string TransType { get; set; }
        public DateTime TimeOffStartDate { get; set; }
        public DateTime TimeOffEndDate { get; set;}
        public decimal TimeOffDayHours { get; set; }
        public int RenderYear { get; set; }
        public int RenderMonth { get; set; }
        public string ReferenceIds { get; set; }
        public bool IsMoneyTrans { get;set; }
        public string TransNote { get;set; }
        public string CurrentAccrualType { get; set; }
        public decimal CurrentDayHours { get; set;}
    }
    public class TimeOffCalendarMonthViewModel
    {
        public int CalendarMonth { get; set; }
        public string DisplayName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IList<TimeOffCalendarDayViewModel> CalendarMonthDays { get; set; }
    }
    public class UserScheduledHolidayViewModel
    {
        public DateTime Holiday { get; set; }
        
    }
    public class UserDailyBalanceViewModel
    {
       
        public int Id { get; set; }
        public string AccrualType { get; set; }
        public DateTime BalanceDate { get; set; }
        public decimal Balance { get; set; }
        public decimal OpeningBalance { get; set; } 
        public decimal Available { get; set; }  
    }
    public class TimeOffCalendarDayViewModel
    {
        public DateTime DayDate { get; set; }
              
        public string DisplayName { get; set; }

        public bool IsSelected { get;set; }

        public bool IsCurrentMonthDay { get; set; }
        public bool IsToday { get; set; }
        public bool IsHoliday { get; set; } 
        public string Event1 { get;set; }//heading,value,status
        public string Event2 { get; set; }
        public string Event3 { get; set; }


    }
    
}
