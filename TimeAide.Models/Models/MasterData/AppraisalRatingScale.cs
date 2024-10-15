
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("AppraisalRatingScale")]
    public partial class AppraisalRatingScale : BaseCompanyObjects
    {
        public AppraisalRatingScale()
        {
            //AppraisalGoal = new HashSet<AppraisalGoal>();
            //AppraisalRatingScaleDetail = new HashSet<AppraisalRatingScaleDetail>();
            //AppraisalSkill = new HashSet<AppraisalSkill>();
        }

        [Column("AppraisalRatingScaleId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Scale Name")]
        public string ScaleName { get; set; }

        [Display(Name = "Description")]
        public string ScaleDescription { get; set; }
        [Display(Name = "Max Value")]
        public decimal? ScaleMaxValue { get; set; }

        [NotMapped]
        public new bool IsAllCompanies { get; set; }
        public virtual ICollection<AppraisalGoal> AppraisalGoals { get; set; }
        public virtual ICollection<AppraisalSkill> AppraisalSkills { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            var list = this.AppraisalGoals.
                            Where(t => t.DataEntryStatus == 1).
                            Select(t => t.CompanyId).Distinct();
            var list2 = this.AppraisalSkills.
                            Where(t => t.DataEntryStatus == 1).
                            Select(t => t.CompanyId).Distinct();
            var retList = list.Union(list2);

            return retList.ToList();
        }
    }
}