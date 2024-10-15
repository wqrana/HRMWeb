namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SupervisorSubDepartment")]
    public partial class SupervisorSubDepartment : BaseEntity
    {

        [Column("SupervisorSubDepartmentId")]
        public override int Id { get; set; }

        public int? SubDepartmentId { get; set; }
        public SubDepartment SubDepartment { get; set; }

        public int? UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
