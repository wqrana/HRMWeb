namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeRotatingSchedule")]
    public partial class EmployeeRotatingSchedule : BaseUserObjects
    {
        [Column("EmployeeRotatingScheduleId")]
        public override int Id { get; set; }
        public int BaseScheduleId { get; set; }

        public string Note { get; set; }
       
        public virtual BaseSchedule BaseSchedule { get; set; }

    }
}
