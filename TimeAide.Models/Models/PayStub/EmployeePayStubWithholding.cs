
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    public class EmployeePayStubWithholding
    {
        public string strBatchID { get; set; }
        public Nullable<int> intUserID { get; set; }
        public string strWithHoldingsName { get; set; }
        public Nullable<decimal> decWithholdingsAmount { get; set; }
        public Nullable<decimal> decBatchEffectivePay { get; set; }
        public string strGLAccount { get; set; }
        public Nullable<decimal> decWithholdingsYTD { get; set; }
        public Nullable<int> intReportOrder { get; set; }

    }
}
