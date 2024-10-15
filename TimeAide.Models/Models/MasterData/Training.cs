using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("Training")]
    public partial class Training : BaseEntity
    {
        public Training()
        {
            EmployeeTraining = new HashSet<EmployeeTraining>();
        }
        [Column("TrainingId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Training Name")]
        public string TrainingName { get; set; }
        [Display(Name = "Training Description")]
        public string TrainingDescription { get; set; }
        [NotMapped]
        public string NameAndDescription
        {
            get
            {
                if (String.IsNullOrEmpty(TrainingDescription))
                    return TrainingName;
                else
                    return TrainingName + " - " + TrainingDescription;
            }
        }
        public virtual ICollection<EmployeeTraining> EmployeeTraining { get; set; }
    }
}
