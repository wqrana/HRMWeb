
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantCustomField")]
    public partial class ApplicantCustomField : BaseApplicantObjects
    {
       
        [Display(Name = "Applicant Custom Id")]
        [Column("ApplicantCustomFieldId")]
        public override int Id { get; set; }

        [Display(Name = "Custom Field")]
        public int CustomFieldId { get; set; }



        [Display(Name = "Field Value")]
        [StringLength(500)]
        public string CustomFieldValue { get; set; }

        [Display(Name = "Field Note")]
        [StringLength(250)]
        public string CustomFieldNote { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        public virtual CustomField CustomField { get; set; }

        public virtual ApplicantInformation ApplicantInformation { get; set; }
    }
}
