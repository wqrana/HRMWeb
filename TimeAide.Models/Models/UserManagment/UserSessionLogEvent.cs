namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TimeAide.Common.Helpers;

    [Table("UserSessionLogEvent")]
    public partial class UserSessionLogEvent : BaseEntity
    {

        public UserSessionLogEvent()
        {
            CompanyId = SessionHelper.SelectedCompanyId;
        }
        [Column("UserSessionLogEventId")]
        public override int Id { get; set; }
        public Boolean LogListing { get; set; }
        public Boolean LogView { get; set; }
        public Boolean LogAddOpen { get; set; }
        public Boolean LogAddSave { get; set; }
        public Boolean LogEditOpen { get; set; }
        public Boolean LogEditSave { get; set; }
        public Boolean LogDeleteOpen { get; set; }
        public Boolean LogDeleteSave { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name = "All Companies")]
        [NotMapped]
        public bool IsAllCompanies
        {
            get
            {
                return !CompanyId.HasValue;
            }
        }
    }
}
