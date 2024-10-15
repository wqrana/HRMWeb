namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SupervisorEmployeeType")]
    public partial class SupervisorEmployeeType : BaseEntity
    {
        [Column("SupervisorEmployeeTypeId")]
        public override int Id { get; set; }

        public int? EmployeeTypeId { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public int? UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
