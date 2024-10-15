namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("Company")]
    public partial class Company : BaseEntity
    {
        public Company()
        {
            UserInformations = new HashSet<UserInformation>();
            SupervisorCompany = new HashSet<SupervisorCompany>();
        }

        [Display(Name = "Company Id")]
        [Column("CompanyId")]
        public override int Id { get; set; }

        
        [StringLength(25)]
        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(200)]
        [Display(Name = "Company Description")]
        public string CompanyDescription { get; set; }

        [StringLength(25)]
        [Display(Name = "EIN")]
        public string EIN { get; set; }

        [StringLength(50)]
        public string ParentCompany { get; set; }
        [StringLength(50)]
        public string DisplayName { get; set; }
        [StringLength(100)]
        public string Address1 { get; set; }
        [StringLength(100)]
        public string Address2 { get; set; }

        public int? CityId { get; set; }
        public int? StateId { get; set; }
        [StringLength(9)]
        public string ZipCode { get; set; }
        [StringLength(50)]
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        [StringLength(50)]
        public string ContactEmail { get; set; }
        [StringLength(50)]
        public string ContactPosition { get; set; }
        [StringLength(9)]
        public string NAICS { get; set; }
        public string DUNS { get; set; }
        [StringLength(9)]
        public string EmployerID { get; set; }
        [StringLength(200)]
        public string NameInLetters { get; set; }
        [StringLength(4)]
        public string SIC { get; set; }
        public string PortalPictureFilePath { get; set; }
        public bool? IsDefaultPortalPicture { get; set; }
        [AllowHtml]
        public string PortalWelcomeStatement { get; set; }
        public bool? IsDefaultPortalStatement { get; set; }
        public int? DefaultLetterSigneeId { get; set; }
        public int? DefaultLetterTemplateId { get; set; }
        public virtual JobCertificationSignee DefaultLetterSignee { get; set; }
        public virtual JobCertificationTemplate DefaultLetterTemplate { get; set; }
        public virtual ICollection<SupervisorCompany> SupervisorCompany { get; set; }
        public virtual ICollection<UserInformation> UserInformations { get; set; }
        public virtual ICollection<EmailTemplate> EmailTemplate { get; set; }
        public virtual Client Client { get; set; }
        new public int? CreatedBy { get; set; }
        public byte[] CompanyLogo { get; set; }
    }
}
