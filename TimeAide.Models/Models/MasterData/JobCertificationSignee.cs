using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("JobCertificationSignee")]
    public partial class JobCertificationSignee : BaseCompanyObjects
    {
        public JobCertificationSignee()
        {
           
        }

        [Column("JobCertificationSigneeId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }
        public byte[] Signature { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            var list = this.Departments.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
