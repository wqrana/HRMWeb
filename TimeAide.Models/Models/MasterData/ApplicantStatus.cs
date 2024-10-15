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

    public partial class ApplicantStatus : BaseEntity
    {

        public ApplicantStatus()
        {
           
        }

        //   [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Column("ApplicantStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Status")]
        public string ApplicantStatusName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Use-As-Hire")]
        public bool UseAsHire { get; set; }

        [Display(Name = "Use-As-Apply")]
        public bool? UseAsApply { get; set; }
        [NotMapped]
        public bool UserAsApplyNP { get; set; }

    }
}
