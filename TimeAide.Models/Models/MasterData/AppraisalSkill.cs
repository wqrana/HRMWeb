
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("AppraisalSkill")]
    public partial class AppraisalSkill : BaseCompanyObjects
    {
        public AppraisalSkill()
        {
            //AppraisalTemplateSkill = new HashSet<AppraisalTemplateSkill>();
            //EmployeeAppraisalSkill = new HashSet<EmployeeAppraisalSkill>();
        }

        [Column("AppraisalSkillId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Skill Name")]
        public string SkillName { get; set; }

        [Display(Name = "Description")]
        public string SkillDescription { get; set; }

        [Required]
        [Display(Name = "Rating Scale")]
        public int AppraisalRatingScaleId { get; set; }
       
        public virtual AppraisalRatingScale AppraisalRatingScale { get; set; }
        public virtual ICollection<AppraisalTemplateSkill> AppraisalTemplateSkills { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            var list = this.AppraisalTemplateSkills.Where(t => t.DataEntryStatus == 1).Select(t => t.AppraisalSkill.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
