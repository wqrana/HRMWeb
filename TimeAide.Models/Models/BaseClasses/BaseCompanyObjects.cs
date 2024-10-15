using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeAide.Common.Helpers;

namespace TimeAide.Web.Models
{
    public class BaseCompanyObjects : BaseEntity
    {
        public BaseCompanyObjects() : base()
        {
            CompanyId = SessionHelper.SelectedCompanyId;
        }
        public int? CompanyId { get; set; }
        [Display(Name = "All Companies")]
        [NotMapped]
        public bool IsAllCompanies
        {
            get
            {
                return !CompanyId.HasValue;
            }
        }

        public virtual List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
        //[NotMapped]
        //public bool CanBeAssignedToCurrentCompany { get; set; }


    }
}