namespace TimeAide.Web.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("Position")]
    public partial class Position : BaseCompanyObjects
    {
        
        public Position()
        {
            EmploymentHistories = new HashSet<EmploymentHistory>();
        }
        [Column("PositionId")]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PositionName { get; set; }

        [StringLength(200)]
        public string PositionDescription { get; set; }

        [StringLength(50)]
        public string PositionCode { get; set; }

        public int? DefaultPayScaleId { get; set; }
        public virtual PayScale DefaultPayScale { get; set; }

        public int? DefaultEEOCategoryId { get; set; }
        [NotMapped]
        public new bool IsAllCompanies { get; set; }        
        public virtual EEOCategory DefaultEEOCategory { get; set; }
        public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; }
        public virtual ICollection<PositionTraining> PositionTrainings { get; set; }
        public virtual ICollection<PositionCredential> PositionCredentials { get; set; }
        public virtual ICollection<PositionAppraisalTemplate> PositionAppraisalTemplates { get; set; }
        public virtual ICollection<PositionQuestion> PositionQuestions { get; set; }
        public override List<int?> GetRefferredCompanies()
        {
            var list = this.EmploymentHistories.Where(t => t.DataEntryStatus == 1).Select(t => t.CompanyId).Distinct();
            return list.ToList();
        }
    }
}
