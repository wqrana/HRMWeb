namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WitholdingPrePostType")]
    public partial class WitholdingPrePostType : BaseGlobalEntity
    {
        [Display(Name = "WitholdingPrePostTypeId")]
        [Column("WitholdingPrePostTypeId")]
        public override int Id { get; set; }

        [Required]
        public string WitholdingPrePostTypeName { get; set; }

        public string WitholdingPrePostTypeDescription { get; set; }
        
    }
}
