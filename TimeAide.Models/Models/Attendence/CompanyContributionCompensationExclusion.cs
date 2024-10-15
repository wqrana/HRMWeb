namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanyContributionCompensationExclusion")]
    public partial class CompanyContributionCompensationExclusion : BaseEntity
    {
        [Display(Name = "CompanyContributionCompensationExclusionId")]
        [Column("CompanyContributionCompensationExclusionId")]
        public override int Id { get; set; }

        public int CompanyContributionId { get; set; }

        public int CompanyCompensationId { get; set; }
        public virtual CompanyCompensation CompanyCompensation { get; set; }

    }
}
