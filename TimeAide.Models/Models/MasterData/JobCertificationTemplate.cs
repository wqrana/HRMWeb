
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

    [Table("JobCertificationTemplate")]
    public partial class JobCertificationTemplate : BaseCompanyObjects
    {     
       
        [Column("JobCertificationTemplateId")]
        public override int Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Template Type")]
        public int? TemplateTypeId { get; set; }

        [Display(Name = "Template Body")]
        public string TemplateBody { get; set; }
      
    }
}
