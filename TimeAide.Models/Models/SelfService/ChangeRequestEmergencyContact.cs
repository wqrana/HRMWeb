using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Web.Models
{
    public class ChangeRequestEmergencyContact : ChangeRequestBase
    {
        [Key]
        [Column("ChangeRequestEmergencyContactId")]
        public override int Id { get; set; }
        public int? EmergencyContactId { get; set; }
        public int? RelationshipId { get; set; }
        [StringLength(50)]
        public string ContactPersonName { get; set; }
        public bool IsDefault { get; set; }
        [StringLength(20)]
        public string MainNumber { get; set; }
        [StringLength(20)]
        public string AlternateNumber { get; set; }
        [NotMapped]
        public virtual string RelationshipName { get; set; }
        public virtual Relationship Relationship { get; set; }

        public int? NewRelationshipId { get; set; }
        [StringLength(50)]
        public string NewContactPersonName { get; set; }
        public bool NewIsDefault { get; set; }
        [StringLength(20)]
        public string NewMainNumber { get; set; }
        [StringLength(20)]
        public string NewAlternateNumber { get; set; }
        [NotMapped]
        public virtual string NewRelationshipName { get; set; }
        public virtual Relationship NewRelationship { get; set; }
        public virtual EmergencyContact EmergencyContact { get; set; }
        public int RequestTypeId { get; set; }
        public string ReasonForDelete { get; set; }

    }
}
