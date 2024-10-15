namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApplicantEmploymentType")]
    public partial class ApplicantEmploymentType : BaseApplicantObjects
    {
        [Column("ApplicantEmploymentTypeId")]
        public override int Id { get; set; }
        [Required]
        public int EmployeeTypeId { get; set; }              
        public virtual EmployeeType EmployeeType { get; set; }

    }
}
