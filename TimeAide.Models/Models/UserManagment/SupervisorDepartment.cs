namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SupervisorDepartment")]
    public partial class SupervisorDepartment : BaseEntity
    {
        [Column("SupervisorDepartmentId")]
        public override int Id { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
