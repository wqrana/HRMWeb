using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("AppraisalRatingScaleDetail")]
    public partial class AppraisalRatingScaleDetail : BaseEntity
    {
        public AppraisalRatingScaleDetail()
        {
            //EmployeeAppraisalGoal = new HashSet<EmployeeAppraisalGoal>();
            EmployeeAppraisalSkill = new HashSet<EmployeeAppraisalSkill>();
        }

        [Column("AppraisalRatingScaleDetailId")]
        public override int Id { get; set; }
        public int AppraisalRatingScaleId { get; set; }

        [Required]
        [Display(Name = "Rating Level")]
        public int RatingLevelId { get; set; }

        [Required]
        [Display(Name = "Rating Value")]
        public decimal RatingValue { get; set; }

        [Required]
        [Display(Name = "Rating Name")]
        public string RatingName { get; set; }
                
        [Display(Name = "Description")]
        public string RatingDescription { get; set; }
        [Required]
        [Display(Name = "Abbreviation")]
        public string RatingAbbreviation { get; set; }

        public virtual AppraisalRatingScale AppraisalRatingScale { get; set; }
        public virtual RatingLevel RatingLevel { get; set; }
        public virtual ICollection<EmployeeAppraisalSkill> EmployeeAppraisalSkill { get; set; }
    }
}