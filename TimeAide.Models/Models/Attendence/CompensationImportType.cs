namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationImportType")]
    public partial class CompensationImportType: BaseGlobalEntity
    {
        [Display(Name = "CompensationImportTypeId")]
        [Column("CompensationImportTypeId")]
        public override int Id { get; set; }

        [Required]
        public string CompensationImportTypeName { get; set; }

        public string CompensationImportTypeDescription { get; set; }

    }
}
