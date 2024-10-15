using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantQAnswerOption")]
    public partial class ApplicantQAnswerOption : BaseEntity
    {
        [Column("ApplicantQAnswerOptionId")]
        public override int Id { get; set; }
        public int ApplicantInterviewQuestionId { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Answer Option")]
        public string AnswerOptionName { get; set; }        
        public virtual ApplicantInterviewQuestion ApplicantInterviewQuestion { get; set; }
    }
}
