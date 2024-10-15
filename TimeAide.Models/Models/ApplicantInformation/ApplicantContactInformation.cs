namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("ApplicantContactInformation")]
    public partial class ApplicantContactInformation : BaseEntity
    {
        [Key]
        [Column("ApplicantContactInformationId")]
        public override int Id { get; set; }
        public int ApplicantInformationId { get; set; }
       
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

        [StringLength(20)]
        public string HomeNumber { get; set; }

        [StringLength(20)]
        public string CelNumber { get; set; }

        [StringLength(20)]
        public string FaxNumber { get; set; }

        [StringLength(20)]
        public string OtherNumber { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Work Email Address")]
        public string WorkEmail { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Personal Email Address")]
        public string PersonalEmail { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Other Email Address")]
        public string OtherEmail { get; set; }

        [StringLength(20)]
        public string WorkNumber { get; set; }

        [StringLength(10)]
        public string WorkExtension { get; set; }

        public virtual City HomeCity { get; set; }
        public virtual City MailingCity { get; set; }

        public virtual State HomeState { get; set; }
        public virtual State MailingState { get; set; }

        public virtual Country HomeCountry { get; set; }
        public virtual Country MailingCountry { get; set; }

        public virtual ApplicantInformation ApplicantInformation { get; set; }      

       
        [NotMapped]
        public bool HasHomeAddress
        {
            get
            {
                return (!string.IsNullOrEmpty(HomeAddress1) || !string.IsNullOrEmpty(HomeAddress2) || !string.IsNullOrEmpty(HomeZipCode) || HomeCityId > 0 || HomeStateId > 0 || HomeCountryId > 0);
            }
        }

        [NotMapped]
        public string WorkNumberWithExtension
        {
            get
            {
                if (string.IsNullOrEmpty(WorkNumber))
                    return "";
                if (string.IsNullOrEmpty(WorkExtension))
                    return WorkNumber;
                return WorkNumber + " Ext: " + WorkExtension;
            }
        }

        [NotMapped]
        public bool HasMailingAddress
        {
            get
            {
                return (!string.IsNullOrEmpty(MailingAddress1) || !string.IsNullOrEmpty(MailingAddress2) || !string.IsNullOrEmpty(MailingZipCode) || MailingCityId > 0 || MailingStateId > 0 || MailingCountryId > 0);
            }
        }
    }
}
