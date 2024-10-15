
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("AppraisalGoal")]
    public partial class AppraisalGoal : BaseCompanyObjects
    {
        public AppraisalGoal()
        {
             AppraisalTemplateGoals = new HashSet<AppraisalTemplateGoal>();
            //EmployeeAppraisalGoal = new HashSet<EmployeeAppraisalGoal>();
        }

        [Column("AppraisalGoalId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Goal Name")]
        public string GoalName { get; set; }

        [Display(Name = "Description")]
        public string GoalDescription { get; set; }

        [Required]
        [Display(Name = "Rating Scale")]
        public int AppraisalRatingScaleId { get; set; }
       
        public virtual AppraisalRatingScale AppraisalRatingScale { get; set; }
        public virtual ICollection<AppraisalTemplateGoal> AppraisalTemplateGoals { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.AppraisalTemplateGoals.Where(t => t.DataEntryStatus == 1).Select(t => t.AppraisalGoal.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
