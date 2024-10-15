namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PeriodEntry")]
    public partial class PeriodEntry: BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodEntry()
        {
            EmployeeCompensation = new HashSet<EmployeeCompensation>();
        }


        [Display(Name = "PeriodEntryId")]
        [Column("PeriodEntryId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Period Entry")]
        public string PeriodEntryName { get; set; }

        public string PeriodEntryDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeCompensation> EmployeeCompensation { get; set; }
    }
}
