using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeAide.Web.ViewModel
{
    public class CompanyAnnouncementViewModel
    {
      public int? CompanyAnnouncementId { get; set; }
      public string AnnouncementName { get; set; }
      [AllowHtml]     
      public string AnnouncementMessage { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? ExpirationDate { get; set; }
      public string AnnouncementStatus { get; set; }
      public bool IsAllCompanies { get; set; }
    }
}