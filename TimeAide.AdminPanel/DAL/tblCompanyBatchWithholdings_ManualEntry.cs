//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeAide.AdminPanel.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblCompanyBatchWithholdings_ManualEntry
    {
        public System.Guid strBatchID { get; set; }
        public int intUserID { get; set; }
        public string strWithHoldingsName { get; set; }
        public System.DateTime dtPayDate { get; set; }
        public decimal decBatchEffectivePay { get; set; }
        public decimal decWithholdingsAmount { get; set; }
        public int intPrePostTaxDeduction { get; set; }
        public string strGLAccount { get; set; }
        public bool boolDeleted { get; set; }
        public int intSequenceNumber { get; set; }
        public int intSupervisorID { get; set; }
        public string strNote { get; set; }
        public System.DateTime dtTimeStamp { get; set; }
        public Nullable<int> intEditType { get; set; }
        public string strGLContributionPayable { get; set; }
    }
}
