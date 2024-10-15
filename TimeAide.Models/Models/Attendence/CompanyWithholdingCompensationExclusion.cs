namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyWithholdingCompensationExclusion")]
    public partial class CompanyWithholdingCompensationExclusion : BaseEntity
    {
        [Display(Name = "CompanyWithholdingCompensationExclusionId")]
        [Column("CompanyWithholdingCompensationExclusionId")]
        public override int Id { get; set; }

        public int CompanyWithholdingId { get; set; }

        public int CompanyCompensationId { get; set; }
        public virtual CompanyCompensation CompanyCompensation { get; set; }

    }
}
