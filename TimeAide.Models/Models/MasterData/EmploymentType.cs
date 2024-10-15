namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("EmploymentType")]
    public partial class EmploymentType : BaseCompanyObjects
    {
        
        public EmploymentType()
        {
            EmploymentHistory = new HashSet<EmploymentHistory>();
        }

        [Display(Name = "Employee Type Id")]
        [Column("EmploymentTypeId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Employment Type")]
        public string EmploymentTypeName { get; set; }

        [StringLength(150)]
        [Display(Name = "Description")]
        public string EmploymentTypeDescription { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistory { get; set; }

        public override List<int?> GetRefferredCompanies()
        {
            var list = this.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
