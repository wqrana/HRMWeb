using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeTraining")]
    public partial class EmployeeTraining : BaseUserObjects
    {
        [Column("EmployeeTrainingId")]
        public override int Id { get; set; }

        [Required]
        public int? TrainingId { get; set; }
        //public string Type { get; set; }
        public int? TrainingTypeId { get; set; }
        [Required]
        public DateTime TrainingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Note { get; set; }
       
        public string DocName { get; set; }
        public string DocFilePath { get; set; }
       
        public virtual Training Training { get; set; }
        public virtual TrainingType TrainingType { get; set; }
        public virtual UserInformation UserInformation { get; set; }

    }
}
