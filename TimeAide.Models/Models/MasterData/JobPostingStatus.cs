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

    public partial class JobPostingStatus : BaseEntity
    {

     
        //   [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [Column("JobPostingStatusId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Status")]
        public string Name { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string Description { get; set; }
       
    }
}
