namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("EmailTemplate")]
    public partial class EmailTemplate : BaseCompanyObjects
    {
        
        public EmailTemplate()
        {
        }

        [Display(Name = "Email Template")]
        [Column("EmailTemplateId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email Subject")]
        public string EmailSubject { get; set; }

        [Display(Name = "Email Body")]
        public string EmailBody { get; set; }

        public int EmailTypeId { get; set; }
        public virtual EmailType EmailType { get; set; }
    }
}
