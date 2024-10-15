using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AppraisalTemplateSkill")]
    public partial class AppraisalTemplateSkill : BaseEntity
    {
        [Column("AppraisalTemplateSkillId")]
        public override int Id { get; set; }
        public int AppraisalTemplateId { get; set; }
        [Required]
        public int AppraisalSkillId { get; set; }

        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
        public virtual AppraisalSkill AppraisalSkill { get; set; }
    }
}
