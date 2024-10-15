namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("UserSessionLog")]
    public partial class UserSessionLog : BaseCompanyObjects
    {
        
        public UserSessionLog()
        {
            UserSessionLogDetail = new HashSet<UserSessionLogDetail>();
        }
        [Column("UserSessionLogId")]
        public override int Id { get; set; }
        [Required]
        public int UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        [StringLength(150)]
        [Display(Name = "IPAddress")]
        public string IPAddress { get; set; }

        public string BrowserInformation { get; set; }
        public string DeviceInformation { get; set; }
        public string OsInformation { get; set; }
        public string Location { get; set; }
        public virtual ICollection<UserSessionLogDetail> UserSessionLogDetail { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            return new List<int?>();
        }
    }
}
