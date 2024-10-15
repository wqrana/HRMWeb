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

    [Table("ReportCriteriaTemplate")]
    public partial class ReportCriteriaTemplate : BaseCompanyObjects
    {
        
        [Column("ReportCriteriaTemplateId")]
        public override int Id { get; set; }
        public int? ReportId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Report Template")]
        public string ReportCriteriaTemplateName { get; set; }       
        public int? CriteriaType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? SuperviorId { get; set; }
        public string EmployeeSelectionIds { get; set; }
        public string DepartmentSelectionIds { get; set; }
        public string SubDepartmentSelectionIds { get; set; }
        public string EmployeeTypeSelectionIds { get; set; }
        public string EmploymentTypeSelectionIds { get; set; }
        public string PositionSelectionIds { get; set; }
        public string StatusSelectionIds { get; set; }
        public string DegreeSelectionIds { get; set; }
        public string TrainingSelectionIds { get; set; }
        public string CredentialSelectionIds { get; set; }
        public string CustomFieldSelectionIds { get; set; }
        public string BenefitSelectionIds { get; set; }
        public string ActionTypeSelectionIds { get; set; }


    }
}