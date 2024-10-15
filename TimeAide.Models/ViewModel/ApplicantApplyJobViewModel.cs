using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeAide.Web.Models;
namespace TimeAide.Models.ViewModel
{
  public  class ApplicantApplyJobViewModel
    {
        public JobPostingDetail JobDetail { get; set; } 
        public int JobPostingDetailId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string ShortFullName { get; set; }
        public int? GenderId { get; set; }
        public int? DisabilityId { get; set; }
        public int? ApplicantReferenceTypeId { get; set; }
        public int? ApplicantReferenceSourceId { get; set; }
        public string JobLocationIds { get; set; }
        public bool? IsWorkedBefore { get; set; }
        public DateTime? DateAvailable { get; set; }
        public bool? IsOvertime { get; set; }
        public bool? IsRelativeInCompany { get; set; }
        public string RelativeName { get; set; }
        [StringLength(20)]
        public string CellNumber { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Personal Email Address")]
        public string PersonalEmail { get; set; }

        public byte[] ApplicantCV { get; set; }
        [StringLength(50)]
        public string HomeAddress1 { get; set; }

        [StringLength(50)]
        public string HomeAddress2 { get; set; }

        public int? HomeCityId { get; set; }
        public int? HomeStateId { get; set; }
        public int? HomeCountryId { get; set; }

        [StringLength(10)]
        public string HomeZipCode { get; set; }

        [StringLength(50)]
        public string MailingAddress1 { get; set; }

        [StringLength(50)]
        public string MailingAddress2 { get; set; }

        public int? MailingCityId { get; set; }
        public int? MailingStateId { get; set; }
        public int? MailingCountryId { get; set; }

        [StringLength(10)]
        public string MailingZipCode { get; set; }

        public IEnumerable<ApplicantInterviewQuestion> InterviewQuestionList { get; set; }
        public IEnumerable<QAViewModel> QAList { get; set; }
    }
    public class QAViewModel
    {
        public int QId { get; set; }
       public IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
    public class AnswerOption
    {
        public int QId { get; set; }
        public int AOptionId { get; set; }
        public string AOptionValue { get; set; }
    }

  
}
