using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    public class CompensationBalance //: BaseCompanyObjects
    {
        public int? UserInformationId { get; set; }
        public int? EmployeeId { get; set; }            
        public string EmployeeName { get; set; }
        public int UserID { get; set; }
        public string TransType { get; set; }
        public string AccrualType { get; set; }
        public decimal? Balance { get; set; }
        public decimal? RemainingBalance { get; set; }
        public DateTime? SearchDate { get; set; }
    }
}
