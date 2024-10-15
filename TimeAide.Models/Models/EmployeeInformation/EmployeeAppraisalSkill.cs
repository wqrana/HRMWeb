using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAppraisalSkill")]
    public partial class EmployeeAppraisalSkill : BaseEntity
    {
        [Column("EmployeeAppraisalSkillId")]
        public override int Id { get; set; }
        public int EmployeeAppraisalId { get; set; }
        public int AppraisalSkillId { get; set; }
        public int AppraisalRatingScaleDetailId { get; set; }
        public string SkillRatingName { get; set; }
        public decimal SkillRatingValue { get; set; }
        public decimal SkillScaleMaxValue { get; set; }
        public string ReviewerComments { get; set; }
        public virtual EmployeeAppraisal EmployeeAppraisal { get; set; }
        public virtual AppraisalSkill AppraisalSkill { get; set; }
        public virtual AppraisalRatingScaleDetail AppraisalRatingScaleDetail { get; set; }
    }
}
