using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Models.ViewModel
{
    public class ApplicantHireViewModel
    {
        public int ApplicantInformationId { get; set; }
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public int CompanyId { get; set; }
        public int? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SSN { get; set; }       
        public string HomeAddress1 { get; set; }      
        public string HomeAddress2 { get; set; }
        public int? HomeCityId { get; set; }
        public int? HomeStateId { get; set; }
        public int? HomeCountryId { get; set; }
        public string HomeZipCode { get; set; }        
        public string MailingAddress1 { get; set; }      
        public string MailingAddress2 { get; set; }
        public int? MailingCityId { get; set; }
        public int? MailingStateId { get; set; }
        public int? MailingCountryId { get; set; }     
        public string MailingZipCode { get; set; }
        public string HomeNumber { get; set; }
        public string CelNumber { get; set; }
        public string FaxNumber { get; set; }
        public string OtherNumber { get; set; }
        public string WorkEmail { get; set; }     
        public string PersonalEmail { get; set; }     
        public string OtherEmail { get; set; }      
       
        public IEnumerable<dynamic> GenderList { get; set; }
        public IEnumerable<dynamic> CompanyList { get; set; }
    }
}
