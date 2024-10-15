
namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DependentStatus")]
    public partial class DependentStatus : BaseEntity
    {
        public DependentStatus()
        {
            EmployeeDependent = new HashSet<EmployeeDependent>();
        }

        [Column("DependentStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string StatusName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public virtual ICollection<EmployeeDependent> EmployeeDependent { get; set; }
    }
}
