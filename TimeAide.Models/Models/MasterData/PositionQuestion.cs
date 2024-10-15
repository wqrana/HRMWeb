
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{

    [Table("PositionQuestion")]
    public partial class PositionQuestion : BaseEntity
    {
        [Column("PositionQuestionId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        [Required]
        public int ApplicantInterviewQuestionId { get; set; }
        public virtual Position Position { get; set; }
        public virtual ApplicantInterviewQuestion ApplicantInterviewQuestion { get; set; }
    }
}
