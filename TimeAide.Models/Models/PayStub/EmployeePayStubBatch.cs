using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Common.Helpers;

namespace TimeAide.Web.Models
{
    public class EmployeePayStubBatch //: BaseCompanyObjects
    {
        public int? UserInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? OldCompanyId { get; set; }
        public int intUserID { get; set; }
        public string strUserName { get; set; }
        public Guid strBatchID { get; set; }      
        public string strBatchDescription { get; set; }     
        public int intBatchStatus { get; set; }
        public System.DateTime dtPayDate { get; set; }
        public Nullable<decimal> decBatchCompensationAmount { get; set; }
        public Nullable<decimal> decBatchDeductionAmount { get; set; }
        public string strStatusDescription { get; set; }
        public Nullable<decimal> decBatchTransactionHoursAmount { get; set; }
        public int intPayWeekNum { get; set; }
        public string strUserBatchStatus { get; set; }            
        public string sHomeAddressLine1 { get; set; }
        public string sHomeAddressLine2 { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public string strZipCode { get; set; }
        public Nullable<decimal> decBatchUserCompensations { get; set; }
        public Nullable<decimal> decBatchUserWithholdings { get; set; }
        public Nullable<decimal> decBatchNetPay { get; set; }
        public System.DateTime dtStartDatePeriod { get; set; }
        public System.DateTime dtEndDatePeriod { get; set; }
        public Nullable<int> intCompanyID { get; set; }
        public string strCompany { get; set; }       
        public int intPayMethodType { get; set; }
        public string strPayMethodType { get; set; }
        public int intBatchType { get; set; }       
        public int intUserBatchStatus { get; set; }
        public Nullable<int> intDirectDepositCount { get; set; }
        public Nullable<int> intCheckCount { get; set; }
        public string strCompanyName { get; set; }
        public string NetPayInWords { get
            {
                return DecimalToWordExtension.ToWords(decBatchNetPay??0);
            } }
       
    }
    public class EmployeePayStubCompany
    {
        public string strAddress1 { get; set; }
        public string strAddress2 { get; set; }
        public string strCity { get; set; }
        public string strState { get; set; }
        public string strZipCode { get; set; }
        public byte[] CompanyLogo { get; set; }
        public string CompanyLogoStr { get
                {
                    string base64Str = "";
                    if (CompanyLogo!=null && CompanyLogo.Length > 0)
                    {
                        base64Str = Convert.ToBase64String(CompanyLogo);
                    }
                    return base64Str;
                } }
    }
}
