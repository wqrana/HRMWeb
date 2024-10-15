 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("EmployeeAppraisal")]
    public partial class EmployeeAppraisal : BaseUserObjects
    {
        public EmployeeAppraisal()
        {
            EmployeeAppraisalDocuments = new HashSet<EmployeeAppraisalDocument>();
            EmployeeAppraisalGoals = new HashSet<EmployeeAppraisalGoal>();
            EmployeeAppraisalSkills = new HashSet<EmployeeAppraisalSkill>();
        }

        [Column("EmployeeAppraisalId")]
        public override int Id { get; set; }
        public int PositionId { get; set; }
        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }
        public int? SubDepartmentId { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int AppraisalTemplateId { get; set; }
        public DateTime? AppraisalReviewDate { get; set; }
        public DateTime? AppraisalDueDate { get; set; }
        public DateTime? EvaluationStartDate { get; set; }
        public DateTime? EvaluationEndDate { get; set; }
        public DateTime? NextAppraisalDueDate { get; set; }
        public int? AppraisalResultId { get; set; }
        public decimal? AppraisalOverallScore { get; set; }
        public decimal? AppraisalTotalMaxValue { get; set; }
        public decimal? AppraisalOverallPct { get; set; }
        public string AppraisalReviewerComments { get; set; }
        public string AppraisalEmployeeComments { get; set; }
        public virtual Position Position { get; set; }
        public virtual Company Company { get; set; }
        public virtual Department Department { get; set; }
        public virtual SubDepartment SubDepartment { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual AppraisalTemplate AppraisalTemplate { get; set; }
        public virtual AppraisalResult AppraisalResult { get; set; }
        public virtual ICollection<EmployeeAppraisalGoal> EmployeeAppraisalGoals { get; set; }
        public virtual ICollection<EmployeeAppraisalSkill> EmployeeAppraisalSkills { get; set; }
        public virtual ICollection<EmployeeAppraisalDocument> EmployeeAppraisalDocuments { get; set; }
        

    }
}
