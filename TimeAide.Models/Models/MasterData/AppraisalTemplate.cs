using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("AppraisalTemplate")]
    public partial class AppraisalTemplate : BaseCompanyObjects
    {
        public AppraisalTemplate()
        {
            
        }

        [Column("AppraisalTemplateId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [NotMapped]
        public new bool IsAllCompanies { get; set; }
        public virtual ICollection<AppraisalTemplateGoal> AppraisalTemplateGoals { get; set; }
        public virtual ICollection<AppraisalTemplateSkill> AppraisalTemplateSkills { get; set; }

        //
        //public virtual ICollection<EmployeeAppraisal> EmployeeAppraisal { get; set; }

        //
        //public virtual ICollection<PositionAppraisalTemplate> PositionAppraisalTemplate { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.AppraisalTemplateSkills.
                            Where(t => t.DataEntryStatus == 1).
                            Select(t => t.AppraisalSkill.CompanyId).Distinct();
            var list2 = this.AppraisalTemplateGoals.
                            Where(t => t.DataEntryStatus == 1).
                            Select(t => t.AppraisalGoal.CompanyId).Distinct();
           var retList= list.Union(list2);

            return retList.ToList();
        }
    }
}
