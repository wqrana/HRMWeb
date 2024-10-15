using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace TimeAide.Web.Models
{
    [Table("AppraisalResult")]
    public partial class AppraisalResult : BaseCompanyObjects
    {
        public AppraisalResult()
        {
            EmployeeAppraisal = new HashSet<EmployeeAppraisal>();
        }

        [Column("AppraisalResultId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Result Name")]
        public string ResultName { get; set; }

        [Display(Name = "Description")]
        public string ResultDescription { get; set; }
        public virtual ICollection<EmployeeAppraisal> EmployeeAppraisal { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.EmployeeAppraisal.Where(t => t.DataEntryStatus == 1).Select(t => t.AppraisalResult.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
