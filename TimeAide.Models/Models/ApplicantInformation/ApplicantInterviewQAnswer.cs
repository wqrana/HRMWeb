
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantInterviewQAnswer")]
    public partial class ApplicantInterviewQAnswer : BaseEntity
    {
              
        [Column("ApplicantInterviewQAnswerId")]
        public override int Id { get; set; }       
        public int ApplicantInterviewId { get; set; }
        public int? ApplicantQAnswerOptionId { get; set; }
        public string AnswerValue { get; set; }

        [NotMapped]
        public int ApplicantInterviewQuestionId { get; set; }
        public virtual ApplicantInterview ApplicantInterview { get; set; }
        public virtual ApplicantQAnswerOption ApplicantQAnswerOption { get; set; }
    }
}
