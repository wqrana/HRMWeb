using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AppraisalTemplateGoal")]
    public partial class AppraisalTemplateGoal : BaseEntity
    {
        [Column("AppraisalTemplateGoalId")]
        public override int Id { get; set; }
        public int AppraisalTemplateId { get; set; }
        [Required]
        public int AppraisalGoalId { get; set; }
               
        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
        public virtual AppraisalGoal AppraisalGoal { get; set; }
    }
}
