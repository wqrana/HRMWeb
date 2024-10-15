namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("TrainingType")]
    public partial class TrainingType : BaseCompanyObjects
    {

        public TrainingType()
        {
            EmployeeTraining = new HashSet<EmployeeTraining>();
        }

        [Column("TrainingTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Training Type")]
        public string TrainingTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string TrainingTypeDescription { get; set; }

        public virtual ICollection<EmployeeTraining> EmployeeTraining { get; set; }
    }
}
