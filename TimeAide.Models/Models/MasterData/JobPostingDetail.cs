
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{

    [Table("JobPostingDetail")]
    public partial class JobPostingDetail : BaseCompanyObjects
    {

        [Column("JobPostingDetailId")]
        public override int Id { get; set; }

        [Display(Name = "Position")]
        [Required]
        public int? PositionId { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Display(Name = "No Of Vacancies")]
        public int? NoOfVacancies { get; set; }

        [Display(Name = "Experience")]
        public string Experience { get; set; }

        [Display(Name = "Salary From")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary To")]
        public decimal? SalaryTo { get; set; }

        [Display(Name = "Employment Type")]
        public int? EmploymentTypeId { get; set; }

        [Display(Name = "Job Status")]
        public int? JobPostingStatusId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? JobPostingStartDate { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? JobPostingExpiringDate { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        public bool? IsHomeAddress { get; set; }
        public bool? IsMailingAddress { get; set; }
        public bool? IsPreviousEmployment { get; set; }
        public bool? IsEducation { get; set; }
        public bool? IsAvailablity { get; set; }
        [NotMapped]
        public string LocationIds { get; set; }
        [NotMapped]
        public string SelectedLocationIds 
        { 
            get 
            {
                if (JobPostingLocations != null)
                {
                    return String.Join(",", JobPostingLocations
                         .Select(s => s.Location.Id.ToString())
                         .ToArray());
                }
                else
                {
                    return string.Empty;
                }
            } 
        }
        [NotMapped]
        public string SelectedLocationNames
        {
            get
            {
                if (JobPostingLocations != null)
                {
                   return String.Join(",",JobPostingLocations
                        .Select(s => s.Location.LocationName)
                        .ToArray());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public virtual Company Company { get; set; }
        public virtual Position Position { get;set;}
        public virtual Department Department { get; set; }
        public virtual Location Location { get; set; }
        public virtual EmploymentType EmploymentType { get; set; }
        public virtual JobPostingStatus JobPostingStatus { get; set; }
        public virtual ICollection<ApplicantInformation> ApplicantInformations { get; set; }
        public virtual ICollection<JobPostingLocation> JobPostingLocations { get; set; }
    }
}
