
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    public class EmployeePayStubCompensation 
    {
        public string strBatchID { get; set; }
        public Nullable<int> intUserID { get; set; }
        public string strCompensationName { get; set; }
        public Nullable<decimal> decHours { get; set; }
        public Nullable<decimal> decPay { get; set; }
        public string strGLAccount { get; set; }
        public Nullable<decimal> decPayYTD { get; set; }
        public Nullable<int> intReportOrder { get; set; }
        public Nullable<decimal> decPayRate { get; set; }

    }
}
