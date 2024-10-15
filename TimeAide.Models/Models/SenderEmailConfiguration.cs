using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public class SenderEmailConfiguration : BaseCompanyObjects
    {
        public SenderEmailConfiguration()
        {

        }

        [Column("SenderEmailConfigurationId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string MailProvider { get; set; }
        [Required]
        [StringLength(250)]
        public string HostName { get; set; }
        [Required]
        [StringLength(1000)]
        public string ProviderAccount { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        public string RetyprPassword { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        [Required]
        [StringLength(255)]
        public string FromEmail { get; set; }
        public bool UseFixedForm { get; set; }
        [StringLength(100)]
        public string SampleEmail { get; set; }
        [StringLength(1000)]
        public string Environment { get; set; }
        [Required]
        [StringLength(1000)]
        public string SenderName { get; set; }

        [NotMapped]
        public string ToEmailForTest { get; set; }
        [NotMapped]
        public string TestEmailBody { get; set; }
        [NotMapped]
        public string TestEmailSubject { get; set; }
    }
}
