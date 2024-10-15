using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
    {
        public class TAWindowTimeOffTransaction : BaseEntity
        {

        }
        public class TAWinTimeOffDocument : BaseEntity
        { 
        public int intTransId { get; set; }
        public string strTransName { get; set; }
        public string sDocumentName { get; set; }
        public int intDocumentType { get; set; }
        public int intSubmissionType { get; set; }
        public int intTimeOffDays { get; set; }
        }
       public class TAWinTransDefTimeOff 
    {
        public int TransId { get; set; }
        [Display(Name = "Trans. Name")]
        public string TransName { get; set; }       
         public string Description { get; set; }
        [Display(Name = "Accrual Type")]
        public string AccrualType { get; set; }
        [Display(Name = "Time-Off Trans.")]
        public bool IsTimeOffTrans { get; set; }

        public bool IsSickInFamilyTrans {get; set; }
        public decimal? MinimumAccrualTypeBalance { get; set; }
        public decimal? MaximumYearlyTaken { get; set; }


    }
}
