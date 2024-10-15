namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmailType")]
    public partial class EmailType : BaseGlobalEntity
    {
        
        public EmailType()
        {
            EmailTemplate = new HashSet<EmailTemplate>();
        }

        [Display(Name = "Email Type")]
        [Column("EmailTypeId")]
        public override int Id { get; set; }

        [StringLength(1000)]
        [Display(Name = "EmailType")]
        public string EmailTypeName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Email Type Description")]
        public string EmailTypeDescription { get; set; }

        
        public virtual ICollection<EmailTemplate> EmailTemplate { get; set; }
    }
}
