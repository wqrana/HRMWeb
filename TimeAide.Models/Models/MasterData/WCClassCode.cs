namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WCClassCode")]
    public partial class WCClassCode : BaseEntity
    {
        public WCClassCode()
        {
            PayInformationHistory = new HashSet<PayInformationHistory>();
        }
        [Column("WCClassCodeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Class Code")]
        public string ClassCode { get; set; }

        [StringLength(1000)]
        [Display(Name = "Class Description")]
        public string ClassDescription { get; set; }

        public virtual ICollection<PayInformationHistory> PayInformationHistory { get; set; }
    }
}
