namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CredentialType")]
    public partial class CredentialType : BaseEntity
    {
        public CredentialType()
        {
        }

        [Display(Name = "Credential Type")]
        [Column("CredentialTypeId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Credential Type")]
        [StringLength(50)]
        public string CredentialTypeName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
    }
}
