namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationTransaction")]
    public partial class CompensationTransaction:BaseEntity
    {
        [Display(Name = "CompensationTransactionId")]
        [Column("CompensationTransactionId")]
        public override int Id { get; set; }

        public int? CompanyCompensationId { get; set; }

        public int? TransactionConfigurationId { get; set; }
        
        public virtual TransactionConfiguration TransactionConfiguration { get; set; }
        public virtual CompanyCompensation CompanyCompensation { get; set; }
    }
}
