

namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantInterview")]
    public partial class ApplicantInterview : BaseApplicantObjects
    {

        [Display(Name = "Applicant Custom Id")]
        [Column("ApplicantInterviewId")]
        public override int Id { get; set; }
        [Display(Name = "Question")]
        public int ApplicantInterviewQuestionId { get; set; }
        public string ApplicantAnswer { get; set; }

        [Display(Name = "Answer")]
        public int? ApplicantInterviewAnswerId { get; set; }
        public double? InterviewAnswerValue { get; set; }
        public double? InterviewAnswerMaxValue { get; set; }
        public string Note { get; set; }       
        public virtual ApplicantInterviewQuestion ApplicantInterviewQuestion { get; set; }
        public virtual ApplicantInterviewAnswer ApplicantInterviewAnswer { get; set; }
    }
}
