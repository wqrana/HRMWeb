namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantInformation")]
    public partial class ApplicantInformation : BaseCompanyObjects
    {

        public ApplicantInformation()
        {
            CreatedBy = Common.Helpers.SessionHelper.LoginId;
            
        }

        [Display(Name = "Applicant Id")]
        [Column("ApplicantInformationId")]
        public override int Id { get; set; }

        [Display(Name = "External Id")]
        public int? ApplicantExternalId { get; set; }                

        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(1)]
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        [StringLength(30)]
        [Display(Name = "First Last Name")]
        public string FirstLastName { get; set; }

        [StringLength(30)]
        [Display(Name = "Second Last Name")]
        public string SecondLastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Short Full Name")]
        public string ShortFullName { get; set; }              

        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        public int? DisabilityId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "DOB")]
        public DateTime? BirthDate { get; set; }    

        [NotMapped]
        public string SSN
        {
            get
            {
                if (!string.IsNullOrEmpty(SSNEncrypted))
                    return Common.Helpers.Encryption.Decrypt(SSNEncrypted);
                else
                    return SSNEncrypted;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SSNEncrypted = Common.Helpers.Encryption.Encrypt(value);
                else

                    SSNEncrypted = value;
            }
        }

        [StringLength(512)]
        [Display(Name = "SSN Encrypted")]
        public string SSNEncrypted { get; set; }
              
        [StringLength(512)]
        public string PictureFilePath { get; set; }
        [StringLength(512)]
        public string ResumeFilePath { get; set; }     
        public new int? CreatedBy { get; set; }
        public int? ApplicantStatusId { get; set; }

        public int? UserInformationId { get; set; }

        public int? JobPostingDetailId { get; set; }
        public virtual ApplicantStatus ApplicantStatus { get; set; }
        public virtual Company Company { get; set; }
        public virtual Client Client { get; set; }       
        public virtual Gender Gender { get; set; }
        public virtual Disability Disability { get; set; }
        public virtual JobPostingDetail JobPostingDetail { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(ShortFullName))
                {
                    return ShortFullName;
                }
                else
                {
                    return (FirstName ?? "") + " " + (FirstLastName ?? "") + " " + (SecondLastName ?? "");
                }
            }
        }       
                
    }  
}
