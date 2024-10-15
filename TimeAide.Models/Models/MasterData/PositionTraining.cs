using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
  
    [Table("PositionTraining")]
    public partial class PositionTraining : BaseEntity
    {
        [Column("PositionTrainingId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        [Required]
        public int TrainingId { get; set; }

        public virtual Position Position { get; set; }
        public virtual Training Training { get; set; }
    }
}
