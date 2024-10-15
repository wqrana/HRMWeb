namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TerminationType")]
    public partial class TerminationType : BaseEntity
    {
        
        public TerminationType()
        {
            Employments = new HashSet<Employment>();
        }

        [Column("TerminationTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name ="Name")]
        public string TerminationTypeName { get; set; }

        [StringLength(50)]
        [Display(Name = "Description")]
        public string TerminationTypeDescription { get; set; }

        
        public virtual ICollection<Employment> Employments { get; set; }
    }
}
