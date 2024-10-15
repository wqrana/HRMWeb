namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompensationType")]
    public partial class CompensationType: BaseGlobalEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompensationType()
        {
            //CompensationItem = new HashSet<CompensationItem>();
        }

        [Display(Name = "Id")]
        [Column("CompensationTypeId")]
        public override int Id { get; set; }

        [Required]
        public string CompensationTypeName { get; set; }

        public string CompensationTypeDescription { get; set; }

        //public virtual ICollection<CompensationItem> CompensationItem { get; set; }
    }
}
