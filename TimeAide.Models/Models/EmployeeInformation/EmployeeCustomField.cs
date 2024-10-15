namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmployeeCustomField")]
    public partial class EmployeeCustomField : BaseUserObjects
    {
        public EmployeeCustomField()
        {
            NotificationLog = new HashSet<NotificationLog>();
        }
        [Display(Name = "Employee Custome Id")]
        [Column("EmployeeCustomFieldId")]
        public override int Id { get; set; }
        [Display(Name = "Custom Field")]
        public int CustomFieldId { get; set; }
        [Display(Name = "Field Value")]
        [StringLength(500)]
        public string CustomFieldValue { get; set; }
        [Display(Name = "Field Note")]
        [StringLength(250)]
        public string CustomFieldNote { get; set; }
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }
        public virtual CustomField CustomField { get; set; }
        [Display(Name = "Issuance Date")]
        public DateTime? IssuanceDate { get; set; }
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<NotificationLog> NotificationLog { get; set; }
    }
}
