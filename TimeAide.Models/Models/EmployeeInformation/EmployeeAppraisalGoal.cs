using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAppraisalGoal")]
    public partial class EmployeeAppraisalGoal : BaseEntity
    {
        [Column("EmployeeAppraisalGoalId")]
        public override int Id { get; set; }
        public int EmployeeAppraisalId { get; set; }
        public int AppraisalGoalId { get; set; }
        public int AppraisalRatingScaleDetailId { get; set; }
        public string GoalRatingName { get; set; }
        public decimal GoalRatingValue { get; set; }
        public decimal GoalScaleMaxValue { get; set; }
        public string ReviewerComments { get; set; }
        public virtual EmployeeAppraisal EmployeeAppraisal { get; set; }
        public virtual AppraisalGoal AppraisalGoal { get; set; }
        //public virtual AppraisalRatingScaleDetail AppraisalRatingScaleDetail { get; set; }
    }
}
