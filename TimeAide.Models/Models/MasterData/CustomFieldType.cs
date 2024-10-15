namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("CustomFieldType")]
    public partial class CustomFieldType : BaseGlobalEntity
    {

        public CustomFieldType()
        {
            CustomField = new HashSet<CustomField>();
        }

        [Column("CustomFieldTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Custom Field Type")]
        public string CustomFieldTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string CustomFieldTypeDescription { get; set; }

        public virtual ICollection<CustomField> CustomField { get; set; }
    }

    public enum CustomFieldTypes
    {
        Regular = 1,
        Equipment = 2,
    }
}
