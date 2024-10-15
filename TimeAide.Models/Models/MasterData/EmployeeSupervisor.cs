namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("EmployeeSupervisor")]
    public partial class EmployeeSupervisor: BaseEntity
    {
        [Column("EmployeeSupervisorId")]
        public override int Id { get; set; }

        public int EmployeeUserId { get; set; }

        public int SupervisorUserId { get; set; }

        public virtual UserInformation EmployeeUser { get; set; }

        public virtual UserInformation SupervisorUser { get; set; }

        //[Display(Name = "Employee User")]
        //public int? EmploymentHistoryId { get; set; }
        //public virtual EmploymentHistory EmploymentHistory { get; set; }
    }
}
