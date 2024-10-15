using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("OSHAInjuryClassification")]
    public partial class OSHAInjuryClassification : BaseEntity
    {
        [Column("OSHAInjuryClassificationId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Injury Classification")]
        public string OSHAInjuryClassificationName { get; set; }
        [Display(Name = "Description")]
        public string OSHAInjuryClassificationDescription { get; set; }
    }
}
