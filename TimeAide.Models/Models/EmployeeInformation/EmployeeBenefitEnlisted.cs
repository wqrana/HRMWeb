    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
namespace TimeAide.Web.Models
{
    [Table("EmployeeBenefitEnlisted")]
    public partial class EmployeeBenefitEnlisted : BaseUserObjects
    {
        [Column("EmployeeBenefitEnlistedId")]
        public override int Id { get; set; }
        public int? BenefitId { get; set; }
        public virtual Benefit Benefit { get; set; }

    }
}
