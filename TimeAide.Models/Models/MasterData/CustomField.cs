namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomField")]
    public partial class CustomField : BaseCompanyObjects, INotifiable
    {
        public CustomField()
        {
            EmployeeCustomField = new HashSet<EmployeeCustomField>();
        }

        [Column("CustomFieldId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Field Name")]
        public string CustomFieldName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Field Description")]
        public string CustomFieldDescription { get; set; }
        [Display(Name = "Is Expirable")]

        public bool IsExpirable { get; set; }

        [Display(Name = "Field Display Order")]
        public int FieldDisplayOrder { get; set; }
        public virtual ICollection<EmployeeCustomField> EmployeeCustomField { get; set; }
        public int? NotificationScheduleId { get; set; }
        public NotificationSchedule NotificationSchedule { get; set; }
        [Display(Name = "Applies To")]
        public int? DocumentRequiredById { get; set; }
        public virtual DocumentRequiredBy DocumentRequiredBy { get; set; }
        public int? CustomFieldTypeId { get; set; }
        public virtual CustomFieldType CustomFieldType { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            return this.EmployeeCustomField.Where(t => t.DataEntryStatus == 1).Select(t => t.UserInformation.CompanyId).Distinct().ToList();
        }
    }
}
