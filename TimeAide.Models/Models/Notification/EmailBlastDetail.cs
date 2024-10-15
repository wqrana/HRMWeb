using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Web.Models;
namespace TimeAide.Web.Models
{
    public class EmailBlastDetail : BaseCompanyObjects
    {
        public EmailBlastDetail()
        {

        }
        [Key]
        [Column("EmailBlastDetailId")]
        public override int Id { get; set; }
        [Display(Name = "Email Blast")]
        public int EmailBlastId { get; set; }
        public virtual EmailBlast EmailBlast { get; set; }
        public int UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public DateTime? SentDate { get; set; }
    }
}