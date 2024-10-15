
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("OSHACaseClassification")]
    public partial class OSHACaseClassification : BaseEntity
    {
        [Column("OSHACaseClassificationId")]
        public override int Id { get; set; }
        [Required]
        [Display(Name = "Case Classification")]
        public string OSHACaseClassificationName { get; set; }
        [Display(Name = "Description")]
        public string OSHACaseClassificationDescription { get; set; }
    }
}
