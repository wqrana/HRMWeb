
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeFutureSchedule")]
    public partial class EmployeeFutureSchedule : BaseUserObjects
    {
        [Column("EmployeeFutureScheduleId")]
        public override int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public int BaseScheduleId { get; set; }
        public string Note { get; set; }

        [NotMapped]
        public bool IsBackDated
        {
            get
            {
                if ((StartDate ?? DateTime.Today) < DateTime.Today)
                    return true;
                else
                    return false;
            }
        }
        public virtual BaseSchedule BaseSchedule { get; set; }

    }
}
