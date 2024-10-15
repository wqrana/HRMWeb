using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ChangeRequestStatus : BaseEntity
    {
        
        public ChangeRequestStatus()
        {
        }

        [Column("ChangeRequestStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "ChangeRequestStatus")]
        public string ChangeRequestStatusName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
