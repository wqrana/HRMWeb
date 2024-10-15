
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{

    [Table("PositionAppraisalTemplate")]
    public partial class PositionAppraisalTemplate : BaseEntity
    {
        [Column("PositionCAppraisalTemplateId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        [Required]
        public int AppraisalTemplateId { get; set; }

        public virtual Position Position { get; set; }
        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
    }
}
