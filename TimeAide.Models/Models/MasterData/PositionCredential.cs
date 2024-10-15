using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{

    [Table("PositionCredential")]
    public partial class PositionCredential : BaseEntity
    {
        [Column("PositionCredentialId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        [Required]
        public int CredentialId { get; set; }
        public bool? IsRequired { get; set; }
        public virtual Position Position { get; set; }
        public virtual Credential Credential { get; set; }
    }
}
