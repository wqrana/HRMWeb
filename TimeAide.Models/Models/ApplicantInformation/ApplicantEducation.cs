
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantEducation")]
    public partial class ApplicantEducation : BaseApplicantObjects
    {
        [Column("ApplicantEducationId")]
        public override int Id { get; set; }
        [Required]
        public string InstitutionName { get; set; }
        [Required]
        public string Title { get; set; }
        public string Note { get; set; }
        [Required]
        public DateTime DateCompleted { get; set; }
        public string DocName { get; set; }
        public string DocFilePath { get; set; }
        [Required]
        public int? DegreeId { get; set; }
        public virtual Degree Degree { get; set; }

    }
}
