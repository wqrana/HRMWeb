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
    
    public partial class tAuditInfo
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DTTimeStamp { get; set; }
        public Nullable<int> nAdminID { get; set; }
        public string sAdminName { get; set; }
        public string sAdminAction { get; set; }
        public string sRecordAffected { get; set; }
        public Nullable<int> nUserIDAffected { get; set; }
        public string sUserNameAffected { get; set; }
        public string sFieldName { get; set; }
        public string PrevValue { get; set; }
        public string NewValue { get; set; }
        public Nullable<long> nWeekID { get; set; }
        public string sNote { get; set; }
    }
}
