using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TimeAide.Web.Models
{
    [Table("NotificationLogEmail")]
    public class NotificationLogEmail : BaseCompanyObjects
    {

        
        public NotificationLogEmail()
        {
        }
        [Display(Name = "Notification Log Email Id")]
        [Column("NotificationLogEmailId")]
        public override int Id { get; set; }

        public string SenderAddress { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public int? NotificationLogId { get; set; }
        public virtual NotificationLog NotificationLog { get; set; }
        [NotMapped]
        public List<string> ToAddressList
        {
            get
            {
                if (string.IsNullOrEmpty(ToAddress))
                    return new List<string>();
                return ToAddress.Split(',').ToList();

            }
        }
    }
}
