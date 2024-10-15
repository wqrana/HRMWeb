namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SupervisorCompany")]
    public partial class SupervisorCompany : BaseEntity
    {
        [Column("SupervisorCompanyId")]
        public override int Id { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
