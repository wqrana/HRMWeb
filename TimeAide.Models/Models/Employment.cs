namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    [Table("Employment")]
    public partial class Employment : BaseUserObjects
    {
        public Employment()
        {
            EmploymentHistory = new HashSet<EmploymentHistory>();
            PayInformationHistory = new HashSet<PayInformationHistory>();
            NotificationLog = new HashSet<NotificationLog>();
        }

        [Display(Name = "Employment Id")]
        [Column("EmploymentId")]
        public override int Id { get; set; }

        //public int? CurrentRecord { get; set; }

        [Display(Name = "Hire Date")]
        [Required]
        public DateTime? OriginalHireDate { get; set; }

        [Display(Name = "Re-Hire Date")]
        [Required]
        public DateTime? EffectiveHireDate { get; set; }

        [Display(Name = "Probation Start Date")]
        //[Required]
        public DateTime? ProbationStartDate { get; set; }

        [Display(Name = "Probation End Date")]
        //[Required]
        public DateTime? ProbationEndDate { get; set; }

        [Display(Name = "Employment Status")]
        public int? EmploymentStatusId { get; set; }

        public DateTime? TerminationDate { get; set; }

        public int? TerminationTypeId { get; set; }

        public int? TerminationReasonId { get; set; }

        public bool? IsExitInterview { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadedDocumentFile { get; set; }

        [StringLength(500)]
        public string DocumentName { get; set; }

        [StringLength(500)]
        public string DocumentPath { get; set; }

        //[NotMapped]
        //public HttpPostedFileBase UploadedExitInterviewDocFile { get; set; }

        [StringLength(500)]
        public string ExitInterviewDocName { get; set; }
        [StringLength(500)]
        public string ExitInterviewDocPath { get; set; }
        [NotMapped]
        public bool IsExitInterviewMarkDel { get; set; }
        public int? TerminationEligibilityId { get; set; }
        public bool UseHireDateforYearsInService { get; set; }
        [StringLength(500)]
        public string TerminationNotes { get; set; }

        public virtual EmploymentStatus EmploymentStatus { get; set; }

        public virtual TerminationEligibility TerminationEligibility { get; set; }

        public virtual TerminationReason TerminationReason { get; set; }

        public virtual TerminationType TerminationType { get; set; }

        [NotMapped]
        public bool LockAccount { get; set; }

        [NotMapped]
        public int EmployeeStatusId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }
        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }
        public virtual ICollection<NotificationLog> NotificationLog { get; set; }

        [NotMapped]
        public String YearsinService
        {
            get
            {
                if (UserInformation == null || UserInformation.Employment.Count == 0)
                    return "";
                var latestEmployment = UserInformation.Employment.OrderByDescending(e => e.OriginalHireDate).FirstOrDefault();
                
                if (UserInformation!=null && latestEmployment!=null)
                {
                    DateTime? dateTime=null;
                    if (UseHireDateforYearsInService)
                        dateTime = latestEmployment.OriginalHireDate.Value; 
                    else 
                        dateTime = latestEmployment.EffectiveHireDate.Value;

                    if (dateTime.HasValue)
                    {
                        DateTime serviceEndDate = DateTime.Now;
                        if (TerminationDate.HasValue)
                            serviceEndDate = TerminationDate.Value;
                        TimeSpan serviceSpan = serviceEndDate - dateTime.Value;
                        var years = (int)serviceSpan.TotalDays / 365;
                        var days = (int)serviceSpan.TotalDays % 365;

                        return years + " years and " + days + " days";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "N/A";
                }
            }
        }
        [NotMapped]
        public DateTime? ApprovedDate { get; set; }
        [NotMapped]
        public int? ApprovedById { get; set; }
        [NotMapped]
        public string EmploymentRange
        {
            get
            {
                string employmentRange = "";
                if (EffectiveHireDate.HasValue)
                    employmentRange += EffectiveHireDate.Value.ToString("MM/dd/yyyy");
                if (TerminationDate.HasValue)
                    employmentRange += " - " + TerminationDate.Value.ToString("MM/dd/yyyy");
                else
                    employmentRange += " - " + "Now";
                return employmentRange;
            }
        }

        [NotMapped]
        public bool CanAddNew
        {
            get
            {
                if (Id == 0)
                    return true;
                if (UserInformation == null)
                    return false;
                var model = UserInformation.Employment.Where(W => W.DataEntryStatus == 1).OrderByDescending(e => e.OriginalHireDate).FirstOrDefault();
                if (model == null || model.Id == 0 || model.TerminationDate.HasValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [NotMapped]
        public bool CanEdit
        {
            get
            {
                //if (Id != 0 && !(this.TerminationDate.HasValue))
                if (Id != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [NotMapped]
        public bool IsClosed
        {
            get
            {
                if ((this.TerminationDate.HasValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [NotMapped]
        public bool HasOpenEmployment
        {
            get
            {
                if (EmploymentHistory.Count > 0 && !EmploymentHistory.OrderByDescending(e => e.StartDate).FirstOrDefault().EndDate.HasValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [NotMapped]
        public bool HasOpenPayInformation
        {
            get
            {
                if (PayInformationHistory.Count > 0 && !PayInformationHistory.OrderByDescending(e => e.StartDate).FirstOrDefault().EndDate.HasValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }


}
