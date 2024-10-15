
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
    public class ApplicantFilterViewModel
    {
        public string ApplicantName { get; set; }
        public string SearchText { get; set; }
        public int? PositionId { get; set; }
        public int? ApplicantStatusId { get; set; }
        public int? LocationId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNo { get; set; }
        public int ViewTypeId { get; set; }
        public IEnumerable<QAFilterCriteriaViewModel> QACriteriaFilters { get; set; }
    }
    public class QAFilterCriteriaViewModel
    {
        public int Id { get; set; }
        public int ApplicantInterviewQuestionId { get; set; }
        public string QuestionName { get; set; }
        public int ApplicantAnswerTypeId { get; set; }       
        public string FilterOpertor { get; set; }
        public string FilterValue { get; set; }    

    }
}
