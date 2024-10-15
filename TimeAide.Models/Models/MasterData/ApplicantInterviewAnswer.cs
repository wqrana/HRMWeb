using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("ApplicantInterviewAnswer")]
    public partial class ApplicantInterviewAnswer : BaseEntity
    {
        [Column("ApplicantInterviewAnswerId")]
        public override int Id { get; set; }
        [Required]
        [StringLength(400)]
        [Display(Name = "Answer")]
        public string AnswerName { get; set; }
        [Display(Name = "Description")]
        [StringLength(400)]
        public string Description { get; set; }
        [Display(Name = "Answer Rating")]
        [Required]
        public double AnswerValue { get; set; }
        [Required]
        [Display(Name = "Max Rating")]
        public double AnswerMaxValue { get; set; }
       
    }
}
