namespace TimeAide.Web.Models
{
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using TimeAide.Web.Models;


    [Table("CompanyAnnouncement")]
    public partial  class CompanyAnnouncement : BaseCompanyObjects
    {
        [Column("CompanyAnnouncementId")]
        public override int Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
