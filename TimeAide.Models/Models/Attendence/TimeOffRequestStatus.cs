namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TimeOffRequestStatus : BaseEntity
    {
            
        [Column("TimeOffRequestStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; }
       
    }
}
