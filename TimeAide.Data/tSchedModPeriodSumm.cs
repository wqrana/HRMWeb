//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeAide.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tSchedModPeriodSumm
    {
        public int ID { get; set; }
        public Nullable<int> nUserID { get; set; }
        public string sUserName { get; set; }
        public string sWeekID { get; set; }
        public string sNote { get; set; }
        public Nullable<System.DateTime> dStartDate { get; set; }
        public Nullable<System.DateTime> dEndDate { get; set; }
        public Nullable<double> dblPeriodHours { get; set; }
        public Nullable<int> nSupervisorID { get; set; }
        public Nullable<System.DateTime> dModifiedDate { get; set; }
        public Nullable<int> nPayPeriodType { get; set; }
        public Nullable<int> nPayWeekNum { get; set; }
    }
}
