namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmployeeTimeAndAttendanceSetting")]
    public partial class EmployeeTimeAndAttendanceSetting : BaseUserObjects
    {

        public EmployeeTimeAndAttendanceSetting()
        {
        }

        [Display(Name = "Employee Time And Attendance Setting Id")]
        [Column("EmployeeTimeAndAttendanceSettingId")]
        public override int Id { get; set; }

        public bool EnableWebPunch { get; set; }
    }
}
