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
    
    public partial class tSchedule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string sDesc { get; set; }
        public Nullable<int> nPunchNum { get; set; }
        public Nullable<int> nPunchCode { get; set; }
        public Nullable<bool> bSunday { get; set; }
        public Nullable<bool> bMonday { get; set; }
        public Nullable<bool> bTuesday { get; set; }
        public Nullable<bool> bWednesday { get; set; }
        public Nullable<bool> bThursday { get; set; }
        public Nullable<bool> bFriday { get; set; }
        public Nullable<bool> bSaturday { get; set; }
        public Nullable<int> nCompanyID { get; set; }
        public Nullable<int> nShiftDiff { get; set; }
        public string sShiftDiffName { get; set; }
        public string sShiftDiffCode { get; set; }
        public Nullable<int> nShiftDiffCount { get; set; }
        public Nullable<System.DateTime> dShiftDiffStart { get; set; }
        public Nullable<System.DateTime> dShiftDiffEnd { get; set; }
        public string sShiftDiffName2 { get; set; }
        public string sShiftDiffCode2 { get; set; }
        public Nullable<System.DateTime> dShiftDiffStart2 { get; set; }
        public Nullable<System.DateTime> dShiftDiffEnd2 { get; set; }
    }
}
