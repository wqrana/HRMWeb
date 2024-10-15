namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Relationship")]
    public partial class Relationship : BaseEntity
    {
        
        public Relationship()
        {
            EmergencyContact = new HashSet<EmergencyContact>();
            EmployeeDependent = new HashSet<EmployeeDependent>();
        }

        [Column("RelationshipId")]
        public override int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string RelationshipName { get; set; }

        [StringLength(100)]
        public string RelationshipDescription { get; set; }

        
        public virtual ICollection<EmergencyContact> EmergencyContact { get; set; }

        
        public virtual ICollection<EmployeeDependent> EmployeeDependent { get; set; }
    }
}
