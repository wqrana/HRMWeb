
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;


namespace TimeAide.Models.ViewModel
{
    public class ApplicantInformationViewModel
    {
        public int ApplicantInformationId { get; set; }
        public int? ApplicantExternalId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string ShortFullName { get; set; }
        public string GenderName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public string LocationName { get; set; }
        public int? ApplicantContactInformationId { get; set; }
        public string CelNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string PictureFilePath { get; set; }
        public string ResumeFilePath { get; set; }
        public int? ApplicantStatusId { get; set; }
        public DateTime? ApplicantStatusDate { get; set; }
        public string ApplicantStatusName { get; set; }
        public bool UseAsHire { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ClientId { get; set; }
        public int? UserInformationId { get; set; }

    }
}
