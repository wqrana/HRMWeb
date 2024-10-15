using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantInterviewQuestion")]
    public partial class ApplicantInterviewQuestion : BaseEntity
    {
        [Column("ApplicantInterviewQuestionId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(400)]
        [Display(Name = "Question")]
        public string QuestionName { get; set; }
        [Display(Name = "Description")]
        [StringLength(400)]
        public string Description { get; set; }
        [Display(Name = "Position Specific")]
        public bool IsPositionSpecific { get; set; }
        [Display(Name = "Required")]
        public bool? IsRequired { get; set; }
        [Display(Name = "Answer Type")]
        public int? ApplicantAnswerTypeId { get; set; }
        [NotMapped]
        public int? PositionId { get; set; }

        public virtual IEnumerable<ApplicantQAnswerOption> ApplicantQAnswerOptions { get; set; }
    }
}
