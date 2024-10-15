using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public class ChangeRequestEmailNumbers :ChangeRequestBase
    {
        public ChangeRequestEmailNumbers()
        {
        }

        public int UserContactInformationId { get; set; }
        public virtual UserContactInformation UserContactInformation { get; set; }

        [Key]
        [Column("ChangeRequestEmailNumbersId")]
        public override int Id { get; set; }
        [StringLength(20)]
        public string HomeNumber { get; set; }
        [StringLength(20)]
        public string CelNumber { get; set; }
        [StringLength(20)]
        public string FaxNumber { get; set; }
        [StringLength(20)]
        public string OtherNumber { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Work Email Address")]
        public string WorkEmail { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Personal Email Address")]
        public string PersonalEmail { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Other Email Address")]
        public string OtherEmail { get; set; }
        [StringLength(20)]
        public string WorkNumber { get; set; }
        [StringLength(10)]
        public string WorkExtension { get; set; }
        [StringLength(20)]
        public string NewHomeNumber { get; set; }
        [StringLength(20)]
        public string NewCelNumber { get; set; }
        [StringLength(20)]
        public string NewFaxNumber { get; set; }
        [StringLength(20)]
        public string NewOtherNumber { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Work Email Address")]
        public string NewWorkEmail { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Personal Email Address")]
        public string NewPersonalEmail { get; set; }
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Other Email Address")]
        public string NewOtherEmail { get; set; }
        [StringLength(20)]
        public string NewWorkNumber { get; set; }
        [StringLength(10)]
        public string NewWorkExtension { get; set; }
        [NotMapped] 
        public bool IsModified { get; set;}
        
    }
}
