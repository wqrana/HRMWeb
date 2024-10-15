namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TerminationEligibility")]
    public partial class TerminationEligibility : BaseEntity
    {
        
        public TerminationEligibility()
        {
            Employments = new HashSet<Employment>();
        }

        [Display(Name = "Termination Eligibility Id")]
        [Column("TerminationEligibilityId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TerminationEligibilityName { get; set; }

        [StringLength(50)]
        public string TerminationEligibilityDescription { get; set; }

        
        public virtual ICollection<Employment> Employments { get; set; }
    }
}
