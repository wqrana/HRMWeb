using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
    public class TimesheetEditorPunchDateViewModel
    {
        public DateTime punchDate { get; set; }
        public int? e_id { get; set; }
        public string e_name { get; set; }
        public long? nWeekID { get; set; }
        public int? tpdID { get; set; }
        public int schId { get;set; }   
        public int schDetailId { get; set; }
        public int e_companyId { get; set; }
    }
    public class TimesheetEditorPunchDateEditViewModel
    {
        public int? tpdID { get; set; }
        public DateTime PunchDate { get; set; }
        public int? e_id { get; set; }
        public string e_name { get; set; }
        public long? nWeekID { get; set; }       
        public int? nSchedID { get; set; }
        public int nSchedDetailID { get; set; } 
        public string sPunchSummary { get; set; }
        public string sExceptions { get; set; }
        public string sDaySchedule { get; set; }
        public string sHoursSummary { get; set; }
        public IEnumerable<TimesheetEditorTransactionViewModel> TransactionData { get; set; }
        public IEnumerable<TimesheetEditorPunchDataViewModel> PunchData { get; set; }
                
    }
    public class TimesheetEditorPunchesViewModel
    {
        public string ReferenceIds { get; set; }
        public int PunchCount { get; set; }
        public DateTime PunchIn1 { get; set; }
        public bool PunchIn1Overnight { get;set; }
        public DateTime PunchOut1 { get; set; }
        public bool PunchOut1Overnight { get; set; }
        public DateTime PunchIn2 { get; set; }
        public bool PunchIn2Overnight { get; set; }
        public DateTime PunchOut2 { get; set; }
        public bool PunchOut2Overnight { get; set; }       
        public string Note { get;set; } 
      

    }
    public class TimesheetEditorPunchDetailViewModel
    {
        public int e_id { get;set; }
        public string e_name { get; set; }
        public long nWeekID { get; set; }
        public DateTime PunchDate { get; set; }
        public IEnumerable<dynamic> PunchData {get; set;}

    }
        public class TimesheetEditorTransactionViewModel {
        public int tppID { get; set; }
        public int? e_id { get; set; }
        public string e_name { get; set; }
        public DateTime? DTPunchDate { get; set; }       
        public double? HoursWorked { get; set; }
        public string sType { get; set; }
        public string sTDesc { get; set; }
        public string pCode { get; set; }
        public bool? b_Processed { get; set; }       
        public bool? bTrans { get; set; }       
        public long? nWeekID { get; set; }
        public int? nIsMoneyTrans { get; set; }
       

    }
    public class TimesheetEditorPunchDataViewModel
    {
        public int referenceId { get; set; }
        public int tpdID { get; set; }
        public DateTime DTPunchDate { get; set; }
        public DateTime DTPunchDateTime { get; set; }       
        public Nullable<int> e_id { get; set; }
        public string e_name { get; set;}
        public bool isOvernight { get; set; }
        public string sNote { get; set; }
    }
   
        public class TransactionResultViewModel
    {
        public int e_id { get; set; }
        public string e_name { get; set; }
        public DateTime DTPunchDate { get; set; }
        public Int64 nWeekID { get; set; }
        public string sType { get; set; }
        public double? HoursWorked { get; set; }

    }
}
