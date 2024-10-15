namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TerminationReason")]
    public partial class TerminationReason : BaseEntity
    {
        
        public TerminationReason()
        {
            Employments = new HashSet<Employment>();
        }

        [Display(Name = "Termination Reason Id")]
        [Column("TerminationReasonId")]
        public override int Id { get; set; }


        [Required]
        [StringLength(200)]
        [Display(Name = "Name")]
        public string TerminationReasonName { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string TerminationReasonDescription { get; set; }

        
        public virtual ICollection<Employment> Employments { get; set; }
    }
}
