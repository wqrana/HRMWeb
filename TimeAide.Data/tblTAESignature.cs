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
    
    public partial class tblTAESignature
    {
        public string intTAESignID { get; set; }
        public int intUserID { get; set; }
        public string strUserName { get; set; }
        public int intMonth { get; set; }
        public int intYear { get; set; }
        public Nullable<System.DateTime> dtEmployeeDateTime { get; set; }
        public string strEmployeeEntry { get; set; }
        public Nullable<bool> bitEmployeeSigned { get; set; }
        public Nullable<System.DateTime> dtSupervisorDateTime { get; set; }
        public string strSupervisorEntry { get; set; }
        public Nullable<bool> bitSupervisorSigned { get; set; }
        public Nullable<int> intSupervisorID { get; set; }
    }
}
