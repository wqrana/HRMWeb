using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Web.Models;
namespace TimeAide.Web.Models
{
    public class EmailBlast : BaseCompanyObjects
    {
        public EmailBlast()
        {
            NotificationLog = new HashSet<NotificationLog>();
            EmailBlastDetail = new HashSet<EmailBlastDetail>();
        }
        [Key]
        [Column("EmailBlastId")]
        public override int Id { get; set; }
        [Display(Name = "Template Id")]
        public int EmailTemplateId { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; }
        public virtual ICollection<NotificationLog> NotificationLog { get; set; }
        public virtual ICollection<EmailBlastDetail> EmailBlastDetail { get; set; }
        [NotMapped]
        public string UserInformationIds { get; set; }
        public bool IsSavedAsDraft { get; set; }
        [NotMapped]
        public DateTime? SentDate
        {
            get 
            {
                if(EmailBlastDetail.Count>0)
                {
                    return EmailBlastDetail.FirstOrDefault().SentDate;
                }
                return null;
            }
        }
    }
}