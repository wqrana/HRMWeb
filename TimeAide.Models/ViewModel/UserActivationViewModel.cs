using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Models.ViewModel
{
   public class UserActivationViewModel
    {
        public int UserInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public int? ToCompanyId { get; set; }
        public int? EmploymentId { get; set; }
        public DateTime? OriginalHireDate { get; set; }

        public DateTime? EffectiveHireDate { get; set; }

        public DateTime? ProbationStartDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }

        public int? EmploymentStatusId { get; set; }
        public int? EmploymentHistoryId { get; set; }
        public DateTime? EmploymentStartDate { get; set; }          
        public int? EmployeeTypeId { get; set; }   
        public int? EmploymentTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? PayInformationHistoryId { get; set; }
          
        public DateTime? PayStartDate { get; set; }
             
        public int? PositionId { get; set; }

        public int? EEOCategoryId { get; set; }

        public int? PayTypeId { get; set; }

        public int? PayFrequencyId { get; set; }
        public decimal? PeriodHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00000}", ApplyFormatInEditMode = true)]
        public decimal RateAmount { get; set; }
        public int? RateFrequencyId { get; set; }
        public decimal PeriodGrossPay { get; set; }
        public decimal YearlyGrossPay { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? TerminationTypeId { get; set; }
        public int? TerminationReasonId { get; set; }
        public int? ApprovedById { get; set; }

        public int UserContactInformationId { get; set; }
        public int RoleTypeId { get; set; }
        public int RoleId { get; set; }
        public string LoginEmail { get; set; }
        public int RegistrationActionTypeId { get; set; }
        public IEnumerable<dynamic> EmploymentStatusList { get; set; }
        public IEnumerable<dynamic> DepartmentList { get; set; }
        public IEnumerable<dynamic> SubDepartmentList { get; set; }     
        public IEnumerable<dynamic> CompanyList { get; set; }
        public IEnumerable<dynamic> ToCompanyList { get; set; }
        public IEnumerable<dynamic> EmployeeTypeList { get; set; }
        public IEnumerable<dynamic> EmploymentTypeList { get; set; }
        public IEnumerable<dynamic> PositionList { get; set; }
        public IEnumerable<dynamic> EEOCategoryList { get; set; }
        public IEnumerable<dynamic> PayTypeList { get; set; }
        public IEnumerable<dynamic> PayFrequencyList { get; set; }
        public IEnumerable<dynamic> RateFrequencyList { get; set; }
        public IEnumerable<dynamic> TerminationTypeList { get; set; }
        public IEnumerable<dynamic> TerminationReasonList { get; set; }
        public IEnumerable<dynamic> SuperviserList { get; set; }

    }
}
