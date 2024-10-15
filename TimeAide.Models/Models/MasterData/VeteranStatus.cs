namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class VeteranStatus : BaseEntity
    {
        
        public VeteranStatus()
        {
            EmployeeVeteranStatus = new HashSet<EmployeeVeteranStatus>();
        }

     //   [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("VeteranStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string VeteranStatusName { get; set; }

        [StringLength(1000)]
        public string VeteranStatusDescription { get; set; }

        
        public virtual ICollection<EmployeeVeteranStatus> EmployeeVeteranStatus { get; set; }
    }
}
